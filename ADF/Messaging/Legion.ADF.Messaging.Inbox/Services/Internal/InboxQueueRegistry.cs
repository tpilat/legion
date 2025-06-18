using Legion.ADF.Messaging.Inbox.Events;
using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.Messaging.Inbox.Services.Internal;

internal class InboxQueueRegistry
{
	private readonly ConcurrentDictionary<string, Func<Model.InboxMessage, InboxMessageReceivedEvent>> _inboxQueueReceivedEventFactories = [];
	private readonly ConcurrentDictionary<string, Model.InboxQueue> _inboxQueues = [];

	private bool _isLocked;

	private readonly object _registerLock = new();
	internal void RegisterInboxQueue<E>(
		IScopeContext scopeContext,
		string queueName,
		Func<Model.InboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		Guid? idMessageType)
		where E : InboxMessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(factory);

		if (_isLocked)
			Throw.InvalidOperationException($"{nameof(InboxQueueRegistry)} is locked", scopeContext);

		var receivedEventNamespace = typeof(E).GetSimplifiedAssemblyQualifiedName();

		var createdResult = Model.InboxQueue.Create(
			scopeContext,
			queueName,
			receivedEventNamespace,
			idMessageType,
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
				Throw.InvalidOperationException($"{nameof(InboxQueueRegistry)} is locked", scopeContext);

			if (_inboxQueueReceivedEventFactories.TryAdd(receivedEventNamespace, factory))
			{
				if (!_inboxQueues.TryAdd(queueName, createdResult.Data!))
					throw new InvalidOperationException($"{nameof(InboxQueueRegistry)}: {nameof(queueName)} = {queueName} was already registered");
			}
			else
			{
				throw new InvalidOperationException($"{nameof(InboxQueueRegistry)}: {nameof(receivedEventNamespace)} = {receivedEventNamespace} was already registered");
			}
		}
	}

	internal void Lock()
	{
		_isLocked = true;
	}

	//private bool _reseted = false;
	//internal void ResetInboxQueues(IScopeContext scopeContext, List<Model.InboxQueue> inboxQueues)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	Throw.IfArgumentNull(inboxQueues, scopeContext);

	//	if (_reseted)
	//		Throw.InvalidOperationException($"{nameof(InboxQueueRegistry)} is already reseted", scopeContext);

	//	lock (_registerLock)
	//	{
	//		if (_reseted)
	//			Throw.InvalidOperationException($"{nameof(InboxQueueRegistry)} is already reseted", scopeContext);

	//		_isLocked = true;

	//		_inboxQueues.Clear();
	//		foreach (var inboxQueue in inboxQueues)
	//		{
	//			var added = _inboxQueues.TryAdd(inboxQueue.Name, inboxQueue);
	//			if (!added)
	//				Throw.InvalidOperationException($"Duplicated {nameof(inboxQueue)} {nameof(inboxQueue.Name)} = {inboxQueue.Name}", scopeContext);
	//		}

	//		_reseted = true;
	//	}
	//}

	internal List<string> GetAllRegisterdReceivedEventNamespaces()
		=> _inboxQueueReceivedEventFactories.Keys.ToList();

	internal InboxMessageReceivedEvent? CreateQueueEvent(string queueReceivedEventNamespace, Model.InboxMessage inboxMessage)
	{
		Throw.IfArgumentNullOrWhiteSpace(queueReceivedEventNamespace);
		Throw.IfArgumentNull(inboxMessage);

		if (!_inboxQueueReceivedEventFactories.TryGetValue(queueReceivedEventNamespace, out var factory))
			return null;

		return factory(inboxMessage);
	}

	internal List<Model.InboxQueue> GetAllInboxQueueClones()
	{
		lock (_registerLock)
		{
			_isLocked = true;

			return _inboxQueues.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}
}
