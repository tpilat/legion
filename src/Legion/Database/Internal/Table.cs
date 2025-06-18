using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class Table : ITable, IValidable
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private Schema _schema;

	public string Name { get; set; }
	public int? Id { get; set; }
	public List<Column> Columns { get; set; }
	public PrimaryKey? PrimaryKey { get; set; }
	public List<ForeignKey>? ForeignKeys { get; set; }
	public List<UniqueConstraint>? UniqueConstraints { get; set; }
	public List<Index>? Indexes { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public string Alias => _schema.Alias;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public ISchema Schema => _schema;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IColumn> ITable.Columns => Columns;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IPrimaryKey? ITable.PrimaryKey => PrimaryKey;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IForeignKey>? ITable.ForeignKeys => ForeignKeys;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IUniqueConstraint>? ITable.UniqueConstraints => UniqueConstraints;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IIndex>? ITable.Indexes => Indexes;

	public Table()
	{
		Columns = [];
		ForeignKeys = [];
		UniqueConstraints = [];
		Indexes = [];
	}

	private bool _built;
	public bool Build(Schema schema)
	{
		if (_built)
			return false;

		Throw.IfArgumentNull(schema);

		_schema = schema;

		_built = true;

		Columns ??= [];
		if (0 < Columns.Count)
			foreach (var column in Columns)
				column.Build(this);

		if (PrimaryKey != null)
		{
			foreach (var columnName in PrimaryKey.Columns)
			{
				var column = Columns?.FirstOrDefault(x => x.Name == columnName);
				if (column == null)
				{
					_schema.AddError(ValidationFailureFactory<Table>.CreateError(Exceptions.Internal.ErrorCodes.DatabaseModelException.InvalidPrimarynKey($"Invalid PK {_schema.Name}.{Name}.{PrimaryKey.Name} | Column = {columnName}")));
					continue;
				}

				column.SetPrimaryKey(PrimaryKey);
				PrimaryKey.AddColumn(column);
			}
		}

		ForeignKeys ??= [];
		if (0 < ForeignKeys.Count)
			foreach (var foreignKey in ForeignKeys)
				foreignKey.Build(this);

		UniqueConstraints ??= [];
		if (0 < UniqueConstraints.Count)
			foreach (var uniqueConstraint in UniqueConstraints)
			{
				foreach (var columnName in uniqueConstraint.Columns)
				{
					var column = Columns?.FirstOrDefault(x => x.Name == columnName);
					if (column == null)
					{
						_schema.AddError(ValidationFailureFactory<Table>.CreateError(Exceptions.Internal.ErrorCodes.DatabaseModelException.InvalidUnique($"Invalid UniqueConstraint {_schema.Name}.{Name}.{uniqueConstraint.Name} | Column = {columnName}")));
						continue;
					}

					column.AddUniqueConstraint(uniqueConstraint);
					uniqueConstraint.AddColumn(column);
				}
				uniqueConstraint.Build(this);
			}

		Indexes ??= [];
		if (0 < Indexes.Count)
			foreach (var index in Indexes)
			{
				foreach (var columnName in index.Columns)
				{
					var column = Columns?.FirstOrDefault(x => x.Name == columnName);
					if (column == null)
					{
						_schema.AddError(ValidationFailureFactory<Table>.CreateError(Exceptions.Internal.ErrorCodes.DatabaseModelException.InvalidIndex($"Invalid Index {_schema.Name}.{Name}.{index.Name} | Column = {columnName}")));
						continue;
					}

					column.AddIndex(index);
					index.AddColumn(column);
				}
				index.Build(this);
			}

		return true;
	}

	public static void SetValidatorRules(
		ValidatorBuilder<Table> builder,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		builder
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
			.ForEach(x => x.Columns, Column.SetValidatorRules, globalValidationState: globalValidationState)
			.ForNavigation(x => x.PrimaryKey, PrimaryKey.SetValidatorRules, globalValidationState: globalValidationState)
			.ForEach(x => x.UniqueConstraints, UniqueConstraint.SetValidatorRules, globalValidationState: globalValidationState)
			.ForEach(x => x.Indexes, Index.SetValidatorRules, globalValidationState: globalValidationState)
			.ForEach(x => x.ForeignKeys, ForeignKey.SetValidatorRules, globalValidationState: globalValidationState)
		;
	}

	public IValidationResult Validate(Dictionary<string, object>? globalValidationState = null)
	{
		var builder = new ValidatorBuilder<Table>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public Table Clone()
		=> new()
		{
			Name = Name,
			Columns = Columns?.Select(x => x.Clone()).ToList()!,
			PrimaryKey = PrimaryKey?.Clone(),
			ForeignKeys = ForeignKeys?.Select(x => x.Clone()).ToList()!,
			UniqueConstraints = UniqueConstraints?.Select(x => x.Clone()).ToList()!,
			Indexes = Indexes?.Select(x => x.Clone()).ToList()!
		};

	public override string? ToString()
	{
		return Name;
	}
}
