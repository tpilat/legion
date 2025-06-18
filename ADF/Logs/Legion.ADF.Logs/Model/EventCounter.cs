using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounter : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	private List<Logs.Model.EventCounterData> _eventCounterDatas;

	public static IValidator<EventCounter> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdEventCounter { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Logs.Model.EventCounterCategory.EventCounterCategory | FK_EventCounter_IdEventCounterCategory
	/// </summary>
	public Guid IdEventCounterCategory { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string DisplayName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string CounterType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NULL
	/// </summary>
	public string? DisplayRateTimeScale { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Metadata { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NULL
	/// </summary>
	public string? DisplayUnits { get; private set; }


	/// <summary>
	/// _1:N Guid IdEventCounterCategory | FK_EventCounter_IdEventCounterCategory
	/// </summary>
	public Logs.Model.EventCounterCategory EventCounterCategory { get; private set; }


	/// <summary>
	/// N:_1 Logs.Model.EventCounterData.IdEventCounter | FK_EventCounterData_IdEventCounter
	/// </summary>
	public IReadOnlyList<Logs.Model.EventCounterData> EventCounterDatas => _eventCounterDatas;

	private EventCounter()
	{
		_eventCounterDatas = [];
	}

	static EventCounter()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<EventCounter>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdEventCounter), IdEventCounter },
			{ nameof(IdEventCounterCategory), IdEventCounterCategory },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
			{ nameof(DisplayName), DisplayName },
			{ nameof(CounterType), CounterType },
			{ nameof(DisplayRateTimeScale), DisplayRateTimeScale },
			{ nameof(Metadata), Metadata },
			{ nameof(DisplayUnits), DisplayUnits },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
		DisplayName = Legion.Text.StringHelper.TrimToFitMaxLength(DisplayName, 127, postfix);
		CounterType = Legion.Text.StringHelper.TrimToFitMaxLength(CounterType, 63, postfix);
		DisplayRateTimeScale = Legion.Text.StringHelper.TrimToFitMaxLength(DisplayRateTimeScale, 31, postfix);
		DisplayUnits = Legion.Text.StringHelper.TrimToFitMaxLength(DisplayUnits, 31, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdEventCounter.ToString();
	}

	public override string? ToString()
	{
		return IdEventCounter.ToString();
	}

	public static ValidatorBuilder<EventCounter> SetDBValidatorRules(ValidatorBuilder<EventCounter> builder)
		=> builder
			.ForProperty(x => x.IdEventCounter, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdEventCounterCategory, v => v.NotDefaultOrEmpty(), (x, parent) => x.EventCounterCategory == null)
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.DisplayName, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.CounterType, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.DisplayRateTimeScale, v => v.MaxLength(31))
			.ForProperty(x => x.DisplayUnits, v => v.MaxLength(31))
		;
}
