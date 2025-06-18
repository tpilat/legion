using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class QueueProcessingMode : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.Queue> _queues;
	private List<MessageBox.Model.Queue> _suspendingModeQueues;
	private List<MessageBox.Model.Topic> _suspendingModeTopics;
	private List<MessageBox.Model.TopicSubscription> _suspendingModeTopicSubscriptions;
	private List<MessageBox.Model.Topic> _topics;
	private List<MessageBox.Model.TopicSubscription> _topicSubscriptions;

	public static IValidator<QueueProcessingMode> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueueProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.Queue.IdProcessingMode | FK_Queue_IdProcessingMode
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Queue> Queues => _queues;

	/// <summary>
	/// N:_1 MessageBox.Model.Queue.IdSuspendingMode | FK_Queue_IdSuspendingMode
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Queue> SuspendingModeQueues => _suspendingModeQueues;

	/// <summary>
	/// N:_1 MessageBox.Model.Topic.IdSuspendingMode | FK_Topic_IdSuspendingMode
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Topic> SuspendingModeTopics => _suspendingModeTopics;

	/// <summary>
	/// N:_1 MessageBox.Model.TopicSubscription.IdSuspendingMode | FK_TopicSubscription_IdSuspendingMode
	/// </summary>
	public IReadOnlyList<MessageBox.Model.TopicSubscription> SuspendingModeTopicSubscriptions => _suspendingModeTopicSubscriptions;

	/// <summary>
	/// N:_1 MessageBox.Model.Topic.IdProcessingMode | FK_Topic_IdProcessingMode
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Topic> Topics => _topics;

	/// <summary>
	/// N:_1 MessageBox.Model.TopicSubscription.IdProcessingMode | FK_TopicSubscription_IdProcessingMode
	/// </summary>
	public IReadOnlyList<MessageBox.Model.TopicSubscription> TopicSubscriptions => _topicSubscriptions;

	private QueueProcessingMode()
	{
		_queues = [];
		_suspendingModeQueues = [];
		_suspendingModeTopics = [];
		_suspendingModeTopicSubscriptions = [];
		_topics = [];
		_topicSubscriptions = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdQueueProcessingMode), IdQueueProcessingMode },
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
		return IdQueueProcessingMode.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<QueueProcessingMode> SetDBValidatorRules(ValidatorBuilder<QueueProcessingMode> builder)
		=> builder
			.ForProperty(x => x.IdQueueProcessingMode, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
