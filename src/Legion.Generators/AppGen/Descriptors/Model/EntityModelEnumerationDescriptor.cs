using Legion.Extensions;
using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;
using Legion.Text;
using System.Data;

namespace Legion.Generators.AppGen.Descriptors;

public class EnumDbRow
{
	public string PrimaryKeyCSharpName { get; set; }
	public string PrimaryKeyCSharpType { get; set; }
	public string BasePropertyCSharpName { get; set; }
	public string BasePropertyCSharpName_NewObject { get; set; }
	public List<string> Values { get; set; }

	public EnumDbRow(string primaryKeyCSharpName, string primaryKeyCSharpType, string basePropertyCSharpName)
	{
		PrimaryKeyCSharpName = primaryKeyCSharpName;
		PrimaryKeyCSharpType = primaryKeyCSharpType;
		BasePropertyCSharpName = basePropertyCSharpName;
		BasePropertyCSharpName_NewObject = $"{basePropertyCSharpName}_NewObject";
		Values = new List<string>();
	}
}

public class EntityModelEnumerationDescriptor : EntityBaseDescriptor
{
	private BaseEntityDescriptor _baseEntityDesc;
	public BaseEntityDescriptor BaseEntityDesc => _baseEntityDesc ?? (_baseEntityDesc = Context.BaseEntityDescriptors[Data.BaseEntityName]);
	public string GetBaseEntityName(string modelName, string contextName) => Data.CustomBaseEntityName ?? BaseEntityDesc.BaseName(modelName, contextName);
	public List<EnumDbRow> EnumData { get; private set; }
	public string EnumerationFileName { get; set; }
	public string EnumName { get; set; }
	public ConvertEntity ConvertEntity { get; set; }

	public EntityModelEnumerationDescriptor(EntityBase entity, GeneratorContext context)
		: base(
			  entity ?? throw new ArgumentNullException(nameof(entity)),
			  context ?? throw new ArgumentNullException(nameof(context)))
	{
		Model = Data.Model;

		EnumData = new List<EnumDbRow>();

		ConvertEntity = entity.ConvertEntity;

		Name = Data.Name;
		EnumName = $"{Name}Enum";
		EnumerationFileName = $"{Name}.Partial.EnumValues.cs";
		FileName = $"{Name}.Partial.EnumKeys.cs";

		this.AddImports(new List<string> //TODO generate as enum / EnumerationClass
		{
		});

		this.AddImports(Data.Properties.SelectMany(p => p.Namespaces).Where(x => x != "System"));

		if (ConvertEntity != ConvertEntity.ToEnumerationClass
			&& ConvertEntity != ConvertEntity.ToEnumAndEnumerationClass)
			ModelResult.AddError(
				$"{entity.FullName}: {nameof(ConvertEntity.ToEnumerationClass)}",
				$"Invalid {nameof(entity.ConvertEntity)} == {entity.ConvertEntity}");

		if (entity.MainColumn == null)
			ModelResult.AddError(
				$"{entity.FullName}: {nameof(ConvertEntity.ToEnumerationClass)}",
				$"{nameof(entity.MainColumn)} must be set.");
	}

	public override void Initialize(string modelName, string contextName)
	{
		this.BuildImports(modelName, contextName);
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityModel(modelName), "Model"/*, Data.Package.PathPart*/);

	public override string Namespace(string modelName, string contextName)
		//=> $"{Settings.Namespace_EntityModel(modelName)}.{Data.Package.NamespacePart}";
		=> $"{Settings.Namespace_EntityModel(modelName)}.Model";

	public override string BaseNamespace(string modelName, string contextName)
		//=> $"{modelName}.{Data.Package.NamespacePart}";
		=> $"{modelName}.Model";

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		SetEnumData();

		if (EnumData.Count == 0)
			ModelResult.AddError($"{Data.FullName}: {nameof(EntityModelEnumerationDescriptor)}", "Enumeration has no values.");

		var result =
			GeneratorInvoker
				.Generate<EntityModelEnumerationGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EntityModelEnumerationDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		ModelResult.MergeAllMessages(result);

		result =
			GeneratorInvoker
				.Generate<EntityModelEnumerationObjectGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EntityModelEnumerationDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}

	private void SetEnumData()
	{
		try
		{
			List<int> pkList = new List<int>();

			var dataSet = Context.SqlReader.LoadAllData(Data.Schema, Data.TableName);
			if (dataSet != null || 0 < dataSet.Tables.Count)
			{
				var dataTable = dataSet.Tables[0];

				var propertyColumnNames = Data.Properties.Select(p => p.ColumnName).ToList();
				foreach (var propertyColumnName in propertyColumnNames)
					if (!dataTable.Columns.Contains(propertyColumnName))
						throw new Exception($"{nameof(SetEnumData)}: Table {Data.ID} does not contains column with name {propertyColumnName}");

				foreach (DataRow row in dataTable.Rows)
				{
					var pkObj = row[Data.FirstPrimaryKey.ColumnName];
					if (pkObj == null)
						throw new Exception($"{nameof(SetEnumData)}: Table {Data.ID} value for {Data.FirstPrimaryKey.ColumnName} can not be null.");

					var obj = row[Data.MainColumn.ColumnName];
					if (obj == null)
						throw new Exception($"{nameof(SetEnumData)}: Table {Data.ID} value for {Data.MainColumn.ColumnName} can not be null.");
					//if (obj.GetType() != typeof(string))
					//	throw new Exception($"{nameof(SetEnumData)}: Table {DbObjectName} column type for {Data.MainColumn.ColumnName} must be string.");

					var basePropertyCSharpName = obj.ToString().ToCammelCase(removeUnderscores: false, throwIfEmpty: false);
					if (basePropertyCSharpName == Name)
						basePropertyCSharpName = basePropertyCSharpName + "_";

					var enumDbRow = new EnumDbRow(Data.FirstPrimaryKey.Name, Data.FirstPrimaryKey.CSharpType, basePropertyCSharpName);
					foreach (var propertyColumnName in propertyColumnNames)
					{
						//if (
						//	//(DbSchemaName?.ToLower().Equals((CSharpCodeGeneratorConfig.SchemaNameForActivityTable ?? "mbs")?.ToLower(), StringComparison.OrdinalIgnoreCase) ?? false)
						//	//&& (DbObjectName?.ToLower().Equals((CSharpCodeGeneratorConfig.ActivityTableName ?? "Activity")?.ToLower(), StringComparison.OrdinalIgnoreCase) ?? false)
						//	Data.IsActivityEntity
						//	&& propertyColumnName.ToLower() == "token")
						//	enumDbRow.Values.Add($"{Data.Model.Settings.ActivityTableName}.{row[propertyColumnName]}.ToString()");
						//else
						//{
						var val = StringHelper.GenerateLiteral((object)row[propertyColumnName], true);
						val = val.Replace(Environment.NewLine, "\"" + Environment.NewLine + "+ \"");
						enumDbRow.Values.Add(val);
						//}
					}
					EnumData.Add(enumDbRow);

					if (Data.FirstPrimaryKey.UnderlyingNullableType == typeof(int))
						pkList.Add((int)row[Data.FirstPrimaryKey.ColumnName]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception($"{nameof(SetEnumData)}: Exception occured while loading data from db, from table {Data.ID}", ex);
		}
	}

	//private void AddEntities()
	//{
	//	int id = Consts.EntityStartID;
	//	foreach (var entity in Data.Model.Entities)
	//	{
	//		var name = entity.Name == "Entity" ? "Entity_" : entity.Name;
	//		if (!EnumData.Any(ed => ed.BasePropertyCSharpName == name))
	//			EnumData.Add(
	//				new EnumDbRow("IdEntity", "int", name)
	//				{
	//					Values = new List<string>
	//					{
	//						StringHelper.GenerateLiteral(id),
	//						StringHelper.GenerateLiteral(entity.Name)
	//					}
	//				});
	//		id++;
	//	}

	//	id = Consts.QueryEntityStartID;
	//	foreach (var queryEntity in Data.Model.QueryEntities)
	//	{
	//		var name = queryEntity.Name == "Entity" ? "Entity_" : queryEntity.Name;
	//		if (!EnumData.Any(ed => ed.BasePropertyCSharpName == name))
	//			EnumData.Add(
	//				new EnumDbRow("IdEntity", "int", name)
	//				{
	//					Values = new List<string>
	//					{
	//						StringHelper.GenerateLiteral(id),
	//						StringHelper.GenerateLiteral(queryEntity.Name)
	//					}
	//				});
	//		id++;
	//	}
	//}

	//private void AddActivities()
	//{
	//	foreach (var controllerDesc in mvcControllerDescriptors)
	//	{
	//		AddActivity(controllerDesc.ControllerDto.Activity);
	//		foreach (var action in controllerDesc.ControllerDto.Actions)
	//		{
	//			AddActivity(action.Activity);
	//		}
	//	}
	//}

	//private void AddActivity(ActivityDto dto)
	//{
	//	if (dto == null || dto.IsSubActivity)
	//		return;

	//	if (!EnumData.Any(ed => ed.BasePropertyCSharpName == dto.Token))
	//	{
	//		EnumData.Add(
	//			new EnumDbRow(dto.Token)
	//			{
	//				Values = new List<string>
	//				{
	//						CSharpCodeUtilities.GenerateLiteral(EnumData.Count + 1),
	//						$"{CSharpCodeUtilities.ActivityEnumName}.{dto.Token}.ToString()",
	//						//(dto.IdEntityType <= 0 ? "null" : CSharpCodeUtilities.GenerateLiteral(dto.IdEntityType)),
	//						//CSharpCodeUtilities.GenerateLiteral((int)dto.CommandExecutionType),
	//						//CSharpCodeUtilities.GenerateLiteral(dto.Order),
	//						//(string.IsNullOrWhiteSpace(dto.CssClass) ? "null" : CSharpCodeUtilities.GenerateLiteral(dto.CssClass))
	//				}
	//			});
	//	}
	//}

	//private void AddEntityOperations()
	//{
	//	int i = -1;
	//	foreach (var entity in Data.Model.Entities)
	//	{
	//		i++;
	//		int idEntity = Consts.EntityStartID + i;

	//		AddOperation(idEntity, CSharpCodeUtilities.Service_OperationName(CSharpCodeUtilities.Service_GetFirstMethodName(entity)));
	//		AddOperation(idEntity, CSharpCodeUtilities.Service_OperationName(CSharpCodeUtilities.Service_GetByExpressionMethodName(entity)));
	//		if (entity.HasCreatePermission)
	//			AddOperation(idEntity, CSharpCodeUtilities.Service_OperationName(CSharpCodeUtilities.Service_InsertMethodName(entity)));
	//		if (entity.HasUpdatePermission)
	//			AddOperation(idEntity, CSharpCodeUtilities.Service_OperationName(CSharpCodeUtilities.Service_UpdateMethodName(entity)));
	//		if (entity.HasDeletePermission)
	//			AddOperation(idEntity, CSharpCodeUtilities.Service_OperationName(CSharpCodeUtilities.Service_DeleteMethodName(entity)));
	//		AddOperation(idEntity, CSharpCodeUtilities.Service_OperationName(CSharpCodeUtilities.Service_IsReferencedMethodName(entity)));
	//	}
	//}

	//private void AddQueryOperations()
	//{
	//	int i = -1;
	//	foreach (var queryEntity in Data.Model.QueryEntities)
	//	{
	//		i++;
	//		int idEntity = Consts.QueryEntityStartID + i;

	//		AddOperation(idEntity, CSharpCodeUtilities.QueryService_OperationName(CSharpCodeUtilities.QueryService_GetIQueryableMethodName(queryEntity)));
	//		AddOperation(idEntity, CSharpCodeUtilities.QueryService_OperationName(CSharpCodeUtilities.QueryService_GetFirstMethodName(queryEntity)));
	//		AddOperation(idEntity, CSharpCodeUtilities.QueryService_OperationName(CSharpCodeUtilities.QueryService_GetByExpressionMethodName(queryEntity)));
	//	}
	//}
}
