using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class MessagePublishing : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<MessagePublishing> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessagePublishing { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_MessagePublishing_IdStepInstance
	/// </summary>
	public Guid? IdStepInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_MessagePublishing_IdJob
	/// </summary>
	public Guid? IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_MessagePublishing_IdAdapter
	/// </summary>
	public Guid? IdAdapter { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.Message.Message | FK_MessagePublishing_IdMessage
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessage | FK_MessagePublishing_IdMessage
	/// </summary>
	public MBox.Model.Message Message { get; private set; }

	private MessagePublishing()
	{
	}

	static MessagePublishing()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessagePublishing>()).Build();
	}

	public override string? ToString()
	{
		return IdMessagePublishing.ToString();
	}

	public static ValidatorBuilder<MessagePublishing> SetDBValidatorRules(ValidatorBuilder<MessagePublishing> builder)
		=> builder
			.ForProperty(x => x.IdMessagePublishing, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty(), x => x.Message == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
		;
}
