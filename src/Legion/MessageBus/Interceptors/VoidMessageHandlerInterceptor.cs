using Legion.Database;
using Legion.MessageBus.Messages;
using Microsoft.Extensions.Logging;

namespace Legion.MessageBus.Interceptors;

public abstract class VoidMessageHandlerInterceptor<TRequestMessage> : IMessageHandlerInterceptor<TRequestMessage>, IMessageHandlerInterceptor
	where TRequestMessage : IRequestMessage
{
	protected ILogger Logger { get; }

	public VoidMessageHandlerInterceptor(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public virtual IResult InterceptHandle(
		IInvocationContext invocationContext,
		TRequestMessage message,
		IConnectionProvider connectionProvider,
		Func<IInvocationContext, TRequestMessage, IConnectionProvider, IResult> next)
	{
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable L_SCOPE_SET // Type or member is obsolete
		invocationContext.InvocationSetLogger(Logger, false);
#pragma warning restore L_SCOPE_SET // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete

		using var loggerScope = invocationContext.CreateLoggerScope();

		return Result.Call(next, invocationContext, message!, connectionProvider);
	}
}
