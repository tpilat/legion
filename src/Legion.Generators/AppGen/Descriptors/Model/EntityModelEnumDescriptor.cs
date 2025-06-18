using Legion.Extensions;
using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;
using System.Data;

namespace Legion.Generators.AppGen.Descriptors;

public class EntityModelEnumDescriptor : EnumDescriptor
{
	public EntityBase Data { get; }

	public EntityModelEnumDescriptor(EntityBase entity, GeneratorContext context)
		: base(context ?? throw new ArgumentNullException(nameof(context)))
	{
		Data = entity ?? throw new ArgumentNullException(nameof(entity));
		Model = Data.Model;

		ConvertEntity = entity.ConvertEntity;
		BaseEntityname = Data.Name;

		Name = $"{Data.Name}Enum";
		FileName = $"{Data.Name}.Enum.cs";

		if (entity.ConvertEntity != ConvertEntity.ToEnum
			&& entity.ConvertEntity != ConvertEntity.ToEnumAndEnumerationClass)
			ModelResult.AddError(
				$"{entity.FullName}: {nameof(ConvertEntity.ToEnumerationClass)}",
				$"Invalid {nameof(entity.ConvertEntity)} == {entity.ConvertEntity.ToString()}");

		if (entity.MainColumn == null)
			ModelResult.AddError(
				$"{entity.FullName}: {nameof(ConvertEntity.ToEnumerationClass)}",
				$"{nameof(entity.MainColumn)} must be set.");
	}

	public override void Initialize(string modelName, string contextName)
	{
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityModel(modelName), "Model"/*, Data.Package.PathPart*/);

	public override string Namespace(string modelName, string contextName)
		//=> $"{Settings.Namespace_EntityModel(modelName)}.{Data.Package.NamespacePart}";
		=> $"{Settings.Namespace_EntityModel(modelName)}.Model";

	public override string BaseNamespace(string modelName, string contextName)
		//=> $"{modelName}.{Data.Package.NamespacePart}";
		=> $"{modelName}.Model";

	public override void SetEnumData()
	{
		var dataSet = Context.SqlReader.LoadAllData(Data.Schema, Data.TableName);
		if (dataSet != null || 0 < dataSet.Tables.Count)
		{
			var dataTable = dataSet.Tables[0];

			var propertyColumnNames = new List<string> { Data.FirstPrimaryKey.ColumnName, Data.MainColumn.ColumnName };

			foreach (var propertyColumnName in propertyColumnNames)
				if (!dataTable.Columns.Contains(propertyColumnName))
					throw new Exception($"{nameof(SetEnumData)}: Table {Data.ID} does not contains column with name {propertyColumnName}");

			var idx = 0;
			foreach (DataRow row in dataTable.Rows)
			{
				idx++;

				var pk = row[Data.FirstPrimaryKey.ColumnName];
				if (pk == null)
					throw new Exception($"{nameof(SetEnumData)}: Table {Data.ID} value for {Data.FirstPrimaryKey.ColumnName} can not be null.");

				var main = row[Data.MainColumn.ColumnName];
				if (main == null)
					throw new Exception($"{nameof(SetEnumData)}: Table {Data.ID} value for {Data.MainColumn.ColumnName} can not be null.");
				//if (obj.GetType() != typeof(string))
				//	throw new Exception($"{nameof(SetEnumData)}: Table {DbObjectName} column type for {Data.MainColumn.ColumnName} must be string.");

				var basePropertyCSharpName = main.ToString().ToCammelCase(removeUnderscores: false, throwIfEmpty: false);
				if (basePropertyCSharpName == Name)
					basePropertyCSharpName = basePropertyCSharpName + "_";

				_enumData.Add(idx, basePropertyCSharpName);
				//_enumData.Add(Utils.Helpers.GuidConverter.GuidToInt32(Guid.Parse(row[Data.FirstPrimaryKey.ColumnName].ToString())), basePropertyCSharpName);
			}
		}
	}
}
