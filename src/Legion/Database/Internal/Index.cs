using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class Index : IIndex, IValidable
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
	public bool IsUnique { get; set; }

	public bool IsPrimary { get; set; }
	public List<string> Columns { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	ITable IIndex.Table => _table;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IColumn> IIndex.Columns => _columns;

	public Index()
	{
		Columns = [];
		_columns = [];
	}

	private bool built;
	public bool Build(Table table)
	{
		if (built)
			return false;

		Throw.IfArgumentNull(table);

		built = true;

		_table = table;

		return true;
	}

	internal Index AddColumn(Column column)
	{
		Throw.IfArgumentNull(column);

		_columns ??= [];
		_columns.Add(column);
		return this;
	}

	public static void SetValidatorRules(
		ValidatorBuilder<Index> builder,
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
		var builder = new ValidatorBuilder<Index>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public Index Clone()
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
