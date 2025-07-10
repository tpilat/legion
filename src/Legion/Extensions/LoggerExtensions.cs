using Legion.Logging;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Legion;

public static partial class LoggerExtensions
{
	public static IDisposable? BeginScope(this ILogger logger, IScopeContext scopeContext)
	{
		Throw.IfArgumentNull(logger);

		if (scopeContext == null)
			return null;

		var scope = new Dictionary<string, string>();

		if (!string.IsNullOrWhiteSpace(scopeContext.BusinessProcess))
			scope.Add(nameof(scopeContext.BusinessProcess), scopeContext.BusinessProcess);

		if (!string.IsNullOrWhiteSpace(scopeContext.Component))
			scope.Add(nameof(scopeContext.Component), scopeContext.Component);

		if (scopeContext.TenantIdentifier.HasValue)
			scope.Add(nameof(scopeContext.TenantIdentifier), scopeContext.TenantIdentifier.Value.ToString());

		if (scopeContext.CorrelationId.HasValue)
			scope.Add(nameof(scopeContext.CorrelationId), scopeContext.CorrelationId.Value.ToString());

		if (scopeContext.IdUser.HasValue)
			scope.Add(nameof(scopeContext.IdUser), scopeContext.IdUser.Value.ToString());

		if (0 < scopeContext.ContextProperties?.Count)
			foreach (var property in scopeContext.ContextProperties)
				if (!string.IsNullOrWhiteSpace(property.Value))
					scope.Add(property.Key, property.Value);

		if (scope.Count == 0)
			return null;

		return logger.BeginScope(scope);
	}

	public static void LogMessage(this ILogger logger, ILogMessage message, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(message);

		if (!logger.IsEnabled(message.LogLevel))
			return;

		if (!skipIfAlreadyLogged || !message.IsLogged)
			message.Log(logger);
		//logger.LogTrace($"{LoggerSettings.LogMessage_Template}", message.ToDictionary());

		message.IsLogged = true;
	}

	public static void LogTraceMessage(this ILogger logger, ILogMessage message, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(message);

		if (!logger.IsEnabled(LogLevel.Trace))
			return;

		message.LogLevel = LogLevel.Trace;

		if (!skipIfAlreadyLogged || !message.IsLogged)
			message.Log(logger);
			//logger.LogTrace($"{LoggerSettings.LogMessage_Template}", message.ToDictionary());

		message.IsLogged = true;
	}

	public static ILogMessage? PrepareTraceMessage(this ILogger logger, IScopeContext scopeContext, Action<LogMessageBuilder>? messageBuilder, bool onlyIfEnabled = false)
	{
		Throw.IfArgumentNull(logger);

		if (onlyIfEnabled && !logger.IsEnabled(LogLevel.Trace))
			return null;

		Throw.IfArgumentNull(scopeContext);

		var builder = new LogMessageBuilder(scopeContext, null)
			.LogLevel(LogLevel.Trace);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	public static ILogMessage? LogTraceMessage(
		this ILogger logger,
		IServiceProvider serviceProvider,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogTraceMessage(
			logger,
			ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogTraceMessage(
		this ILogger logger,
		string sourceSystemName,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogTraceMessage(
			logger,
			ScopeContext.Create(sourceSystemName, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogTraceMessage(this ILogger logger, IScopeContext scopeContext, Action<LogMessageBuilder> messageBuilder, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(messageBuilder);

		var message = PrepareTraceMessage(logger, scopeContext, messageBuilder, true);
		if (message == null)
			return null;

		LogTraceMessage(logger, message, skipIfAlreadyLogged);

		return message;
	}

	public static void LogDebugMessage(this ILogger logger, ILogMessage message, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(message);

		if (!logger.IsEnabled(LogLevel.Debug))
			return;

		message.LogLevel = LogLevel.Debug;

		if (!skipIfAlreadyLogged || !message.IsLogged)
			message.Log(logger);
			//logger.LogDebug($"{LoggerSettings.LogMessage_Template}", message.ToDictionary());

		message.IsLogged = true;
	}

	public static ILogMessage? PrepareDebugMessage(this ILogger logger, IScopeContext scopeContext, Action<LogMessageBuilder> messageBuilder, bool onlyIfEnabled = false)
	{
		Throw.IfArgumentNull(logger);

		if (onlyIfEnabled && !logger.IsEnabled(LogLevel.Debug))
			return null;

		Throw.IfArgumentNull(scopeContext);

		var builder = new LogMessageBuilder(scopeContext, null)
			.LogLevel(LogLevel.Debug);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	public static ILogMessage? LogDebugMessage(
		this ILogger logger,
		IServiceProvider serviceProvider,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogDebugMessage(
			logger,
			ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogDebugMessage(
		this ILogger logger,
		string sourceSystemName,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogDebugMessage(
			logger,
			ScopeContext.Create(sourceSystemName, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogDebugMessage(this ILogger logger, IScopeContext scopeContext, Action<LogMessageBuilder> messageBuilder, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(messageBuilder);

		var message = PrepareDebugMessage(logger, scopeContext, messageBuilder, true);
		if (message == null)
			return null;

		LogDebugMessage(logger, message, skipIfAlreadyLogged);

		return message;
	}

	public static void LogInformationMessage(this ILogger logger, ILogMessage message, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(message);

		if (!logger.IsEnabled(LogLevel.Information))
			return;

		message.LogLevel = LogLevel.Information;

		if (!skipIfAlreadyLogged || !message.IsLogged)
			message.Log(logger);
			//logger.LogInformation($"{LoggerSettings.LogMessage_Template}", message.ToDictionary());

		message.IsLogged = true;
	}

	public static ILogMessage? PrepareInformationMessage(this ILogger logger, IScopeContext scopeContext, Action<LogMessageBuilder> messageBuilder, bool onlyIfEnabled = false)
	{
		Throw.IfArgumentNull(logger);

		if (onlyIfEnabled && !logger.IsEnabled(LogLevel.Information))
			return null;

		Throw.IfArgumentNull(scopeContext);

		var builder = new LogMessageBuilder(scopeContext, null)
			.LogLevel(LogLevel.Information);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	public static ILogMessage? LogInformationMessage(
		this ILogger logger,
		IServiceProvider serviceProvider,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogInformationMessage(
			logger,
			ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogInformationMessage(
		this ILogger logger,
		string sourceSystemName,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogInformationMessage(
			logger,
			ScopeContext.Create(sourceSystemName, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogInformationMessage(this ILogger logger, IScopeContext scopeContext, Action<LogMessageBuilder> messageBuilder, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(messageBuilder);

		var message = PrepareInformationMessage(logger, scopeContext, messageBuilder, true);
		if (message == null)
			return null;

		LogInformationMessage(logger, message, skipIfAlreadyLogged);

		return message;
	}

	public static void LogWarningMessage(this ILogger logger, ILogMessage message, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(message);

		if (!logger.IsEnabled(LogLevel.Warning))
			return;

		message.LogLevel = LogLevel.Warning;

		if (!skipIfAlreadyLogged || !message.IsLogged)
			message.Log(logger);
			//logger.LogWarning($"{LoggerSettings.LogMessage_Template}", message.ToDictionary());

		message.IsLogged = true;
	}

	public static ILogMessage? PrepareWarningMessage(this ILogger logger, IScopeContext scopeContext, IErrorCode? errorCode, Action<LogMessageBuilder> messageBuilder, bool onlyIfEnabled = false)
	{
		Throw.IfArgumentNull(logger);

		if (onlyIfEnabled && !logger.IsEnabled(LogLevel.Warning))
			return null;

		Throw.IfArgumentNull(scopeContext);

		var builder = new LogMessageBuilder(scopeContext, errorCode)
			.LogLevel(LogLevel.Warning);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	public static ILogMessage? LogWarningMessage(
		this ILogger logger,
		IServiceProvider serviceProvider,
		IErrorCode? errorCode,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogWarningMessage(
			logger,
			ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			errorCode,
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogWarningMessage(
		this ILogger logger,
		string sourceSystemName,
		IErrorCode? errorCode,
		Action<LogMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogWarningMessage(
			logger,
			ScopeContext.Create(sourceSystemName, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			errorCode,
			messageBuilder,
			skipIfAlreadyLogged);

	public static ILogMessage? LogWarningMessage(this ILogger logger, IScopeContext scopeContext, IErrorCode? errorCode, Action<LogMessageBuilder> messageBuilder, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(messageBuilder);

		var message = PrepareWarningMessage(logger, scopeContext, errorCode, messageBuilder, true);
		if (message == null)
			return null;

		LogWarningMessage(logger, message, skipIfAlreadyLogged);

		return message;
	}

	public static void LogErrorMessage(this ILogger logger, IErrorMessage message, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(message);

		message.LogLevel = LogLevel.Error;

		if (!skipIfAlreadyLogged || !message.IsLogged)
			message.Log(logger);
			//logger.LogError($"{LoggerSettings.LogMessage_Template}", message.ToDictionary());

		message.IsLogged = true;
	}

	public static IErrorMessage? PrepareErrorMessage(
		this ILogger logger,
		IScopeContext scopeContext,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder>? messageBuilder,
		bool onlyIfEnabled = false)
	{
		Throw.IfArgumentNull(logger);

		if (onlyIfEnabled && !logger.IsEnabled(LogLevel.Error))
			return null;

		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		var builder = new ErrorMessageBuilder(scopeContext, errorCode)
			.LogLevel(LogLevel.Error);

		if (messageBuilder == null)
		{
			builder
				.InternalMessage(errorCode.Message)
				.Detail(errorCode.Description);
		}
		else
		{
			messageBuilder?.Invoke(builder);
		}

		var message = builder.Build();

		return message;
	}

	public static IErrorMessage? LogErrorMessage(
		this ILogger logger,
		IServiceProvider serviceProvider,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder>? messageBuilder = null,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogErrorMessage(
			logger,
			ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			errorCode,
			messageBuilder,
			skipIfAlreadyLogged);

	public static IErrorMessage? LogErrorMessage(
		this ILogger logger,
		string sourceSystemName,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder>? messageBuilder = null,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogErrorMessage(
			logger,
			ScopeContext.Create(sourceSystemName, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			errorCode,
			messageBuilder,
			skipIfAlreadyLogged);

	public static IErrorMessage LogErrorMessage(
		this ILogger logger,
		IScopeContext scopeContext,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder>? messageBuilder = null,
		bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		var message = PrepareErrorMessage(logger, scopeContext, errorCode, messageBuilder, false);
		LogErrorMessage(logger, message!, skipIfAlreadyLogged);

		return message!;
	}

	public static void LogCriticalMessage(this ILogger logger, IErrorMessage message, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(message);

		message.LogLevel = LogLevel.Critical;

		if (!skipIfAlreadyLogged || !message.IsLogged)
			message.Log(logger);
			//logger.LogCritical($"{LoggerSettings.LogMessage_Template}", message.ToDictionary());

		message.IsLogged = true;
	}

	public static IErrorMessage? PrepareCriticalMessage(this ILogger logger, IScopeContext scopeContext, IErrorCode errorCode, Action<ErrorMessageBuilder> messageBuilder, bool onlyIfEnabled = false)
	{
		Throw.IfArgumentNull(logger);

		if (onlyIfEnabled && !logger.IsEnabled(LogLevel.Critical))
			return null;

		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		var builder = new ErrorMessageBuilder(scopeContext, errorCode)
			.LogLevel(LogLevel.Critical);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	public static IErrorMessage? LogCriticalMessage(
		this ILogger logger,
		IServiceProvider serviceProvider,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogCriticalMessage(
			logger,
			ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			errorCode,
			messageBuilder,
			skipIfAlreadyLogged);

	public static IErrorMessage? LogCriticalMessage(
		this ILogger logger,
		string sourceSystemName,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder,
		bool skipIfAlreadyLogged = true,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> LogCriticalMessage(
			logger,
			ScopeContext.Create(sourceSystemName, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber),
			errorCode,
			messageBuilder,
			skipIfAlreadyLogged);

	public static IErrorMessage LogCriticalMessage(this ILogger logger, IScopeContext scopeContext, IErrorCode errorCode, Action<ErrorMessageBuilder> messageBuilder, bool skipIfAlreadyLogged = true)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);
		Throw.IfArgumentNull(messageBuilder);

		var message = PrepareCriticalMessage(logger, scopeContext, errorCode, messageBuilder, false);
		LogCriticalMessage(logger, message!, skipIfAlreadyLogged);

		return message!;
	}

	//public static void LogEnvironmentInfo(this ILogger logger)
	//	=> LogEnvironmentInfo(logger, EnvironmentInfoProvider.GetEnvironmentInfo());

	//public static void LogEnvironmentInfo(this ILogger logger, EnvironmentInfo environmentInfo)
	//{
	//	logger.LogInformation($"{LoggerSettings.EnvironmentInfo_Template}", environmentInfo.ToDictionary());
	//}

	public static void LogResultErrorMessages(
		this ILogger logger,
		IResult result,
		bool skipIfAlreadyLogged,
		bool logWarnings)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(result);

		foreach (var errorMessage in result.ErrorMessages)
		{
			if (errorMessage.LogLevel == LogLevel.Error)
			{
				logger.LogErrorMessage(errorMessage, skipIfAlreadyLogged);
			}
			else if (errorMessage.LogLevel == LogLevel.Critical)
			{
				logger.LogCriticalMessage(errorMessage, skipIfAlreadyLogged);
			}
			else
				throw new NotSupportedException($"{nameof(errorMessage.LogLevel)} = {errorMessage.LogLevel}");
		}

		if (logWarnings)
		{
			foreach (var warningMessage in result.WarningMessages)
				logger.LogWarningMessage(warningMessage, skipIfAlreadyLogged);
		}
	}

	public static void LogResultErrorMessages(
		this ILogger logger,
		IScopeContext scopeContext,
		IErrorCode errorCode,
		IResult result,
		bool dataMustBeNotNull,
		bool skipIfAlreadyLogged,
		bool logWarnings)
	{
		Throw.IfArgumentNull(logger);
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);
		Throw.IfArgumentNull(result);

		var loggedError = false;

		foreach (var errorMessage in result.ErrorMessages)
		{
			if (errorMessage.LogLevel == LogLevel.Error)
			{
				logger.LogErrorMessage(errorMessage, skipIfAlreadyLogged);
				loggedError = true;
			}
			else if (errorMessage.LogLevel == LogLevel.Critical)
			{
				logger.LogCriticalMessage(errorMessage, skipIfAlreadyLogged);
				loggedError = true;
			}
			else
				throw new NotSupportedException($"{nameof(errorMessage.LogLevel)} = {errorMessage.LogLevel}");
		}

		if (logWarnings)
		{
			foreach (var warningMessage in result.WarningMessages)
				logger.LogWarningMessage(warningMessage, skipIfAlreadyLogged);
		}

		if (!loggedError && dataMustBeNotNull && result.HasErrorOrNullData)
			logger.LogErrorMessage(ErrorMessage.CreateErrorMessage(scopeContext, errorCode, x => x.InternalMessage("Result has no data")), skipIfAlreadyLogged);
	}

	//public static void LogResultAllMessages(
	//	this ILogger logger,
	//	IResult result,
	//	string? defaultClientErrorMessage,
	//	bool skipIfAlreadyLogged)
	//{
	//	Throw.ArgumentNull(logger);
	//	Throw.ArgumentNull(result);

	//	if (result.HasAnyMessage)
	//	{
	//		if (0 < result.ErrorMessages?.Count)
	//			foreach (var errorMessage in result.ErrorMessages)
	//				if (!string.IsNullOrWhiteSpace(defaultClientErrorMessage) && string.IsNullOrWhiteSpace(errorMessage.ClientMessage))
	//					errorMessage.ClientMessage = defaultClientErrorMessage;
	//	}

	//	var messages = new List<ILogMessage>(result.ErrorMessages);
	//	messages.AddRange(result.WarningMessages);
	//	messages.AddRange(result.SuccessMessages);

	//	messages = messages.OrderBy(x => x.CreatedUtc).ToList();
	//	foreach (var message in messages)
	//	{
	//		switch (message.LogLevel)
	//		{
	//			case LogLevel.Trace:
	//				logger.LogTraceMessage(message, skipIfAlreadyLogged);
	//				break;
	//			case LogLevel.Debug:
	//				logger.LogDebugMessage(message, skipIfAlreadyLogged);
	//				break;
	//			case LogLevel.Information:
	//				logger.LogInformationMessage(message, skipIfAlreadyLogged);
	//				break;
	//			case LogLevel.Warning:
	//				logger.LogWarningMessage(message, skipIfAlreadyLogged);
	//				break;
	//			case LogLevel.Error:
	//				logger.LogErrorMessage((message as IErrorMessage)!, skipIfAlreadyLogged);
	//				break;
	//			case LogLevel.Critical:
	//				logger.LogCriticalMessage((message as IErrorMessage)!, skipIfAlreadyLogged);
	//				break;
	//			default:
	//				throw new NotSupportedException($"{nameof(message.LogLevel)} = {message.LogLevel}");
	//		}
	//	}
	//}

	//public static void LogResultErrorMessages(
	//	this ILogger logger,
	//	IResult result,
	//	string? defaultClientErrorMessage,
	//	bool skipIfAlreadyLogged)
	//{
	//	Throw.ArgumentNull(logger);
	//	Throw.ArgumentNull(result);

	//	foreach (var errorMessage in result.ErrorMessages)
	//	{
	//		if (errorMessage.LogLevel == LogLevel.Error)
	//			logger.LogErrorMessage(errorMessage, skipIfAlreadyLogged);
	//		else if (errorMessage.LogLevel == LogLevel.Critical)
	//			logger.LogCriticalMessage(errorMessage, skipIfAlreadyLogged);
	//		else
	//			throw new NotSupportedException($"{nameof(errorMessage.LogLevel)} = {errorMessage.LogLevel}");
	//	}
	//}
}
