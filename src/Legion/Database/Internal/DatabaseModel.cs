using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class DatabaseModel : IDatabaseModel, IValidable
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private List<IValidationFailure>? _errors;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private List<IValidationFailure>? _configWarnings;

	public DatabaseProviderType ProviderType { get; set; }
	public string Name { get; set; }
	public int? Id { get; set; }
	public string? CollationName { get; set; }
	public string? DefaultSchema { get; set; }
	public DateTime? CreationDate { get; set; }
	public List<Schema> Schemas { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public List<Table> Tables => Schemas.SelectMany(x => x.Tables).ToList();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public List<View> Views => Schemas.SelectMany(x => x.Views).ToList();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public List<Sequence> Sequences => Schemas.SelectMany(x => x.Sequences).ToList();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IValidationFailure>? IDatabaseModel.Errors => _errors;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IValidationFailure>? IDatabaseModel.ConfigWarnings => _configWarnings;

	public DatabaseModel()
	{
		Schemas = [];
	}

	private readonly object _lock = new();
	private bool _built;
	public bool Build()
	{
		if (_built)
			return false;

		lock (_lock)
		{
			if (_built)
				return false;

			var validateResult = Validate();
			_errors = validateResult.Failures?.Where(x => x.Severity == ValidationSeverity.Error).ToList();
			if (_errors?.Count == 0)
				_errors = null;

			_configWarnings = validateResult.Failures?.Where(x => x.Severity == ValidationSeverity.Warning).ToList();
			if (_configWarnings?.Count == 0)
				_configWarnings = null;

			if (0 < _errors?.Count)
				return false;

			_built = true;

			foreach (var schema in Schemas)
				schema.Build(this);

			foreach (var fromSchema in Schemas)
			{
				if (0 < fromSchema.Tables?.Count)
				{
					foreach (var fromTable in fromSchema.Tables.Where(x => 0 < x.ForeignKeys?.Count))
					{
						foreach (var foreignKey in fromTable.ForeignKeys!)
						{
							var fromColumn = fromTable.Columns.FirstOrDefault(x => x.Name == foreignKey.Column);
							if (fromColumn == null)
							{
								AddError(ValidationFailureFactory<DatabaseModel>.CreateError(Exceptions.Internal.ErrorCodes.DatabaseModelException.InvalidForeignKey($"Invalid FK: {fromSchema.Name}.{fromTable.Name}.{foreignKey.Name} | {nameof(fromColumn)} == null")));
								continue;
							}

							var toSchema = Schemas.FirstOrDefault(x => x.Alias == foreignKey.ForeignSchemaAlias);
							var toTable = toSchema?.Tables?.FirstOrDefault(x => x.Name == foreignKey.ForeignTableName);
							var toColumn = toTable?.Columns.FirstOrDefault(x => x.Name == foreignKey.ForeignColumnName);
							if (toColumn == null)
							{
								AddError(ValidationFailureFactory<DatabaseModel>.CreateError(Exceptions.Internal.ErrorCodes.DatabaseModelException.InvalidForeignKey($"Invalid FK: {fromSchema.Name}.{fromTable.Name}.{foreignKey.Name} | {nameof(toColumn)} == null")));
								continue;
							}

							foreignKey.SetFromColumn(fromColumn);
							foreignKey.SetToColumn(toColumn);
							fromColumn.SetTargetForeignKey(foreignKey);
							toColumn.AddSourceForeignKey(foreignKey);
						}
					}
				}
			}

			if (0< _errors?.Count)
				return false;
		}

		return _errors == null || _errors.Count == 0;
	}

	public void AddSchema(Schema schema)
	{
		Throw.IfArgumentNull(schema);

		Schemas ??= [];
		Schemas.Add(schema);
	}

	public void AddError(IValidationFailure error)
	{
		Throw.IfArgumentNull(error);

		_errors ??= [];
		_errors.Add(error);
	}

	public static void SetValidatorRules(
		ValidatorBuilder<DatabaseModel> builder,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		builder
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
			.ForEach(x => x.Schemas, Schema.SetValidatorRules, globalValidationState: globalValidationState)
		;
	}

	public IValidationResult Validate(Dictionary<string, object>? globalValidationState = null)
	{
		var builder = new ValidatorBuilder<DatabaseModel>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public DatabaseModel Clone()
		=> new()
		{
			ProviderType = ProviderType,
			Name = Name,
			Schemas = Schemas?.Select(x => x.Clone()).ToList()!
		};

	public override string? ToString()
	{
		return Name;
	}
}
