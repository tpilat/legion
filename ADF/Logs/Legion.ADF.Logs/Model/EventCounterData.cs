using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounterData : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<EventCounterData> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdEventCounterData { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Logs.Model.EventCounter.EventCounter | FK_EventCounterData_IdEventCounter
	/// </summary>
	public Guid IdEventCounter { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: double precision NULL
	/// </summary>
	public double? Increment { get; private set; }

	/// <summary>
	/// Database DataType: double precision NULL
	/// </summary>
	public double? Mean { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? Count { get; private set; }

	/// <summary>
	/// Database DataType: double precision NULL
	/// </summary>
	public double? Min { get; private set; }

	/// <summary>
	/// Database DataType: double precision NULL
	/// </summary>
	public double? Max { get; private set; }


	/// <summary>
	/// _1:N Guid IdEventCounter | FK_EventCounterData_IdEventCounter
	/// </summary>
	public Logs.Model.EventCounter EventCounter { get; private set; }

	private EventCounterData()
	{
	}

	static EventCounterData()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<EventCounterData>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdEventCounterData), IdEventCounterData },
			{ nameof(IdEventCounter), IdEventCounter },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
			{ nameof(Increment), Increment },
			{ nameof(Mean), Mean },
			{ nameof(Count), Count },
			{ nameof(Min), Min },
			{ nameof(Max), Max },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdEventCounterData.ToString();
	}

	public override string? ToString()
	{
		return IdEventCounterData.ToString();
	}

	public static ValidatorBuilder<EventCounterData> SetDBValidatorRules(ValidatorBuilder<EventCounterData> builder)
		=> builder
			.ForProperty(x => x.IdEventCounterData, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdEventCounter, v => v.NotDefaultOrEmpty(), (x, parent) => x.EventCounter == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RuntimeUniqueKey, v => v.NotDefaultOrEmpty())
		;
}
