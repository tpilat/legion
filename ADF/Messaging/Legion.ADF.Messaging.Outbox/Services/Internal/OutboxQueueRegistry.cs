using Legion.ADF.Messaging.Outbox.Events;
using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.Messaging.Outbox.Services.Internal;

internal class OutboxQueueRegistry
{
	private readonly ConcurrentDictionary<string, Func<Model.OutboxMessage, OutboxMessageReceivedEvent>> _outboxQueueReceivedEventFactories = [];
	private readonly ConcurrentDictionary<string, Model.OutboxQueue> _outboxQueues = [];

	private bool _isLocked;

	private readonly object _registerLock = new();
	internal void RegisterOutboxQueue<E>(
		IScopeContext scopeContext,
		string queueName,
		Func<Model.OutboxMessage, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		Guid? idMessageType)
		where E : OutboxMessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(factory);

		if (_isLocked)
			Throw.InvalidOperationException($"{nameof(OutboxQueueRegistry)} is locked", scopeContext);

		var receivedEventNamespace = typeof(E).GetSimplifiedAssemblyQualifiedName();

		var createdResult = Model.OutboxQueue.Create(
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
				Throw.InvalidOperationException($"{nameof(OutboxQueueRegistry)} is locked", scopeContext);

			if (_outboxQueueReceivedEventFactories.TryAdd(receivedEventNamespace, factory))
			{
				if (!_outboxQueues.TryAdd(queueName, createdResult.Data!))
					throw new InvalidOperationException($"{nameof(OutboxQueueRegistry)}: {nameof(queueName)} = {queueName} was already registered");
			}
			else
			{
				throw new InvalidOperationException($"{nameof(OutboxQueueRegistry)}: {nameof(receivedEventNamespace)} = {receivedEventNamespace} was already registered");
			}
		}
	}

	internal void Lock()
	{
		_isLocked = true;
	}

	//private bool _reseted = false;
	//internal void ResetOutboxQueues(IScopeContext scopeContext, List<Model.OutboxQueue> outboxQueues)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	Throw.IfArgumentNull(outboxQueues, scopeContext);

	//	if (_reseted)
	//		Throw.InvalidOperationException($"{nameof(OutboxQueueRegistry)} is already reseted", scopeContext);

	//	lock (_registerLock)
	//	{
	//		if (_reseted)
	//			Throw.InvalidOperationException($"{nameof(OutboxQueueRegistry)} is already reseted", scopeContext);

	//		_isLocked = true;

	//		_outboxQueues.Clear();
	//		foreach (var outboxQueue in outboxQueues)
	//		{
	//			var added = _outboxQueues.TryAdd(outboxQueue.Name, outboxQueue);
	//			if (!added)
	//				Throw.InvalidOperationException($"Duplicated {nameof(outboxQueue)} {nameof(outboxQueue.Name)} = {outboxQueue.Name}", scopeContext);
	//		}

	//		_reseted = true;
	//	}
	//}

	internal List<string> GetAllRegisterdReceivedEventNamespaces()
		=> _outboxQueueReceivedEventFactories.Keys.ToList();

	internal OutboxMessageReceivedEvent? CreateQueueEvent(string queueReceivedEventNamespace, Model.OutboxMessage outboxMessage)
	{
		Throw.IfArgumentNullOrWhiteSpace(queueReceivedEventNamespace);
		Throw.IfArgumentNull(outboxMessage);

		if (!_outboxQueueReceivedEventFactories.TryGetValue(queueReceivedEventNamespace, out var factory))
			return null;

		return factory(outboxMessage);
	}

	internal List<Model.OutboxQueue> GetAllOutboxQueueClones()
	{
		lock (_registerLock)
		{
			_isLocked = true;

			return _outboxQueues.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}
}
