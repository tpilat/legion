using Legion.ADF.Messaging.MessageBox.Events;
using Legion.ADF.Messaging.MessageBox.Services.Internal;

namespace Legion.ADF.Messaging.MessageBox;

public class ADFMessagingMessageBoxTopicSubscriptionBuilder
{
	private readonly IScopeContext _scopeContext;
	private readonly TopicRegistry _topicRegistry;
	private readonly Model.Topic _topic;

	internal ADFMessagingMessageBoxTopicSubscriptionBuilder(IScopeContext scopeContext, TopicRegistry topicRegistry, Model.Topic topic)
	{
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(topicRegistry);
		Throw.IfArgumentNull(topic);

		_scopeContext = scopeContext;
		_topicRegistry = topicRegistry;
		_topic = topic;
	}

	public void RegisterSubscription<E>(
		string subscriptionName,
		Func<Model.Message, E> factory,
		Guid? idJob,
		Guid? idOrchestration)
		where E : MessageReceivedEvent
	{
		_topicRegistry.RegisterSubscription(
			_scopeContext,
			_topic,
			subscriptionName,
			factory,
			_topic.IsSequentialFIFO,
			_topic.MessagesBatchCount,
			_topic.MaxDegreeOfParallelism,
			_topic.TimeoutForMessageProcessing,
			_topic.MaxMessageProcessingRetryCount,
			_topic.Properties,
			_topic.IdProcessingMode,
			_topic.IdSuspendingMode,
			idJob,
			idOrchestration);
	}

	public void RegisterSubscription<E>(
		string subscriptionName,
		Func<Model.Message, E> factory,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		TimeSpan timeoutForMessageProcessing,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		Guid? idJob,
		Guid? idOrchestration)
		where E : MessageReceivedEvent
	{
		_topicRegistry.RegisterSubscription(
			_scopeContext,
			_topic,
			subscriptionName,
			factory,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			timeoutForMessageProcessing,
			maxMessageProcessingRetryCount,
			properties,
			idProcessingMode,
			idSuspendingMode,
			idJob,
			idOrchestration);
	}
}
