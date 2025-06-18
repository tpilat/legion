using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class UniqueConstraint : IUniqueConstraint, IValidable
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
	public ITable Table => _table;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IColumn> IUniqueConstraint.Columns => _columns;

	public UniqueConstraint()
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

		if (_columns.Count == 1)
			_columns[0].IsSingleUniqueConstraint = true;
		else
			foreach (var column in _columns)
				column.IsMultiUniqueConstraint = true;

		return true;
	}

	public UniqueConstraint AddColumn(Column column)
	{
		Throw.IfArgumentNull(column);

		_columns ??= [];
		_columns.Add(column);
		return this;
	}

	public static void SetValidatorRules(
		ValidatorBuilder<UniqueConstraint> builder,
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
		var builder = new ValidatorBuilder<UniqueConstraint>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public UniqueConstraint Clone()
		=> new()
		{
			Name = Name,
			Columns = Columns?.ToList()!
		};

	public override string? ToString()
	{
		return $"{Name}: {string.Join(", ", Columns ?? [])}";
	}
}
