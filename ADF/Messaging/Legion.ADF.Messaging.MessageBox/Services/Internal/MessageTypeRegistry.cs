using Legion.Extensions;
using System.Collections.Concurrent;

namespace Legion.ADF.Messaging.MessageBox.Services.Internal;

internal class MessageTypeRegistry
{
	private readonly ConcurrentDictionary<string, Model.MessageType> _messageTypes = [];

	private bool _isLocked;

	internal void RegisterMessageType<T>(IScopeContext scopeContext)
	{
		var type = typeof(T);
		var name = type.Name;
		var @namespace = type.GetSimplifiedAssemblyQualifiedName();

		RegisterMessageType(
			scopeContext,
			code: name,
			name,
			@namespace);
	}

	private readonly object _registerLock = new();
	internal void RegisterMessageType(
		IScopeContext scopeContext,
		string code,
		string name,
		string @namespace)
	{
		if (_isLocked)
			Throw.InvalidOperationException($"{nameof(MessageTypeRegistry)} is locked", scopeContext);

		var createdResult = Model.MessageType.Create(
			scopeContext,
			code,
			name,
			@namespace);

		createdResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		lock (_registerLock)
		{
			if (_isLocked)
				Throw.InvalidOperationException($"{nameof(MessageTypeRegistry)} is locked", scopeContext);

			if (!_messageTypes.TryAdd(@namespace, createdResult.Data!))
				throw new InvalidOperationException($"{nameof(MessageTypeRegistry)}: {nameof(@namespace)} = {@namespace} was already registered");
		}
	}

	private bool _reseted = false;
	internal void ResetMessageTypes(IScopeContext scopeContext, List<Model.MessageType> messageTypes)
	{
		scopeContext = scopeContext.CreateNew();

		if (messageTypes == null)
			Throw.IfArgumentNull(messageTypes, scopeContext);

		if (_reseted)
			Throw.InvalidOperationException($"{nameof(MessageTypeRegistry)} is already reseted", scopeContext);

		lock (_registerLock)
		{
			if (_reseted)
				Throw.InvalidOperationException($"{nameof(MessageTypeRegistry)} is already reseted", scopeContext);

			_isLocked = true;

			_messageTypes.Clear();
			foreach (var messageType in messageTypes)
			{
				var added = _messageTypes.TryAdd(messageType.Namespace, messageType);
				if (!added)
					Throw.InvalidOperationException($"Duplicated {nameof(messageType)} {nameof(messageType.Namespace)} = {messageType.Namespace}", scopeContext);
			}

			_reseted = true;
		}
	}

	internal List<Model.MessageType> GetAllMessageTypesClones()
	{
		lock (_registerLock)
		{
			_isLocked = true;

			return _messageTypes.Values
				.Select(iq => iq.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull))
				.ToList()!;
		}
	}

	internal Guid? GetIdMessageType(string messageTypeNamespace, bool @lock)
	{
		lock (_registerLock)
		{
			if (@lock)
				_isLocked = true;

			_messageTypes.TryGetValue(messageTypeNamespace, out var messageType);

			return messageType?.IdMessageType;
		}
	}
}
