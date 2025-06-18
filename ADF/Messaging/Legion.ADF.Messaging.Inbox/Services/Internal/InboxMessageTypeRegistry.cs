using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.Messaging.Inbox.Services.Internal;

internal class InboxMessageTypeRegistry
{
	private readonly ConcurrentDictionary<string, Model.InboxMessageType> _inboxMessageTypes = [];

	private bool _isLocked;

	internal void RegisterInboxMessageType<T>(IScopeContext scopeContext)
	{
		var type = typeof(T);
		var name = type.Name;
		var @namespace = type.GetSimplifiedAssemblyQualifiedName();

		RegisterInboxMessageType(
			scopeContext,
			code: name,
			name,
			@namespace);
	}

	private readonly object _registerLock = new();
	internal void RegisterInboxMessageType(
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		if (_isLocked)
			Throw.InvalidOperationException($"{nameof(InboxMessageTypeRegistry)} is locked", scopeContext);

		var createdResult = Model.InboxMessageType.Create(
			scopeContext,
			code,
			name,
			@namespace);

		createdResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		lock (_registerLock)
		{
			if (_isLocked)
				Throw.InvalidOperationException($"{nameof(InboxMessageTypeRegistry)} is locked", scopeContext);

			if (!_inboxMessageTypes.TryAdd(@namespace, createdResult.Data!))
				throw new InvalidOperationException($"{nameof(InboxMessageTypeRegistry)}: {nameof(@namespace)} = {@namespace} was already registered");
		}
	}

	private bool _reseted = false;
	internal void ResetInboxMessageTypes(IScopeContext scopeContext, List<Model.InboxMessageType> inboxMessageTypes)
	{
		scopeContext = scopeContext.CreateNew();

		Throw.IfArgumentNull(inboxMessageTypes, scopeContext);

		if (_reseted)
			Throw.InvalidOperationException($"{nameof(InboxMessageTypeRegistry)} is already reseted", scopeContext);

		lock (_registerLock)
		{
			if (_reseted)
				Throw.InvalidOperationException($"{nameof(InboxMessageTypeRegistry)} is already reseted", scopeContext);

			_isLocked = true;

			_inboxMessageTypes.Clear();
			foreach (var inboxMessageType in inboxMessageTypes)
			{
				var added = _inboxMessageTypes.TryAdd(inboxMessageType.Namespace, inboxMessageType);
				if (!added)
					Throw.InvalidOperationException($"Duplicated {nameof(inboxMessageType)} {nameof(inboxMessageType.Namespace)} = {inboxMessageType.Namespace}", scopeContext);
			}

			_reseted = true;
		}
	}

	internal List<Model.InboxMessageType> GetAllInboxMessageTypesClones()
	{
		lock (_registerLock)
		{
			_isLocked = true;

			return _inboxMessageTypes.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}

	internal Guid? GetIdInboxMessageType(string inboxMessageTypeNamespace, bool @lock)
	{
		lock (_registerLock)
		{
			if (@lock)
				_isLocked = true;

			_inboxMessageTypes.TryGetValue(inboxMessageTypeNamespace, out var inboxMessageType);

			return inboxMessageType?.IdInboxMessageType;
		}
	}
}
