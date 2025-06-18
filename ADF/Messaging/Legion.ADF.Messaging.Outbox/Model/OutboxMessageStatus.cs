using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageStatus : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	private List<Outbox.Model.OutboxMessageArchive> _outboxMessageArchives;
	private List<Outbox.Model.OutboxMessageProcessingLog> _outboxMessageProcessingLogs;
	private List<Outbox.Model.OutboxMessage> _outboxMessages;

	public static IValidator<OutboxMessageStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageArchive.IdOutboxMessageStatus | FK_OutboxMessageArchive_IdOutboxMessageStatus
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageArchive> OutboxMessageArchives => _outboxMessageArchives;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageProcessingLog.IdOutboxMessageStatus | FK_OutboxMessageProcessingLog_IdOutboxMessageStatus
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageProcessingLog> OutboxMessageProcessingLogs => _outboxMessageProcessingLogs;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessage.IdOutboxMessageStatus | FK_OutboxMessage_IdOutboxMessageStatus
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessage> OutboxMessages => _outboxMessages;

	private OutboxMessageStatus()
	{
		_outboxMessageArchives = [];
		_outboxMessageProcessingLogs = [];
		_outboxMessages = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxMessageStatus), IdOutboxMessageStatus },
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
		return IdOutboxMessageStatus.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<OutboxMessageStatus> SetDBValidatorRules(ValidatorBuilder<OutboxMessageStatus> builder)
		=> builder
			.ForProperty(x => x.IdOutboxMessageStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
