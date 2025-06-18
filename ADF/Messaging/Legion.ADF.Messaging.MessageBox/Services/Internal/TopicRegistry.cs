using Legion.ADF.Messaging.MessageBox.Events;
using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.Messaging.MessageBox.Services.Internal;

internal class TopicRegistry
{
	private readonly ConcurrentDictionary<string, Model.Topic> _topics = [];
	private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Model.TopicSubscription>> _topicSubscription = [];
	private readonly ConcurrentDictionary<string, Func<Model.Message, MessageReceivedEvent>> _globalTopicReceivedEventFactories = [];
	private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Func<Model.Message, MessageReceivedEvent>>> _topicReceivedEventFactories = [];

	private bool _isLocked;

	private readonly object _registerLock = new();
	internal Model.Topic RegisterTopic(
		IScopeContext scopeContext,
		string topicName,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode)
	{
		scopeContext = scopeContext.CreateNew();

		if (_isLocked)
			Throw.InvalidOperationException($"{nameof(TopicRegistry)} is locked", scopeContext);

		var createdResult = Model.Topic.Create(
			scopeContext,
			topicName,
			isSequentialFIFO,
			messagesBatchCount,
			maxDegreeOfParallelism,
			timeoutForMessageProcessing,
			maxMessageProcessingRetryCount,
			properties,
			idProcessingMode,
			idSuspendingMode);

		createdResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		lock (_registerLock)
		{
			if (_isLocked)
				Throw.InvalidOperationException($"{nameof(TopicRegistry)} is locked", scopeContext);

			if (!_topics.TryAdd(topicName, createdResult.Data!))
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: {nameof(topicName)} = {topicName} was already registered");
			else
			{
				_topicSubscription.TryAdd(topicName, []);
				_topicReceivedEventFactories.TryAdd(topicName, []);
			}
		}

		return createdResult.Data!;
	}

	internal void RegisterSubscription<E>(
		IScopeContext scopeContext,
		Model.Topic topic,
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
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(topic);
		Throw.IfArgumentNull(factory);

		var receivedEventNamespace = typeof(E).GetSimplifiedAssemblyQualifiedName();

		var createdResult = Model.TopicSubscription.Create(
			scopeContext,
			topic,
			new DTOs.TopicSubscriptionDto
			{
				SubscriptionName = subscriptionName,
				ReceivedEventNamespace = receivedEventNamespace,
				IsSequentialFIFO = isSequentialFIFO,
				MessagesBatchCount = messagesBatchCount,
				MaxDegreeOfParallelism = maxDegreeOfParallelism,
				TimeoutForMessageProcessing = timeoutForMessageProcessing,
				MaxMessageProcessingRetryCount = maxMessageProcessingRetryCount,
				Properties = properties,
				IdProcessingMode = idProcessingMode,
				IdSuspendingMode = idSuspendingMode,
				IdJob = idJob,
				IdOrchestration = idOrchestration
			});

		createdResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		topic.AddSubscription(scopeContext, createdResult.Data!);

		lock (_registerLock)
		{
			if (_isLocked)
				Throw.InvalidOperationException($"{nameof(TopicRegistry)} is locked", scopeContext);

			if (!_topics.TryGetValue(topic.Name, out var storedTopic))
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: Missing topic with name = {topic.Name}");

			if (topic != storedTopic)
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: Invalid stored topic reference with name = {topic.Name}");

			if (!_topicSubscription.TryGetValue(topic.Name, out var storedTopicSubscriptions))
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: Missing topic subscriptions for topic name = {topic.Name}");

			if (!_topicReceivedEventFactories.TryGetValue(topic.Name, out var storedTopicReceivedEventFactories))
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: Missing topic receivedEventFactories for topic name = {topic.Name}");

			if (storedTopicReceivedEventFactories.TryAdd(receivedEventNamespace, factory))
			{
				if (!storedTopicSubscriptions.TryAdd(subscriptionName, createdResult.Data!))
					throw new InvalidOperationException($"{nameof(TopicRegistry)}: {nameof(Model.Topic)} = {topic.Name}: {nameof(subscriptionName)} = {subscriptionName} was already registered");
			}
			else
			{
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: {nameof(Model.Topic)} = {topic.Name}: {nameof(receivedEventNamespace)} = {receivedEventNamespace} was already registered");
			}

			if (!_globalTopicReceivedEventFactories.TryAdd(receivedEventNamespace, factory))
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: {nameof(receivedEventNamespace)} = {receivedEventNamespace} was registered with multiple topics");
		}
	}

	internal void Lock()
	{
		_isLocked = true;
	}

	//private bool _reseted = false;
	//internal void ResetTopics(IScopeContext scopeContext, List<Model.Topic> topics)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	if (topics == null)
	//		Throw.IfArgumentNull(topics, scopeContext);

	//	if (_reseted)
	//		Throw.InvalidOperationException($"{nameof(TopicRegistry)} is already reseted", scopeContext);

	//	lock (_registerLock)
	//	{
	//		if (_reseted)
	//			Throw.InvalidOperationException($"{nameof(TopicRegistry)} is already reseted", scopeContext);

	//		_isLocked = true;

	//		_topics.Clear();
	//		foreach (var topic in topics)
	//		{
	//			var added = _topics.TryAdd(topic.Name, topic);
	//			if (!added)
	//				Throw.InvalidOperationException($"Duplicated {nameof(topic)} {nameof(topic.Name)} = {topic.Name}", scopeContext);
	//		}

	//		_reseted = true;
	//	}
	//}

	//internal void ResetSubscriptions(IScopeContext scopeContext, string topicName, List<Model.TopicSubscription> topicSubscriptions)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	Throw.IfArgumentNullOrWhiteSpace(topicName, scopeContext);
	//	Throw.IfArgumentNull(topicSubscriptions, scopeContext);

	//	if (_reseted)
	//		Throw.InvalidOperationException($"{nameof(TopicRegistry)} is already reseted", scopeContext);

	//	if (!_topicSubscription.TryGetValue(topicName, out var storedTopicSubscriptions))
	//		throw new InvalidOperationException($"{nameof(TopicRegistry)}: Missing topic subscriptions for {nameof(topicName)} = {topicName}");

	//	lock (_registerLock)
	//	{
	//		if (_reseted)
	//			Throw.InvalidOperationException($"{nameof(TopicRegistry)} is already reseted", scopeContext);

	//		_isLocked = true;

	//		storedTopicSubscriptions.Clear();
	//		foreach (var topicSubscription in topicSubscriptions)
	//		{
	//			var added = storedTopicSubscriptions.TryAdd(topicSubscription.SubscriptionName, topicSubscription);
	//			if (!added)
	//				Throw.InvalidOperationException($"Duplicated {nameof(topicSubscription)} {nameof(topicSubscription.SubscriptionName)} = {topicSubscription.SubscriptionName}", scopeContext);
	//		}

	//		_reseted = true;
	//	}
	//}

	internal List<string> GetAllTopicNames()
		=> _topics.Keys.ToList();

	internal List<Model.TopicSubscription> GetAllTopicSubscriptions(string topicName)
	{
		Throw.IfArgumentNullOrWhiteSpace(topicName);

		if (!_topicSubscription.TryGetValue(topicName, out var storedTopicSubscriptions))
			throw new InvalidOperationException($"{nameof(TopicRegistry)}: Missing topic subscriptions for topic name = {topicName}");

		return storedTopicSubscriptions.Values.ToList();
	}

	internal List<string> GetAllRegisterdReceivedEventNamespaces(string topicName)
	{
		Throw.IfArgumentNullOrWhiteSpace(topicName);

		if (!_topicReceivedEventFactories.TryGetValue(topicName, out var storedTopicReceivedEventFactories))
			throw new InvalidOperationException($"{nameof(TopicRegistry)}: Missing topic receivedEventFactories for topic name = {topicName}");

		return storedTopicReceivedEventFactories.Keys.ToList();
	}

	internal MessageReceivedEvent? CreateTopicEvent(string topicName, string topicReceivedEventNamespace, Model.Message message)
	{
		Throw.IfArgumentNullOrWhiteSpace(topicReceivedEventNamespace);
		Throw.IfArgumentNull(message);

		if (!_topicReceivedEventFactories.TryGetValue(topicName, out var storedTopicReceivedEventFactories))
			return null;

		if (!storedTopicReceivedEventFactories.TryGetValue(topicReceivedEventNamespace, out var factory))
			return null;

		return factory(message);
	}

	internal List<Model.Topic> GetAllTopicsClones()
	{
		lock (_registerLock)
		{
			_isLocked = true;

			return _topics.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}

	internal List<Model.TopicSubscription> GetAllTopicSubscriptioinsClones(string topicName)
	{
		lock (_registerLock)
		{
			_isLocked = true;

			if (!_topicSubscription.TryGetValue(topicName, out var storedTopicSubscriptions))
				throw new InvalidOperationException($"{nameof(TopicRegistry)}: Missing topic subscriptions for topic name = {topicName}");

			return storedTopicSubscriptions.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}
}
