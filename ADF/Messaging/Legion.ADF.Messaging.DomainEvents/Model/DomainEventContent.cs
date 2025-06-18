using Legion.Validation;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventContent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<DomainEventContent> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdDomainEventContent { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NOT NULL
	/// </summary>
	public string Content { get; private set; }


	/// <summary>
	/// 1:_1 DomainEvent.IdContent | FK_DomainEvent_IdDomainEventContent
	/// </summary>
	public DomainEvents.Model.DomainEvent DomainEvent { get; private set; }

	private DomainEventContent()
	{
	}

	static DomainEventContent()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<DomainEventContent>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdDomainEventContent), IdDomainEventContent },
			{ nameof(Content), Content },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdDomainEventContent.ToString();
	}

	public override string? ToString()
	{
		return IdDomainEventContent.ToString();
	}

	public static ValidatorBuilder<DomainEventContent> SetDBValidatorRules(ValidatorBuilder<DomainEventContent> builder)
		=> builder
			.ForProperty(x => x.IdDomainEventContent, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Content, v => v.NotDefaultOrEmpty())
		;
}
