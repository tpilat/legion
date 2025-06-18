using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounterCategory : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	private List<Logs.Model.EventCounter> _eventCounters;

	public static IValidator<EventCounterCategory> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdEventCounterCategory { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Source { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string DisplayName { get; private set; }


	/// <summary>
	/// N:_1 Logs.Model.EventCounter.IdEventCounterCategory | FK_EventCounter_IdEventCounterCategory
	/// </summary>
	public IReadOnlyList<Logs.Model.EventCounter> EventCounters => _eventCounters;

	private EventCounterCategory()
	{
		_eventCounters = [];
	}

	static EventCounterCategory()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<EventCounterCategory>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdEventCounterCategory), IdEventCounterCategory },
			{ nameof(Source), Source },
			{ nameof(DisplayName), DisplayName },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Source = Legion.Text.StringHelper.TrimToFitMaxLength(Source, 127, postfix);
		DisplayName = Legion.Text.StringHelper.TrimToFitMaxLength(DisplayName, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdEventCounterCategory.ToString();
	}

	public override string? ToString()
	{
		return IdEventCounterCategory.ToString();
	}

	public static ValidatorBuilder<EventCounterCategory> SetDBValidatorRules(ValidatorBuilder<EventCounterCategory> builder)
		=> builder
			.ForProperty(x => x.IdEventCounterCategory, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Source, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.DisplayName, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
