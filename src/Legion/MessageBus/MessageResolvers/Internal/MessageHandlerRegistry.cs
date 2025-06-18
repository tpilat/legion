using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Concurrent;

namespace Legion.MessageBus.MessageResolvers.Internal;

internal class MessageHandlerRegistry : IMessageRegistry
{
	private static readonly Type _iRequestMessage = typeof(IRequestMessage<>);
	private static readonly Type _iVoidRequestMessage = typeof(IRequestMessage);
	private static readonly Type _iCommand = typeof(ICommand<>);
	private static readonly Type _iVoidCommand = typeof(ICommand);
	private static readonly Type _iQuery = typeof(IQuery<>);

	private static readonly Type _iMessageHandlerTypeDefinition = typeof(IMessageHandler<,>);
	private static readonly Type _iAsyncMessageHandlerTypeDefinition = typeof(IAsyncMessageHandler<,>);
	private static readonly Type _iVoidMessageHandlerTypeDefinition = typeof(IMessageHandler<>);
	private static readonly Type _iAsyncVoidMessageHandlerTypeDefinition = typeof(IAsyncMessageHandler<>);

	private static readonly Type _iMessageHandlerInterceptorTypeDefinition = typeof(IMessageHandlerInterceptor<,>);
	private static readonly Type _iAsyncMessageHandlerInterceptorTypeDefinition = typeof(IAsyncMessageHandlerInterceptor<,>);
	private static readonly Type _iVoidMessageHandlerInterceptorTypeDefinition = typeof(IMessageHandlerInterceptor<>);
	private static readonly Type _iAsyncVoidMessageHandlerInterceptorTypeDefinition = typeof(IAsyncMessageHandlerInterceptor<>);

	private static readonly Type _messageHandlerInterceptorTypeDefinition = typeof(MessageHandlerInterceptor<,>);
	private static readonly Type _asyncMessageHandlerInterceptorTypeDefinition = typeof(AsyncMessageHandlerInterceptor<,>);
	private static readonly Type _voidMessageHandlerInterceptorTypeDefinition = typeof(VoidMessageHandlerInterceptor<>);
	private static readonly Type _asyncVoidMessageHandlerInterceptorTypeDefinition = typeof(AsyncVoidMessageHandlerInterceptor<>);

	private static readonly ConcurrentDictionary<Type, Type> _messageHandlersRegistry = []; //ConcurrentDictionary<messageType, handlerType>
	private static readonly ConcurrentDictionary<Type, Type> _asyncMessageHandlersRegistry = []; //ConcurrentDictionary<messageType, handlerType>
	private static readonly ConcurrentDictionary<Type, Type> _voidMessageHandlersRegistry = []; //ConcurrentDictionary<messageType, handlerType>
	private static readonly ConcurrentDictionary<Type, Type> _asyncVoidMessageHandlersRegistry = []; //ConcurrentDictionary<messageType, handlerType>

	private static readonly ConcurrentDictionary<Type, MessageType> _types = []; //ConcurrentDictionary<crl_message_type, MessageType>
	private static readonly ConcurrentDictionary<IMessageType, Type> _messageTypes = []; //ConcurrentDictionary<MessageType, crl_message_type>

	private readonly IServiceCollection _services;
	private readonly IMessageTypeResolver _typeResolver;
	private readonly ServiceLifetime _handlerLifetime;
	private readonly ServiceLifetime _interceptorLifetime;

	public MessageHandlerRegistry(
		IServiceCollection services,
		IMessageTypeResolver typeResolver,
		ServiceLifetime handlerLifetime = ServiceLifetime.Transient,
		ServiceLifetime interceptorLifetime = ServiceLifetime.Transient)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNull(typeResolver);

		_services = services;
		_typeResolver = typeResolver;
		_handlerLifetime = handlerLifetime;
		_interceptorLifetime = interceptorLifetime;
	}

	public static List<IMessageType> GetAllMessageTypes()
		=> _types.Values.Cast<IMessageType>().ToList();

	public static IMessageType? GetMessageType(Type type)
	{
		_types.TryGetValue(type, out var messageType);
		return messageType;
	}

	public static bool CanBeHandled(Type type)
	{
		_types.TryGetValue(type, out var messageType);
		return messageType?.MessageMetaType.IsRequest() ?? false;
	}

	public static Type? GetType(IMessageType messageType)
	{
		_messageTypes.TryGetValue(messageType, out var type);
		return type;
	}

	public static Type? GetMessageHandlerType<TRequestMessage, TResponse>()
		where TRequestMessage : IRequestMessage<TResponse>
	{
		return GetMessageHandlerType(typeof(TRequestMessage));
	}

	public static Type? GetMessageHandlerType(Type messageType)
	{
		Throw.IfArgumentNull(messageType);

		_messageHandlersRegistry.TryGetValue(messageType, out Type? handlerType);
		return handlerType;
	}

	public static Type? GetAsyncMessageHandlerType<TRequestMessage, TResponse>()
		where TRequestMessage : IRequestMessage<TResponse>
	{
		return GetAsyncMessageHandlerType(typeof(TRequestMessage));
	}

	public static Type? GetAsyncMessageHandlerType(Type messageType)
	{
		Throw.IfArgumentNull(messageType);

		_asyncMessageHandlersRegistry.TryGetValue(messageType, out Type? handlerType);
		return handlerType;
	}

	public static Type? GetVoidMessageHandlerType<TRequestMessage>()
		where TRequestMessage : IRequestMessage
	{
		return GetVoidMessageHandlerType(typeof(TRequestMessage));
	}

	public static Type? GetVoidMessageHandlerType(Type messageType)
	{
		Throw.IfArgumentNull(messageType);

		_voidMessageHandlersRegistry.TryGetValue(messageType, out Type? handlerType);
		return handlerType;
	}

	public static Type? GetAsyncVoidMessageHandlerType<TRequestMessage>()
		where TRequestMessage : IRequestMessage
	{
		return GetAsyncVoidMessageHandlerType(typeof(TRequestMessage));
	}

	public static Type? GetAsyncVoidMessageHandlerType(Type messageType)
	{
		Throw.IfArgumentNull(messageType);

		_asyncVoidMessageHandlersRegistry.TryGetValue(messageType, out Type? handlerType);
		return handlerType;
	}

	public bool TryRegisterHandlerAndInterceptor(Type type)
	{
		Throw.IfArgumentNull(type);

		var interfaces = type.GetInterfaces();
		if (interfaces == null)
			return false;

		var registered = false;
		foreach (var ifc in interfaces)
		{
			if (ifc.IsGenericType)
			{
				var genericTypeDefinition = ifc.GetGenericTypeDefinition();
				if (ifc.GenericTypeArguments.Length == 1)
				{
					if (_iVoidMessageHandlerTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterVoidMessageHandler(ifc.GenericTypeArguments[0], type);
						registered = true;
					}
					else if (_iAsyncVoidMessageHandlerTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterAsyncVoidMessageHandler(ifc.GenericTypeArguments[0], type);
						registered = true;
					}
					else if (_iVoidMessageHandlerInterceptorTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterVoidMessageHandlerInterceptor(ifc.GenericTypeArguments[0], type);
					}
					else if (_iAsyncVoidMessageHandlerInterceptorTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterAsyncVoidMessageHandlerInterceptor(ifc.GenericTypeArguments[0], type);
					}
				}
				else if (ifc.GenericTypeArguments.Length == 2)
				{
					if (_iMessageHandlerTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterMessageHandler(ifc.GenericTypeArguments[0], ifc.GenericTypeArguments[1], type);
						registered = true;
					}
					else if (_iAsyncMessageHandlerTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterAsyncMessageHandler(ifc.GenericTypeArguments[0], ifc.GenericTypeArguments[1], type);
						registered = true;
					}
					else if (_iMessageHandlerInterceptorTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterMessageHandlerInterceptor(ifc.GenericTypeArguments[0], ifc.GenericTypeArguments[1], type);
					}
					else if (_iAsyncMessageHandlerInterceptorTypeDefinition.IsAssignableFrom(genericTypeDefinition))
					{
						RegisterAsyncMessageHandlerInterceptor(ifc.GenericTypeArguments[0], ifc.GenericTypeArguments[1], type);
					}
				}
			}
		}

		return registered;
	}

	public void RegisterMessageHandler(Type messageType, Type resposeType, Type handlerType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(resposeType);
		Throw.IfArgumentNull(handlerType);

		bool exists = false;
		if (_messageHandlersRegistry.TryGetValue(messageType, out var registeredHandlerType))
			exists = true;

		var iMessageType = _iRequestMessage.MakeGenericType(resposeType);
		if (!iMessageType.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For handler {handlerType.FullName} the message type {messageType.FullName} must implement {iMessageType.FullName}");

		var iMessageHandlerType = _iMessageHandlerTypeDefinition.MakeGenericType(messageType, resposeType);
		if (!iMessageHandlerType.IsAssignableFrom(handlerType))
			Throw.ConfigurationException($"Handler type {handlerType.FullName} must implement {iMessageHandlerType.FullName}");

		if (exists)
		{
			if (registeredHandlerType != handlerType)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered to {registeredHandlerType?.FullName ?? "--NULL--"} Cannot be registered to {handlerType.FullName}");

			_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
			return;
		}
		else
		{
			var added = _messageHandlersRegistry.TryAdd(messageType, handlerType);
			if (!added)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered. Cannot be registered to {handlerType.FullName}");
		}

		var messageMetaType = MessageMetaType.RequestMessage_WithResponse;

		var iCommand = _iCommand.MakeGenericType(resposeType);
		if (!iCommand.IsAssignableFrom(messageType))
			messageMetaType = MessageMetaType.Command_WithResponse;
		else
		{
			var iQuery = _iQuery.MakeGenericType(resposeType);
			if (!iQuery.IsAssignableFrom(messageType))
				messageMetaType = MessageMetaType.Query_WithResponse;
		}

		AddMessageType(messageType, resposeType, messageMetaType);

		_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
	}

	public void RegisterMessageHandlerInterceptor(Type messageType, Type resposeType, Type interceptorType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(resposeType);
		Throw.IfArgumentNull(interceptorType);

		if (!interceptorType.Inherits(_messageHandlerInterceptorTypeDefinition))
			Throw.ConfigurationException($"Interceptor {interceptorType.FullName} must implement {_messageHandlerInterceptorTypeDefinition.ToFriendlyFullName()}");

		var iMessageType = _iRequestMessage.MakeGenericType(resposeType);
		if (!iMessageType.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For interceptor {interceptorType.FullName} message type {messageType.FullName} must implement {iMessageType.FullName}");

		_services.TryAdd(new ServiceDescriptor(interceptorType, interceptorType, _interceptorLifetime));
	}

	public void RegisterAsyncMessageHandler(Type messageType, Type resposeType, Type handlerType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(resposeType);
		Throw.IfArgumentNull(handlerType);

		bool exists = false;
		if (_asyncMessageHandlersRegistry.TryGetValue(messageType, out var registeredHandlerType))
			exists = true;

		var iMessageType = _iRequestMessage.MakeGenericType(resposeType);
		if (!iMessageType.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For handler {handlerType.FullName} the message type {messageType.FullName} must implement {iMessageType.FullName}");

		var iMessageHandlerType = _iAsyncMessageHandlerTypeDefinition.MakeGenericType(messageType, resposeType);
		if (!iMessageHandlerType.IsAssignableFrom(handlerType))
			Throw.ConfigurationException($"Handler type {handlerType.FullName} must implement {iMessageHandlerType.FullName}");

		if (exists)
		{
			if (registeredHandlerType != handlerType)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered to {registeredHandlerType?.FullName ?? "--NULL--"} Cannot be registered to {handlerType.FullName}");

			_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
			return;
		}
		else
		{
			var added = _asyncMessageHandlersRegistry.TryAdd(messageType, handlerType);
			if (!added)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered. Cannot be registered to {handlerType.FullName}");
		}

		var messageMetaType = MessageMetaType.RequestMessage_WithResponse;

		var iCommand = _iCommand.MakeGenericType(resposeType);
		if (!iCommand.IsAssignableFrom(messageType))
			messageMetaType = MessageMetaType.Command_WithResponse;
		else
		{
			var iQuery = _iQuery.MakeGenericType(resposeType);
			if (!iQuery.IsAssignableFrom(messageType))
				messageMetaType = MessageMetaType.Query_WithResponse;
		}

		AddMessageType(messageType, resposeType, messageMetaType);

		_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
	}

	public void RegisterAsyncMessageHandlerInterceptor(Type messageType, Type resposeType, Type interceptorType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(resposeType);
		Throw.IfArgumentNull(interceptorType);

		if (!interceptorType.Inherits(_asyncMessageHandlerInterceptorTypeDefinition))
			Throw.ConfigurationException($"Interceptor {interceptorType.FullName} must implement {_asyncMessageHandlerInterceptorTypeDefinition.ToFriendlyFullName()}");

		var iMessageType = _iRequestMessage.MakeGenericType(resposeType);
		if (!iMessageType.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For interceptor {interceptorType.FullName} message type {messageType.FullName} must implement {iMessageType.FullName}");

		_services.TryAdd(new ServiceDescriptor(interceptorType, interceptorType, _interceptorLifetime));
	}

	public void RegisterVoidMessageHandler(Type messageType, Type handlerType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(handlerType);

		bool exists = false;
		if (_voidMessageHandlersRegistry.TryGetValue(messageType, out var registeredHandlerType))
			exists = true;

		if (!_iVoidRequestMessage.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For handler {handlerType.FullName} the message type {messageType.FullName} must implement {_iVoidRequestMessage.FullName}");

		var iMessageHandlerType = _iVoidMessageHandlerTypeDefinition.MakeGenericType(messageType);
		if (!iMessageHandlerType.IsAssignableFrom(handlerType))
			Throw.ConfigurationException($"Handler type {handlerType.FullName} must implement {iMessageHandlerType.FullName}");

		if (exists)
		{
			if (registeredHandlerType != handlerType)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered to {registeredHandlerType?.FullName ?? "--NULL--"} Cannot be registered to {handlerType.FullName}");

			_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
			return;
		}
		else
		{
			var added = _voidMessageHandlersRegistry.TryAdd(messageType, handlerType);
			if (!added)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered. Cannot be registered to {handlerType.FullName}");
		}

		var messageMetaType = MessageMetaType.RequestMessage_Void;

		if (!_iVoidCommand.IsAssignableFrom(messageType))
			messageMetaType = MessageMetaType.Command_Void;

		AddMessageType(messageType, null, messageMetaType);

		_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
	}

	public void RegisterVoidMessageHandlerInterceptor(Type messageType, Type interceptorType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(interceptorType);

		if (!interceptorType.Inherits(_voidMessageHandlerInterceptorTypeDefinition))
			Throw.ConfigurationException($"Interceptor {interceptorType.FullName} must implement {_voidMessageHandlerInterceptorTypeDefinition.ToFriendlyFullName()}");

		if (!_iVoidRequestMessage.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For interceptor {interceptorType.FullName} message type {messageType.FullName} must implement {_iVoidRequestMessage.FullName}");

		_services.TryAdd(new ServiceDescriptor(interceptorType, interceptorType, _interceptorLifetime));
	}

	public void RegisterAsyncVoidMessageHandler(Type messageType, Type handlerType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(handlerType);

		bool exists = false;
		if (_asyncVoidMessageHandlersRegistry.TryGetValue(messageType, out var registeredHandlerType))
			exists = true;

		if (!_iVoidRequestMessage.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For handler {handlerType.FullName} the message type {messageType.FullName} must implement {_iVoidRequestMessage.FullName}");

		var iMessageHandlerType = _iAsyncVoidMessageHandlerTypeDefinition.MakeGenericType(messageType);
		if (!iMessageHandlerType.IsAssignableFrom(handlerType))
			Throw.ConfigurationException($"Handler type {handlerType.FullName} must implement {iMessageHandlerType.FullName}");

		if (exists)
		{
			if (registeredHandlerType != handlerType)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered to {registeredHandlerType?.FullName ?? "--NULL--"} Cannot be registered to {handlerType.FullName}");

			_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
			return;
		}
		else
		{
			var added = _asyncVoidMessageHandlersRegistry.TryAdd(messageType, handlerType);
			if (!added)
				Throw.ConfigurationException($"Message type {messageType.FullName} is already registered. Cannot be registered to {handlerType.FullName}");
		}

		var messageMetaType = MessageMetaType.RequestMessage_Void;

		if (!_iVoidCommand.IsAssignableFrom(messageType))
			messageMetaType = MessageMetaType.Command_Void;

		AddMessageType(messageType, null, messageMetaType);

		_services.TryAdd(new ServiceDescriptor(iMessageHandlerType, handlerType, _handlerLifetime));
	}

	public void RegisterAsyncVoidMessageHandlerInterceptor(Type messageType, Type interceptorType)
	{
		Throw.IfArgumentNull(messageType);
		Throw.IfArgumentNull(interceptorType);

		if (!interceptorType.Inherits(_asyncVoidMessageHandlerInterceptorTypeDefinition))
			Throw.ConfigurationException($"Interceptor {interceptorType.FullName} must implement {_asyncVoidMessageHandlerInterceptorTypeDefinition.ToFriendlyFullName()}");

		if (!_iVoidRequestMessage.IsAssignableFrom(messageType))
			Throw.ConfigurationException($"For interceptor {interceptorType.FullName} message type {messageType.FullName} must implement {_iVoidRequestMessage.FullName}");

		_services.TryAdd(new ServiceDescriptor(interceptorType, interceptorType, _interceptorLifetime));
	}




	private void AddMessageType(Type type, Type? responseType, MessageMetaType messageMetaType)
	{
		var resolvedTypeString = _typeResolver.ToName(type);
		if (string.IsNullOrWhiteSpace(resolvedTypeString))
			Throw.ConfigurationException($"Message type {type?.FullName} {nameof(resolvedTypeString)} == NULL");

		var messageType =
			new MessageType
			(
				type.FullName ?? type.ToFriendlyFullName() ?? type.Name,
				resolvedTypeString,
				messageMetaType
			);

		IMessageType? previousResponseType = null;
		var added = _types.TryAdd(type, messageType);
		if (added)
		{
			_messageTypes.TryAdd(messageType, type);
		}
		else
		{
			previousResponseType = _types[type].ResponseMessageType;
		}

		if (responseType != null)
		{
			resolvedTypeString = _typeResolver.ToName(responseType);
			if (string.IsNullOrWhiteSpace(resolvedTypeString))
				Throw.ConfigurationException($"Response type's {responseType} FullName == NULL");

			var responseMessageMetaType = messageMetaType switch
			{
				MessageMetaType.RequestMessage_WithResponse => MessageMetaType.Response_ForRequestMessage,
				MessageMetaType.Command_WithResponse => MessageMetaType.Response_ForCommand,
				MessageMetaType.Query_WithResponse => MessageMetaType.Response_ForQuery,
				_ => (MessageMetaType?)null,
			};

			if (!responseMessageMetaType.HasValue)
				Throw.ConfigurationException($"Type {type.FullName} cannot have any response. {nameof(messageMetaType)} = {messageMetaType}");

			if (added)
			{
				if (previousResponseType != null)
				{
					if (!previousResponseType.CrlType.Equals(resolvedTypeString, StringComparison.OrdinalIgnoreCase))
						Throw.ConfigurationException($"Message type {type.FullName} previously registered with {nameof(responseType)} == {previousResponseType.CrlType} | Current {nameof(responseType)} == {resolvedTypeString}");

					if (previousResponseType.MessageMetaType != responseMessageMetaType)
						Throw.ConfigurationException($"Message type {type.FullName} previously registered with {nameof(previousResponseType.MessageMetaType)} == {previousResponseType.MessageMetaType} | Current {nameof(responseMessageMetaType)} == {responseMessageMetaType}");
				}
				else
				{
					messageType.ResponseMessageType =
						new MessageType
						(
							responseType.FullName ?? responseType.ToFriendlyFullName() ?? responseType.Name,
							resolvedTypeString,
							responseMessageMetaType.Value
						);

					//added = _types.TryAdd(responseType, messageType.ResponseMessageType);
					//if (added)
					//	_messageTypes.TryAdd(messageType.ResponseMessageType, responseType);
				}
				//else
				//{
				//	Throw.ConfigurationException($"Message type {type.FullName} previously registered with no {nameof(responseType)} | Current {nameof(responseType)} == {resolvedTypeString}");
				//}
			}
		}
	}
}
