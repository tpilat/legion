using Legion.Validation;

namespace Legion.Database.Internal;

public class Sequence
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	private Schema _schema;

	/// <summary>
	///     The schema that contains the sequence, or <c>null</c> to use the default schema.
	/// </summary>
	public Schema Schema { get; set; }

	/// <summary>
	///     The sequence name.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	///     The database/store type of the sequence, or <c>null</c> if not set.
	/// </summary>
	public string StoreType { get; set; }

	public Type CsharpType { get; set; }

	/// <summary>
	///     The start value for the sequence, or <c>null</c> if not set.
	/// </summary>
	public long? StartValue { get; set; }

	/// <summary>
	///     The amount to increment by to generate the next value in, the sequence, or <c>null</c> if not set.
	/// </summary>
	public int? IncrementBy { get; set; }

	/// <summary>
	///     The minimum value supported by the sequence, or <c>null</c> if not set.
	/// </summary>
	public long? MinValue { get; set; }

	/// <summary>
	///     The maximum value supported by the sequence, or <c>null</c> if not set.
	/// </summary>
	public long? MaxValue { get; set; }

	/// <summary>
	///     Indicates whether or not the sequence will start over when the max value is reached, or <c>null</c> if not set.
	/// </summary>
	public bool? IsCyclic { get; set; }

	private bool built;
	public bool Build(Schema schema)
	{
		if (built)
			return false;

		Throw.IfArgumentNull(schema);

		built = true;

		_schema = schema;

		return true;
	}

	public static void SetValidatorRules(
		ValidatorBuilder<Sequence> builder,
		Dictionary<string, object>? globalValidationState = null,
		Dictionary<string, object>? localValidationState = null)
	{
		builder
			.ForProperty(x => x.Name, v => v.NotDefaultOrWhiteSpace())
		;
	}

	public IValidationResult Validate(Dictionary<string, object>? globalValidationState = null)
	{
		var builder = new ValidatorBuilder<Sequence>();
		SetValidatorRules(builder, globalValidationState, localValidationState: null);
		var validator = builder.Build();

		return validator.Validate(this);
	}

	public Sequence Clone()
		=> new()
		{
			Name = Name,
			StoreType = StoreType,
			CsharpType = CsharpType,
			IncrementBy = IncrementBy,
			IsCyclic = IsCyclic,
			MaxValue = MaxValue,
			MinValue = MinValue,
			StartValue = StartValue
		};

	public override string? ToString()
	{
		return Name;
	}
}
