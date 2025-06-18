using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class MessageType : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	private List<MBox.Model.Message> _messages;
	private List<MBox.Model.Queue> _queues;

	public static IValidator<MessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
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
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string MimeType { get; private set; }


	/// <summary>
	/// N:_1 MBox.Model.Message.IdMessageType | FK_Message_IdMessageType
	/// </summary>
	public IReadOnlyList<MBox.Model.Message> Messages => _messages;

	/// <summary>
	/// N:_1 MBox.Model.Queue.IdMessageType | FK_Queue_IdMessageType
	/// </summary>
	public IReadOnlyList<MBox.Model.Queue> Queues => _queues;

	private MessageType()
	{
		_messages = [];
		_queues = [];
	}

	static MessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessageType>()).Build();
	}

	public override string? ToString()
	{
		return IdMessageType.ToString();
	}

	public static ValidatorBuilder<MessageType> SetDBValidatorRules(ValidatorBuilder<MessageType> builder)
		=> builder
			.ForProperty(x => x.IdMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.MimeType, v => v.NotDefaultOrEmpty().MaxLength(1023))
		;
}
