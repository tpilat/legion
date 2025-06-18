using Legion.Extensions;
using Legion.MessageBus.Interceptors;
using Legion.MessageBus.MessageHandlers;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Legion.MessageBus.MessageResolvers.Internal;

internal class EventHandlerRegistry : IEventRegistry
{
	private static readonly Type _iEvent = typeof(IEvent);

	private static readonly Type _iEventHandlerTypeDefinition = typeof(IEventHandler<>);
	private static readonly Type _iAsyncEventHandlerTypeDefinition = typeof(IAsyncEventHandler<>);

	private static readonly Type _iEventHandlerInterceptorTypeDefinition = typeof(IEventHandlerInterceptor<>);
	private static readonly Type _iAsyncEventHandlerInterceptorTypeDefinition = typeof(IAsyncEventHandlerInterceptor<>);

	private static readonly Type _eventHandlerInterceptorTypeDefinition = typeof(EventHandlerInterceptor<>);
	private static readonly Type _asyncEventHandlerInterceptorTypeDefinition = typeof(AsyncEventHandlerInterceptor<>);

	private static readonly ConcurrentDictionary<Type, List<Type>> _eventHandlersRegistry = []; //ConcurrentDictionary<eventType, List<handlerType>>
	private static readonly ConcurrentDictionary<Type, List<Type>> _asyncEventHandlersRegistry = []; //ConcurrentDictionary<eventType, List<handlerType>>

	private static readonly ConcurrentDictionary<Type, MessageType> _types = []; //ConcurrentDictionary<crl_message_type, MessageType>
	private static readonly ConcurrentDictionary<IMessageType, Type> _messageTypes = []; //ConcurrentDictionary<MessageType, crl_message_type>

	private readonly IServiceCollection _services;
	private readonly IMessageTypeResolver _typeResolver;
	private readonly ServiceLifetime _handlerLifetime;
	private readonly ServiceLifetime _interceptorLifetime;

	public EventHandlerRegistry(
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

	public static List<IMessageType> GetAllEventTypes()
		=> _types.Values.Cast<IMessageType>().ToList();

	public static IMessageType? GetEventType(Type type)
	{
		_types.TryGetValue(type, out var eventType);
		return eventType;
	}

	public static bool CanBeHandled(Type type)
	{
		_types.TryGetValue(type, out var eventType);
		return eventType?.MessageMetaType.IsRequest() ?? false;
	}

	public static Type? GetType(IMessageType eventType)
	{
		_messageTypes.TryGetValue(eventType, out var type);
		return type;
	}

	public static List<Type>? GetEventHandlerType<TEvent>()
		where TEvent : IEvent
	{
		return GetEventHandlerType(typeof(TEvent));
	}

	public static List<Type>? GetEventHandlerType(Type eventType)
	{
		Throw.IfArgumentNull(eventType);

		_eventHandlersRegistry.TryGetValue(eventType, out List<Type>? handlerTypes);
		return handlerTypes;
	}

	public static List<Type>? GetAsyncEventHandlerType<TEvent>()
		where TEvent : IEvent
	{
		return GetAsyncEventHandlerType(typeof(TEvent));
	}

	public static List<Type>? GetAsyncEventHandlerType(Type eventType)
	{
		Throw.IfArgumentNull(eventType);

		_asyncEventHandlersRegistry.TryGetValue(eventType, out List<Type>? handlerTypes);
		return handlerTypes;
	}

	public bool TryRegisterHandlerAndInterceptor(Type type)
	{
		Throw.IfArgumentNull(type);

		var interfaces = type.GetInterfaces();
		if (interfaces == null)
			return false;

		foreach (var ifc in interfaces)
		{
			if (ifc.IsGenericType)
			{
				if (ifc.GenericTypeArguments.Length == 1)
				{
					if (_iEventHandlerTypeDefinition.IsAssignableFrom(ifc.GetGenericTypeDefinition()))
					{
						RegisterEventHandler(ifc.GenericTypeArguments[0], type);
						return true;
					}
					else if (_iAsyncEventHandlerTypeDefinition.IsAssignableFrom(ifc.GetGenericTypeDefinition()))
					{
						RegisterAsyncEventHandler(ifc.GenericTypeArguments[0], type);
						return true;
					}
					else if (_iEventHandlerInterceptorTypeDefinition.IsAssignableFrom(ifc.GetGenericTypeDefinition()))
					{
						RegisterEventHandlerInterceptor(ifc.GenericTypeArguments[0], type);
					}
					else if (_iAsyncEventHandlerInterceptorTypeDefinition.IsAssignableFrom(ifc.GetGenericTypeDefinition()))
					{
						RegisterAsyncEventHandlerInterceptor(ifc.GenericTypeArguments[0], type);
					}
				}
			}
		}

		return false;
	}

	public void RegisterEventHandler(Type eventType, Type handlerType)
	{
		Throw.IfArgumentNull(eventType);
		Throw.IfArgumentNull(handlerType);

		if (!_iEvent.IsAssignableFrom(eventType))
			Throw.ConfigurationException($"For handler {handlerType.FullName} the event type {eventType.FullName} must implement {_iEvent.FullName}");

		var iEventHandlerType = _iEventHandlerTypeDefinition.MakeGenericType(eventType);
		if (!iEventHandlerType.IsAssignableFrom(handlerType))
			Throw.ConfigurationException($"Handler type {handlerType.FullName} must implement {iEventHandlerType.FullName}");

		var added = _eventHandlersRegistry.AddOrUpdate(eventType, [handlerType], (key, existingTypes) =>
		{
			existingTypes.Add(handlerType);
			return existingTypes;
		});

		AddEventType(eventType, MessageMetaType.Event);

		_services.Add(new ServiceDescriptor(iEventHandlerType, handlerType, _handlerLifetime));
	}

	public void RegisterEventHandlerInterceptor(Type eventType, Type interceptorType)
	{
		Throw.IfArgumentNull(eventType);
		Throw.IfArgumentNull(interceptorType);

		if (!interceptorType.Inherits(_eventHandlerInterceptorTypeDefinition))
			Throw.ConfigurationException($"Interceptor {interceptorType.FullName} must implement {_eventHandlerInterceptorTypeDefinition.ToFriendlyFullName()}");

		if (!_iEvent.IsAssignableFrom(eventType))
			Throw.ConfigurationException($"For interceptor {interceptorType.FullName} event type {eventType.FullName} must implement {_iEvent.FullName}");

		_services.Add(new ServiceDescriptor(interceptorType, interceptorType, _interceptorLifetime));
	}

	public void RegisterAsyncEventHandler(Type eventType, Type handlerType)
	{
		Throw.IfArgumentNull(eventType);
		Throw.IfArgumentNull(handlerType);

		if (!_iEvent.IsAssignableFrom(eventType))
			Throw.ConfigurationException($"For handler {handlerType.FullName} the event type {eventType.FullName} must implement {_iEvent.FullName}");

		var iEventHandlerType = _iAsyncEventHandlerTypeDefinition.MakeGenericType(eventType);
		if (!iEventHandlerType.IsAssignableFrom(handlerType))
			Throw.ConfigurationException($"Handler type {handlerType.FullName} must implement {iEventHandlerType.FullName}");

		var added = _asyncEventHandlersRegistry.AddOrUpdate(eventType, [handlerType], (key, existingTypes) =>
		{
			existingTypes.Add(handlerType);
			return existingTypes;
		});

		AddEventType(eventType, MessageMetaType.Event);

		_services.Add(new ServiceDescriptor(iEventHandlerType, handlerType, _handlerLifetime));
	}

	public void RegisterAsyncEventHandlerInterceptor(Type eventType, Type interceptorType)
	{
		Throw.IfArgumentNull(eventType);
		Throw.IfArgumentNull(interceptorType);

		if (!interceptorType.Inherits(_asyncEventHandlerInterceptorTypeDefinition))
			Throw.ConfigurationException($"Interceptor {interceptorType.FullName} must implement {_asyncEventHandlerInterceptorTypeDefinition.ToFriendlyFullName()}");

		if (!_iEvent.IsAssignableFrom(eventType))
			Throw.ConfigurationException($"For interceptor {interceptorType.FullName} event type {eventType.FullName} must implement {_iEvent.FullName}");

		_services.Add(new ServiceDescriptor(interceptorType, interceptorType, _interceptorLifetime));
	}

	private void AddEventType(Type eventType, MessageMetaType messageMetaType)
	{
		var resolvedTypeString = _typeResolver.ToName(eventType);
		if (string.IsNullOrWhiteSpace(resolvedTypeString))
			Throw.ConfigurationException($"Message type {eventType?.FullName} {nameof(resolvedTypeString)} == NULL");

		var messageType =
			new MessageType
			(
				eventType.FullName ?? eventType.ToFriendlyFullName() ?? eventType.Name,
				resolvedTypeString,
				messageMetaType
			);

		var added = _types.TryAdd(eventType, messageType);
		if (added)
			_messageTypes.TryAdd(messageType, eventType);
	}
}
