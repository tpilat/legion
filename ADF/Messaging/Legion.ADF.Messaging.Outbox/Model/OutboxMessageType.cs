using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageType : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	private List<Outbox.Model.OutboxMessageArchive> _outboxMessageArchives;
	private List<Outbox.Model.OutboxMessage> _outboxMessages;
	private List<Outbox.Model.OutboxQueue> _outboxQueues;

	public static IValidator<OutboxMessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxInstance.OutboxInstance | FK_OutboxMessageType_IdOutboxInstance
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdOutboxInstance | FK_OutboxMessageType_IdOutboxInstance
	/// </summary>
	public Outbox.Model.OutboxInstance OutboxInstance { get; private set; }


	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageArchive.IdMessageType | FK_OutboxMessageArchive_IdMessageType
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageArchive> OutboxMessageArchives => _outboxMessageArchives;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessage.IdMessageType | FK_OutboxMessage_IdMessageType
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessage> OutboxMessages => _outboxMessages;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxQueue.IdMessageType | FK_OutboxQueue_IdMessageType
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxQueue> OutboxQueues => _outboxQueues;

	private OutboxMessageType()
	{
		_outboxMessageArchives = [];
		_outboxMessages = [];
		_outboxQueues = [];
	}

	static OutboxMessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OutboxMessageType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxMessageType), IdOutboxMessageType },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdOutboxInstance), IdOutboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOutboxMessageType.ToString();
	}

	public override string? ToString()
	{
		return IdOutboxMessageType.ToString();
	}

	public static ValidatorBuilder<OutboxMessageType> SetDBValidatorRules(ValidatorBuilder<OutboxMessageType> builder)
		=> builder
			.ForProperty(x => x.IdOutboxMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOutboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxInstance == null)
		;
}
