using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageArchive> _messageArchives;
	private List<MessageBox.Model.Message> _messages;
	private List<MessageBox.Model.Queue> _queues;

	public static IValidator<MessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageType { get; private set; }

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
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_MessageType_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_MessageType_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.MessageArchive.IdMessageType | FK_MessageArchive_IdMessageType
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageArchive> MessageArchives => _messageArchives;

	/// <summary>
	/// N:_1 MessageBox.Model.Message.IdMessageType | FK_Message_IdMessageType
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Message> Messages => _messages;

	/// <summary>
	/// N:_1 MessageBox.Model.Queue.IdMessageType | FK_Queue_IdMessageType
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Queue> Queues => _queues;

	private MessageType()
	{
		_messageArchives = [];
		_messages = [];
		_queues = [];
	}

	static MessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessageType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdMessageType.ToString();
	}

	public override string? ToString()
	{
		return IdMessageType.ToString();
	}

	public static ValidatorBuilder<MessageType> SetDBValidatorRules(ValidatorBuilder<MessageType> builder)
		=> builder
			.ForProperty(x => x.IdMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
