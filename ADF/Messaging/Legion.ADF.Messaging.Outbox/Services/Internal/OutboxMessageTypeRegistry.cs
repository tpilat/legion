using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.Messaging.Outbox.Services.Internal;

internal class OutboxMessageTypeRegistry
{
	private readonly ConcurrentDictionary<string, Model.OutboxMessageType> _outboxMessageTypes = [];

	private bool _isLocked;

	internal void RegisterOutboxMessageType<T>(IScopeContext scopeContext)
	{
		var type = typeof(T);
		var name = type.Name;
		var @namespace = type.GetSimplifiedAssemblyQualifiedName();

		RegisterOutboxMessageType(
			scopeContext,
			code: name,
			name,
			@namespace);
	}

	private readonly object _registerLock = new();
	internal void RegisterOutboxMessageType(
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		if (_isLocked)
			Throw.InvalidOperationException($"{nameof(OutboxMessageTypeRegistry)} is locked", scopeContext);

		var createdResult = Model.OutboxMessageType.Create(
			scopeContext,
			code,
			name,
			@namespace);

		createdResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		lock (_registerLock)
		{
			if (_isLocked)
				Throw.InvalidOperationException($"{nameof(OutboxMessageTypeRegistry)} is locked", scopeContext);

			if (!_outboxMessageTypes.TryAdd(@namespace, createdResult.Data!))
				throw new InvalidOperationException($"{nameof(OutboxMessageTypeRegistry)}: {nameof(@namespace)} = {@namespace} was already registered");
		}
	}

	private bool _reseted = false;
	internal void ResetOutboxMessageTypes(IScopeContext scopeContext, List<Model.OutboxMessageType> outboxMessageTypes)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(outboxMessageTypes, scopeContext);

		if (_reseted)
			Throw.InvalidOperationException($"{nameof(OutboxMessageTypeRegistry)} is already reseted", scopeContext);

		lock (_registerLock)
		{
			if (_reseted)
				Throw.InvalidOperationException($"{nameof(OutboxMessageTypeRegistry)} is already reseted", scopeContext);

			_isLocked = true;

			_outboxMessageTypes.Clear();
			foreach (var outboxMessageType in outboxMessageTypes)
			{
				var added = _outboxMessageTypes.TryAdd(outboxMessageType.Namespace, outboxMessageType);
				if (!added)
					Throw.InvalidOperationException($"Duplicated {nameof(outboxMessageType)} {nameof(outboxMessageType.Namespace)} = {outboxMessageType.Namespace}", scopeContext);
			}

			_reseted = true;
		}
	}

	internal List<Model.OutboxMessageType> GetAllOutboxMessageTypesClones()
	{
		lock (_registerLock)
		{
			_isLocked = true;

			return _outboxMessageTypes.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}

	internal Guid? GetIdOutboxMessageType(string outboxMessageTypeNamespace, bool @lock)
	{
		lock (_registerLock)
		{
			if (@lock)
				_isLocked = true;

			_outboxMessageTypes.TryGetValue(outboxMessageTypeNamespace, out var outboxMessageType);

			return outboxMessageType?.IdOutboxMessageType;
		}
	}
}
