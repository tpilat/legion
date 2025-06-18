using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class BlockedMessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<BlockedMessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdBlockedMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_BlockedMessageType_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_BlockedMessageType_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	private BlockedMessageType()
	{
	}

	static BlockedMessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<BlockedMessageType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdBlockedMessageType), IdBlockedMessageType },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdBlockedMessageType.ToString();
	}

	public override string? ToString()
	{
		return IdBlockedMessageType.ToString();
	}

	public static ValidatorBuilder<BlockedMessageType> SetDBValidatorRules(ValidatorBuilder<BlockedMessageType> builder)
		=> builder
			.ForProperty(x => x.IdBlockedMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
