using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class BlockedOutboxMessageType : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<BlockedOutboxMessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdBlockedOutboxMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxInstance.OutboxInstance | FK_BlockedOutboxMessageType_OutboxInstance
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdOutboxInstance | FK_BlockedOutboxMessageType_OutboxInstance
	/// </summary>
	public Outbox.Model.OutboxInstance OutboxInstance { get; private set; }

	private BlockedOutboxMessageType()
	{
	}

	static BlockedOutboxMessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<BlockedOutboxMessageType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdBlockedOutboxMessageType), IdBlockedOutboxMessageType },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdOutboxInstance), IdOutboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdBlockedOutboxMessageType.ToString();
	}

	public override string? ToString()
	{
		return IdBlockedOutboxMessageType.ToString();
	}

	public static ValidatorBuilder<BlockedOutboxMessageType> SetDBValidatorRules(ValidatorBuilder<BlockedOutboxMessageType> builder)
		=> builder
			.ForProperty(x => x.IdBlockedOutboxMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOutboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxInstance == null)
		;
}
