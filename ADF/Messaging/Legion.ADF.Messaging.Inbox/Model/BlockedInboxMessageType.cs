using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class BlockedInboxMessageType : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<BlockedInboxMessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdBlockedInboxMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxInstance.InboxInstance | FK_BlockedInboxMessageType_InboxInstance
	/// </summary>
	public Guid IdInboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdInboxInstance | FK_BlockedInboxMessageType_InboxInstance
	/// </summary>
	public Inbox.Model.InboxInstance InboxInstance { get; private set; }

	private BlockedInboxMessageType()
	{
	}

	static BlockedInboxMessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<BlockedInboxMessageType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdBlockedInboxMessageType), IdBlockedInboxMessageType },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdInboxInstance), IdInboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdBlockedInboxMessageType.ToString();
	}

	public override string? ToString()
	{
		return IdBlockedInboxMessageType.ToString();
	}

	public static ValidatorBuilder<BlockedInboxMessageType> SetDBValidatorRules(ValidatorBuilder<BlockedInboxMessageType> builder)
		=> builder
			.ForProperty(x => x.IdBlockedInboxMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdInboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxInstance == null)
		;
}
