using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class MessageStatus : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	private List<MBox.Model.Message> _messages;

	public static IValidator<MessageStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 MBox.Model.Message.IdMessageStatus | FK_Message_IdMessageStatus
	/// </summary>
	public IReadOnlyList<MBox.Model.Message> Messages => _messages;

	private MessageStatus()
	{
		_messages = [];
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<MessageStatus> SetDBValidatorRules(ValidatorBuilder<MessageStatus> builder)
		=> builder
			.ForProperty(x => x.IdMessageStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
