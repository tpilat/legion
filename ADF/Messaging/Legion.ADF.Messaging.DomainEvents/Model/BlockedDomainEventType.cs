using Legion.Validation;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class BlockedDomainEventType : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<BlockedDomainEventType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdBlockedDomainEventType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	private BlockedDomainEventType()
	{
	}

	static BlockedDomainEventType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<BlockedDomainEventType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdBlockedDomainEventType), IdBlockedDomainEventType },
			{ nameof(Namespace), Namespace },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdBlockedDomainEventType.ToString();
	}

	public override string? ToString()
	{
		return IdBlockedDomainEventType.ToString();
	}

	public static ValidatorBuilder<BlockedDomainEventType> SetDBValidatorRules(ValidatorBuilder<BlockedDomainEventType> builder)
		=> builder
			.ForProperty(x => x.IdBlockedDomainEventType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
		;
}
