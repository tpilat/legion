using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxQueueProcessingMode : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	private List<Outbox.Model.OutboxQueue> _outboxQueues;
	private List<Outbox.Model.OutboxQueue> _suspendingModeOutboxQueues;

	public static IValidator<OutboxQueueProcessingMode> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxQueueProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Outbox.Model.OutboxQueue.IdProcessingMode | FK_OutboxQueue_IdProcessingMode
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxQueue> OutboxQueues => _outboxQueues;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxQueue.IdSuspendingMode | FK_OutboxQueue_IdSuspendingMode
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxQueue> SuspendingModeOutboxQueues => _suspendingModeOutboxQueues;

	private OutboxQueueProcessingMode()
	{
		_outboxQueues = [];
		_suspendingModeOutboxQueues = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxQueueProcessingMode), IdOutboxQueueProcessingMode },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOutboxQueueProcessingMode.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<OutboxQueueProcessingMode> SetDBValidatorRules(ValidatorBuilder<OutboxQueueProcessingMode> builder)
		=> builder
			.ForProperty(x => x.IdOutboxQueueProcessingMode, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
