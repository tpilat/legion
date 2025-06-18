using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class PrimaryKey : IPrimaryKey, IValidable
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private Table _table;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private List<Column> _columns;

	public string Name { get; set; }
	public List<string> Columns { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	ITable IPrimaryKey.Table => _table;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IColumn> IPrimaryKey.Columns => _columns;

	public PrimaryKey()
	{
		Columns = [];
		_columns = [];
	}

	private bool _initialized;
	public bool Initialize(Table table)
	{
		if (_initialized)
			return false;

		Throw.IfArgumentNull(table);

		_initialized = true;

		_table = table;

		return true;
	}

	internal PrimaryKey AddColumn(Column column)
	{
		Throw.IfArgumentNull(column);

		_columns ??= [];
		_columns.Add(column);
		column.IsPrimaryKey = true;
		return this;
	}
	public static void SetValidatorRules(
		ValidatorBuilder<PrimaryKey> builder,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		builder
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.Columns, v => v.NotDefaultOrEmpty())
			.ForEach(x => x.Columns, (item, globalState, localState) =>
			{
				item.WithError(
					(x, parent) => string.IsNullOrWhiteSpace(x),
					x => Exceptions.Internal.ErrorCodes.EmptyValueException.NotWhiteSpaceStringValidation(Validation.Validators.PropertyValidators.NotDefaultOrEmptyValidator.GetResourceMessage()));
			})
		;
	}

	public IValidationResult Validate(Dictionary<string, object>? globalValidationState = null)
	{
		var builder = new ValidatorBuilder<PrimaryKey>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public PrimaryKey Clone()
		=> new()
		{
			Name = Name,
			Columns = Columns?.ToList()!
		};

	public override string? ToString()
	{
		return $"{Name}: {string.Join(", ", Columns ?? new List<string>())}";
	}
}
