using Legion.Database.Metamodel;
using Legion.Validation;

namespace Legion.Database.Internal;

public class Schema : ISchema, IValidable
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private DatabaseModel _model;

	public string Name { get; set; }
	public string Alias { get; set; }
	public int? Id { get; set; }
	public List<Table>? Tables { get; set; }
	public List<View>? Views { get; set; }
	public List<Sequence>? Sequences { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IDatabaseModel ISchema.Model => _model;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<ITable>? ISchema.Tables => Tables?.Cast<ITable>()!;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IEnumerable<IView>? ISchema.Views => Views?.Cast<IView>()!;

	public Schema()
	{
		Tables = [];
		Views = [];
		Sequences = [];
	}

	private bool _built;
	public bool Build(DatabaseModel model)
	{
		if (_built)
			return false;

		Throw.IfArgumentNull(model);

		_built = true;

		_model = model;

		Tables ??= [];
		if (0 < Tables?.Count)
			foreach (var table in Tables)
				table.Build(this);

		Views ??= [];
		if (0 < Views?.Count)
			foreach (var view in Views)
				view.Build(this);

		Sequences ??= [];
		if (0 < Sequences?.Count)
			foreach (var sequence in Sequences)
				sequence.Build(this);

		return true;
	}

	public void AddError(IValidationFailure error)
		=> _model.AddError(error);

	public static void SetValidatorRules(
		ValidatorBuilder<Schema> builder,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		builder
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
			.ForProperty(x => x.Alias, v => v.NotDefaultOrWhiteSpace())
			.ForEach(x => x.Tables, Table.SetValidatorRules, globalValidationState: globalValidationState)
			.ForEach(x => x.Views, View.SetValidatorRules, globalValidationState: globalValidationState)
			.ForEach(x => x.Sequences, Sequence.SetValidatorRules, globalValidationState: globalValidationState)
		;
	}

	public IValidationResult Validate(Dictionary<string, object>? globalValidationState = null)
	{
		var builder = new ValidatorBuilder<Schema>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public Schema Clone()
		=> new()
		{
			Name = Name,
			Alias = Alias,
			Tables = Tables?.Select(x => x.Clone()).ToList(),
			Views = Views?.Select(x => x.Clone()).ToList(),
			Sequences = Sequences?.Select(x => x.Clone()).ToList()
		};

	public override string? ToString()
	{
		return Name;
	}
}
