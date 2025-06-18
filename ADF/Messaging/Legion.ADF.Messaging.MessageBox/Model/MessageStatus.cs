using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageStatus : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageArchive> _messageArchives;
	private List<MessageBox.Model.Message> _messages;

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
	/// N:_1 MessageBox.Model.MessageArchive.IdMessageStatus | FK_MessageArchive_IdMessageStatus
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageArchive> MessageArchives => _messageArchives;

	/// <summary>
	/// N:_1 MessageBox.Model.Message.IdMessageStatus | FK_Message_IdMessageStatus
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Message> Messages => _messages;

	private MessageStatus()
	{
		_messageArchives = [];
		_messages = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageStatus), IdMessageStatus },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdMessageStatus.ToString();
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
