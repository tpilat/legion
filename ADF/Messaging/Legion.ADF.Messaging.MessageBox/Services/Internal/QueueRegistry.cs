using Legion.ADF.Messaging.MessageBox.Events;
using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.Messaging.MessageBox.Services.Internal;

internal class QueueRegistry
{
	private readonly ConcurrentDictionary<string, Func<Model.Message, MessageReceivedEvent>> _queueReceivedEventFactories = [];
	private readonly ConcurrentDictionary<string, Model.Queue> _queues = [];

	private bool _isLocked;

	private readonly object _registerLock = new();
	internal void RegisterQueue<E>(
		IScopeContext scopeContext,
		string queueName,
		Func<Model.Message, E> factory,
		TimeSpan timeoutForMessageProcessing,
		bool isSequentialFIFO,
		int messagesBatchCount,
		int? maxDegreeOfParallelism,
		int maxMessageProcessingRetryCount,
		string? properties,
		Guid idProcessingMode,
		Guid idSuspendingMode,
		Guid? idMessageType,
		Guid? idJob,
		Guid? idOrchestration)
		where E : MessageReceivedEvent
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(factory);

		if (_isLocked)
			Throw.InvalidOperationException($"{nameof(QueueRegistry)} is locked", scopeContext);

		var receivedEventNamespace = typeof(E).GetSimplifiedAssemblyQualifiedName();

		var createdResult = Model.Queue.Create(
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
			idSuspendingMode,
			idJob,
			idOrchestration);

		createdResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		lock (_registerLock)
		{
			if (_isLocked)
				Throw.InvalidOperationException($"{nameof(QueueRegistry)} is locked", scopeContext);

			if (_queueReceivedEventFactories.TryAdd(receivedEventNamespace, factory))
			{
				if (!_queues.TryAdd(queueName, createdResult.Data!))
					throw new InvalidOperationException($"{nameof(QueueRegistry)}: {nameof(queueName)} = {queueName} was already registered");
			}
			else
			{
				throw new InvalidOperationException($"{nameof(QueueRegistry)}: {nameof(receivedEventNamespace)} = {receivedEventNamespace} was already registered");
			}
		}
	}

	internal void Lock()
	{
		_isLocked = true;
	}

	//private bool _reseted = false;
	//internal void ResetQueues(IScopeContext scopeContext, List<Model.Queue> queues)
	//{
	//	scopeContext = scopeContext.CreateNew();

	//	Throw.IfArgumentNull(queues, scopeContext);

	//	if (_reseted)
	//		Throw.InvalidOperationException($"{nameof(QueueRegistry)} is already reseted", scopeContext);

	//	lock (_registerLock)
	//	{
	//		if (_reseted)
	//			Throw.InvalidOperationException($"{nameof(QueueRegistry)} is already reseted", scopeContext);

	//		_isLocked = true;

	//		_queues.Clear();
	//		foreach (var queue in queues)
	//		{
	//			var added = _queues.TryAdd(queue.Name, queue);
	//			if (!added)
	//				Throw.InvalidOperationException($"Duplicated {nameof(queue)} {nameof(queue.Name)} = {queue.Name}", scopeContext);
	//		}

	//		_reseted = true;
	//	}
	//}

	internal List<string> GetAllRegisterdReceivedEventNamespaces()
		=> _queueReceivedEventFactories.Keys.ToList();

	internal MessageReceivedEvent? CreateQueueEvent(string queueReceivedEventNamespace, Model.Message message)
	{
		Throw.IfArgumentNullOrWhiteSpace(queueReceivedEventNamespace);
		Throw.IfArgumentNull(message);

		if (!_queueReceivedEventFactories.TryGetValue(queueReceivedEventNamespace, out var factory))
			return null;

		return factory(message);
	}

	internal List<Model.Queue> GetAllQueueClones()
	{
		lock (_registerLock)
		{
			_isLocked = true;

			return _queues.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}
}
