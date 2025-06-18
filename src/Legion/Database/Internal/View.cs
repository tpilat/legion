using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class View : IView, IValidable
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
	public string Definition { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public string? Alias => _schema?.Name;

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
	IEnumerable<IColumn> IView.Columns => Columns;

	public View()
	{
		Columns = [];
	}

	private bool built;
	public bool Build(Schema schema)
	{
		if (built)
			return false;

		Columns ??= [];
		if (0 < Columns.Count)
			foreach (var column in Columns)
				column.Build(this);

		Throw.IfArgumentNull(schema);

		built = true;

		_schema = schema;

		return true;
	}

	public static void SetValidatorRules(
		ValidatorBuilder<View> builder,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		builder
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
			.ForEach(x => x.Columns, Column.SetValidatorRules, globalValidationState: globalValidationState)
			.ForProperty(x => x.Definition, v => v.NotDefaultOrWhiteSpace())
		;
	}

	public IValidationResult Validate(Dictionary<string, object>? globalValidationState = null)
	{
		var builder = new ValidatorBuilder<View>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public View Clone()
		=> new()
		{
			Name = Name,
			Columns = Columns?.Select(x => x.Clone()).ToList()!,
			Definition = Definition
		};

	public override string? ToString()
	{
		return Name;
	}
}
