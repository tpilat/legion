using Legion.Exceptions;
using Legion.Text;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;

namespace Legion.Logging;

public class LogMessage : ILogMessage
{
	public const string LogMessageParamName = "@LogMessage";

	private string? _operationName;

	public Guid IdLogMessage { get; set; }
	public LogLevel LogLevel { get; set; }
	public int IdLogLevel => (int)LogLevel;

	public IErrorCode? ErrorCode { get; set; }
	public DateTimeOffset CreatedUtc { get; set; }
	public IScopeContext ScopeContext { get; set; }
	public string? OperationName
	{
		get => _operationName ?? ScopeContext.GetLastTraceFrame();
		set => _operationName = value;
	}
	public string? AggregateName { get; set; }
	public string? AggregateIdentifier { get; set; }
	public string? ClientMessage { get; set; }
	public string? InternalMessage { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public Exception? Exception { get; internal set; }
	public bool ShouldSerializeException() => false;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	Exception? ILogMessage.Exception
	{
		get => Exception;
		set => Exception = value;
	}

	public string? StackTrace { get; set; }
	public string? Detail { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public string ClientMessageWithId => ToString(true, false, false);
	public bool ShouldSerializeClientMessageWithId() => false;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public string ClientMessageWithIdAndPropName => ToString(true, true, false);
	public bool ShouldSerializeClientMessageWithIdAndPropName() => false;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public string FullMessage => ToString(true, true, true);
	public bool ShouldSerializeFullMessage() => false;

	public bool IsLogged { get; set; }
	public string? PropertyName { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public object? ValidationFailure { get; set; }
	public bool ShouldSerializeValidationFailure() => false;
	public string? DisplayPropertyName { get; set; }
	public bool IsValidationError { get; set; }

	public bool DisableTransactionRollback { get; set; }
	public string? SourceContext { get; set; }

	internal LogMessage(IScopeContext scopeContext, IErrorCode? errorCode)
	{
		Throw.IfArgumentNull(scopeContext);

		IdLogMessage = GlobalContext.Instance.NewGuid();
		CreatedUtc = DateTimeOffset.UtcNow;
		ScopeContext = scopeContext;
		ErrorCode = errorCode;
	}

	private static ILogMessage CreateLogMessage(
		IServiceProvider serviceProvider,
		LogLevel logLevel,
		Action<LogMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		Throw.IfArgumentNull(messageBuilder);

		var builder = new LogMessageBuilder(Legion.ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber), errorCode: null)
			.LogLevel(logLevel);

		messageBuilder.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	private static ILogMessage CreateLogMessage(
		IScopeContext scopeContext,
		LogLevel logLevel,
		Action<LogMessageBuilder> messageBuilder)
	{
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(messageBuilder);

		var builder = new LogMessageBuilder(scopeContext, errorCode: null)
			.LogLevel(logLevel);

		messageBuilder.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	private static ILogMessage CreateLogMessage(
		IServiceProvider serviceProvider,
		LogLevel logLevel,
		IErrorCode? errorCode,
		Action<LogMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		if (errorCode == null)
			Throw.IfArgumentNull(messageBuilder);

		var builder = new LogMessageBuilder(Legion.ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber), errorCode)
			.LogLevel(logLevel);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	private static ILogMessage CreateLogMessage(
		IScopeContext scopeContext,
		LogLevel logLevel,
		IErrorCode? errorCode,
		Action<LogMessageBuilder> messageBuilder)
	{
		Throw.IfArgumentNull(scopeContext);

		if (errorCode == null)
			Throw.IfArgumentNull(messageBuilder);

		var builder = new LogMessageBuilder(scopeContext, errorCode)
			.LogLevel(logLevel);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	private static IErrorMessage CreateErrorMessage(
		IServiceProvider serviceProvider,
		LogLevel logLevel,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		Throw.IfArgumentNull(errorCode);

		var builder = new ErrorMessageBuilder(Legion.ScopeContext.Create(serviceProvider, true, memberName: memberName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber), errorCode)
			.LogLevel(logLevel);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	private static IErrorMessage CreateErrorMessage(
		IScopeContext scopeContext,
		LogLevel logLevel,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder)
	{
		Throw.IfArgumentNull(scopeContext);
		Throw.IfArgumentNull(errorCode);

		var builder = new ErrorMessageBuilder(scopeContext, errorCode)
			.LogLevel(logLevel);

		messageBuilder?.Invoke(builder);
		var message = builder.Build();

		return message;
	}

	public static ILogMessage CreateTraceMessage(
		IServiceProvider serviceProvider,
		Action<LogMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateLogMessage(serviceProvider, LogLevel.Trace, messageBuilder, memberName, sourceFilePath, sourceLineNumber);

	public static ILogMessage CreateTraceMessage(
		IScopeContext scopeContext,
		Action<LogMessageBuilder> messageBuilder)
		=> CreateLogMessage(scopeContext, LogLevel.Trace, messageBuilder);

	public static ILogMessage CreateDebugMessage(
		IServiceProvider serviceProvider,
		Action<LogMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateLogMessage(serviceProvider, LogLevel.Debug, messageBuilder, memberName, sourceFilePath, sourceLineNumber);

	public static ILogMessage CreateDebugMessage(
		IScopeContext scopeContext,
		Action<LogMessageBuilder> messageBuilder)
		=> CreateLogMessage(scopeContext, LogLevel.Debug, messageBuilder);

	public static ILogMessage CreateInformationMessage(
		IServiceProvider serviceProvider,
		Action<LogMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateLogMessage(serviceProvider, LogLevel.Information, messageBuilder, memberName, sourceFilePath, sourceLineNumber);

	public static ILogMessage CreateInformationMessage(
		IScopeContext scopeContext,
		Action<LogMessageBuilder> messageBuilder)
		=> CreateLogMessage(scopeContext, LogLevel.Information, messageBuilder);

	public static ILogMessage CreateWarningMessage(
		IServiceProvider serviceProvider,
		IErrorCode? errorCode,
		Action<LogMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateLogMessage(serviceProvider, LogLevel.Warning, errorCode, messageBuilder, memberName, sourceFilePath, sourceLineNumber);

	public static ILogMessage CreateWarningMessage(
		IScopeContext scopeContext,
		IErrorCode? errorCode,
		Action<LogMessageBuilder> messageBuilder)
		=> CreateLogMessage(scopeContext, LogLevel.Warning, errorCode, messageBuilder);

	public static IErrorMessage CreateErrorMessage(
		IServiceProvider serviceProvider,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateErrorMessage(serviceProvider, LogLevel.Error, errorCode, messageBuilder, memberName, sourceFilePath, sourceLineNumber);

	public static IErrorMessage CreateErrorMessage(
		IScopeContext scopeContext,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder)
		=> CreateErrorMessage(scopeContext, LogLevel.Error, errorCode, messageBuilder);

	public static IErrorMessage CreateCriticalMessage(
		IServiceProvider serviceProvider,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
		=> CreateErrorMessage(serviceProvider, LogLevel.Critical, errorCode, messageBuilder, memberName, sourceFilePath, sourceLineNumber);

	public static IErrorMessage CreateCriticalMessage(
		IScopeContext scopeContext,
		IErrorCode errorCode,
		Action<ErrorMessageBuilder> messageBuilder)
		=> CreateErrorMessage(scopeContext, LogLevel.Critical, errorCode, messageBuilder);

	public IDictionary<string, object?> ToDictionary(Serializer.ISerializer? serializer = null)
	{
		var dict = new Dictionary<string, object?>
		{
			{ nameof(IdLogMessage), IdLogMessage },
			//{ nameof(LogLevel), LogLevel },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ScopeContext.RuntimeUniqueKey), ScopeContext.RuntimeUniqueKey },
			{ nameof(IsValidationError), IsValidationError }
		};

		if (ErrorCode != null)
		{
			dict.Add($"{nameof(ErrorCode)}.{nameof(ErrorCode.Code)}", ErrorCode.Code);
			dict.Add($"{nameof(ErrorCode)}.{nameof(ErrorCode.Message)}", ErrorCode.Message);
			dict.Add($"{nameof(ErrorCode)}.{nameof(ErrorCode.Description)}", ErrorCode.Description);
		}

		if (ScopeContext.IdApplicationEntry.HasValue)
			dict.Add(nameof(ScopeContext.IdApplicationEntry), ScopeContext.IdApplicationEntry.Value);

		if (!string.IsNullOrWhiteSpace(OperationName))
			dict.Add(nameof(OperationName), OperationName);

		if (!string.IsNullOrWhiteSpace(AggregateName))
			dict.Add(nameof(AggregateName), AggregateName);

		if (!string.IsNullOrWhiteSpace(AggregateIdentifier))
			dict.Add(nameof(AggregateIdentifier), AggregateIdentifier);

		if (!string.IsNullOrWhiteSpace(ClientMessage))
			dict.Add(nameof(ClientMessage), ClientMessage);

		if (!string.IsNullOrWhiteSpace(InternalMessage))
			dict.Add(nameof(InternalMessage), InternalMessage);

		if (!string.IsNullOrWhiteSpace(ScopeContext.SourceSystemName))
			dict.Add(nameof(ScopeContext.SourceSystemName), ScopeContext.SourceSystemName);

		if (ScopeContext.TraceFrameStack != null)
			dict.Add(nameof(ScopeContext.TraceFrameStack), $"{ScopeContext.TraceFrameStack}");

		if (!string.IsNullOrWhiteSpace(StackTrace))
			dict.Add(nameof(StackTrace), StackTrace);

		if (!string.IsNullOrWhiteSpace(Detail))
			dict.Add(nameof(Detail), Detail);

		if (ScopeContext.IdUser.HasValue)
			dict.Add(nameof(ScopeContext.IdUser), ScopeContext.IdUser);

		if (!string.IsNullOrWhiteSpace(PropertyName))
			dict.Add(nameof(PropertyName), PropertyName);

		if (!string.IsNullOrWhiteSpace(DisplayPropertyName))
			dict.Add(nameof(DisplayPropertyName), DisplayPropertyName);

		if (ValidationFailure != null)
			dict.Add(nameof(ValidationFailure), ValidationFailure.ToString());

		if (ScopeContext.CorrelationId.HasValue)
			dict.Add(nameof(ScopeContext.CorrelationId), ScopeContext.CorrelationId.Value);

		if (ScopeContext.ContextProperties != null && 0 < ScopeContext.ContextProperties.Count)
			dict.Add(nameof(ScopeContext.ContextProperties), Newtonsoft.Json.JsonConvert.SerializeObject(ScopeContext.ContextProperties));

		if (!string.IsNullOrWhiteSpace(SourceContext))
			dict.Add(nameof(SourceContext), SourceContext);

		return dict;
	}

	public Exception ToException()
		=> ExceptionHelper.ToException(this, false);

	public string ToMessage()
	{
		var sb = new StringBuilder();

		bool empty = string.IsNullOrWhiteSpace(ErrorCode?.Code);
		if (!empty)
			sb.Append($"{ErrorCode!.Code}:");

		bool hasOnlyErrorCode = true;
		var errorCodeMessageWasSet = false;

		if (!string.IsNullOrWhiteSpace(InternalMessage))
		{
			if (empty)
				sb.Append(InternalMessage);
			else
			{
				if (hasOnlyErrorCode)
					sb.Append($" {InternalMessage}");
				else
					sb.Append($" | {InternalMessage}");
			}

			empty = false;
			hasOnlyErrorCode = false;
		}
		else if (!string.IsNullOrWhiteSpace(ErrorCode?.Message) && Detail != ErrorCode!.Message)
		{
			if (empty)
				sb.Append(ErrorCode.Message);
			else
			{
				if (hasOnlyErrorCode)
					sb.Append($" {ErrorCode.Message}");
				else
					sb.Append($" | {ErrorCode.Message}");
			}

			empty = false;
			hasOnlyErrorCode = false;
			errorCodeMessageWasSet = true;
		}

		if (!string.IsNullOrWhiteSpace(Detail))
		{
			if (empty)
				sb.Append(Detail);
			else
			{
				if (hasOnlyErrorCode)
					sb.Append($" {Detail}");
				else
					sb.Append($" | {Detail}");
			}

			empty = false;
			hasOnlyErrorCode = false;
		}
		else if (!errorCodeMessageWasSet
			&& !string.IsNullOrWhiteSpace(ErrorCode?.Message)
			&& InternalMessage != ErrorCode!.Message)
		{
			if (empty)
				sb.Append(ErrorCode.Message);
			else
			{
				if (hasOnlyErrorCode)
					sb.Append($" {ErrorCode.Message}");
				else
					sb.Append($" | {ErrorCode.Message}");
			}

			empty = false;
			hasOnlyErrorCode = false;
		}

		if (empty)
			sb.Append(ClientMessage);
		else if (hasOnlyErrorCode)
			sb.Append($" {ClientMessage}");

		return sb.ToString();
	}

	public override string? ToString()
	{
		//return FullMessage;
		return IdLogMessage.ToString();
	}

	public string ToString(bool withId, bool withPropertyName, bool withDetail)
	{
		var sb = new StringBuilder();

		bool empty = string.IsNullOrWhiteSpace(ErrorCode?.Code);
		if (!empty)
		{
			sb.Append($"{ErrorCode!.Code}:");

			if (!string.IsNullOrWhiteSpace(ErrorCode?.Message))
				sb.Append($" [{ErrorCode!.Message}]:");
		}

		bool hasOnlyErrorCode = true;
		if (!string.IsNullOrWhiteSpace(ClientMessage))
		{
			if (empty)
				sb.Append(ClientMessage);
			else
				sb.Append($" {ClientMessage}");

			empty = false;
			hasOnlyErrorCode = false;
		}

		if (withPropertyName)
		{
			if (!string.IsNullOrWhiteSpace(DisplayPropertyName))
			{
				if (empty)
					sb.Append(DisplayPropertyName);
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {DisplayPropertyName}");
					else
						sb.Append($" - {DisplayPropertyName}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}
		}

		if (withId)
		{
			if (empty)
				sb.Append($"ID: {IdLogMessage}");
			else
			{
				if (hasOnlyErrorCode)
					sb.Append($" ID: {IdLogMessage}");
				else
					sb.Append($" (ID: {IdLogMessage})");
			}

			empty = false;
			hasOnlyErrorCode = false;
		}

		if (withDetail)
		{
			var errorCodeMessageWasSet = false;
			if (!string.IsNullOrWhiteSpace(InternalMessage))
			{
				if (empty)
					sb.Append(InternalMessage);
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {InternalMessage}");
					else
						sb.Append($" | {InternalMessage}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}
			else if (!string.IsNullOrWhiteSpace(ErrorCode?.Message) && Detail != ErrorCode!.Message)
			{
				if (empty)
					sb.Append(ErrorCode.Message);
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {ErrorCode.Message}");
					else
						sb.Append($" | {ErrorCode.Message}");
				}

				empty = false;
				hasOnlyErrorCode = false;
				errorCodeMessageWasSet = true;
			}

			if (!string.IsNullOrWhiteSpace(StackTrace))
			{
				if (empty)
					sb.Append(StackTrace);
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {StackTrace}");
					else
						sb.Append($" | {StackTrace}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}

			if (!string.IsNullOrWhiteSpace(Detail))
			{
				if (empty)
					sb.Append(Detail);
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {Detail}");
					else
						sb.Append($" | {Detail}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}
			else if (!errorCodeMessageWasSet
				&& !string.IsNullOrWhiteSpace(ErrorCode?.Message)
				&& InternalMessage != ErrorCode!.Message)
			{
				if (empty)
					sb.Append(ErrorCode.Message);
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {ErrorCode.Message}");
					else
						sb.Append($" | {ErrorCode.Message}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}

			if (!string.IsNullOrWhiteSpace(OperationName))
			{
				if (empty)
					sb.Append($"{nameof(OperationName)} = {OperationName}");
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {nameof(OperationName)} = {OperationName}");
					else
						sb.Append($" | {nameof(OperationName)} = {OperationName}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}

			if (!string.IsNullOrWhiteSpace(AggregateName))
			{
				if (empty)
					sb.Append($"{nameof(AggregateName)} = {AggregateName}");
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {nameof(AggregateName)} = {AggregateName}");
					else
						sb.Append($" | {nameof(AggregateName)} = {AggregateName}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}

			if (!string.IsNullOrWhiteSpace(AggregateIdentifier))
			{
				if (empty)
					sb.Append($"{nameof(AggregateIdentifier)} = {AggregateIdentifier}");
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" {nameof(AggregateIdentifier)} = {AggregateIdentifier}");
					else
						sb.Append($" | {nameof(AggregateIdentifier)} = {AggregateIdentifier}");
				}

				empty = false;
				hasOnlyErrorCode = false;
			}

			if (0 < ScopeContext.ContextProperties?.Count)
			{
				if (empty)
					sb.Append(string.Join("|", ScopeContext.ContextProperties.Select(x => $"{x.Key} = {x.Value}")));
				else
				{
					if (hasOnlyErrorCode)
						sb.Append($" | {string.Join(" | ", ScopeContext.ContextProperties.Select(x => $"{x.Key} = {x.Value}"))}");
					else
						sb.Append($" | {string.Join(" | ", ScopeContext.ContextProperties.Select(x => $"{x.Key} = {x.Value}"))}");
				}
			}
		}

		return sb.ToString();
	}

	public LogMessageDto ToDto(params string[] ignoredPropterties)
	{
		ignoredPropterties ??= [];
		var dto = new LogMessageDto();

		if (!ignoredPropterties.Contains(nameof(IdLogMessage)))
			dto.IdLogMessage = IdLogMessage;

		if (!ignoredPropterties.Contains(nameof(LogLevel)))
			dto.LogLevel = LogLevel;

		if (!ignoredPropterties.Contains(nameof(ErrorCode)))
			dto.ErrorCode = new ErrorCodeDto(ErrorCode);

		if (!ignoredPropterties.Contains(nameof(CreatedUtc)))
			dto.CreatedUtc = CreatedUtc;

		if (!ignoredPropterties.Contains(nameof(IsLogged)))
			dto.IsLogged = IsLogged;

		if (!ignoredPropterties.Contains(nameof(IsValidationError)))
			dto.IsValidationError = IsValidationError;

		if (!ignoredPropterties.Contains(nameof(ScopeContext)))
			dto.ScopeContext = ScopeContext;

		if (!ignoredPropterties.Contains(nameof(OperationName)))
			dto.OperationName = OperationName;

		if (!ignoredPropterties.Contains(nameof(AggregateName)))
			dto.AggregateName = AggregateName;

		if (!ignoredPropterties.Contains(nameof(AggregateIdentifier)))
			dto.AggregateIdentifier = AggregateIdentifier;

		if (!ignoredPropterties.Contains(nameof(ClientMessage)))
			dto.ClientMessage = ClientMessage;

		if (!ignoredPropterties.Contains(nameof(InternalMessage)))
			dto.InternalMessage = InternalMessage;

		if (!ignoredPropterties.Contains(nameof(Exception)))
			dto.Exception = Exception;

		if (!ignoredPropterties.Contains(nameof(StackTrace)))
			dto.StackTrace = StackTrace;

		if (!ignoredPropterties.Contains(nameof(Detail)))
			dto.Detail = Detail;

		if (!ignoredPropterties.Contains(nameof(PropertyName)))
			dto.PropertyName = PropertyName;

		if (!ignoredPropterties.Contains(nameof(ValidationFailure)))
			dto.ValidationFailure = ValidationFailure;

		if (!ignoredPropterties.Contains(nameof(DisplayPropertyName)))
			dto.DisplayPropertyName = DisplayPropertyName;

		if (!ignoredPropterties.Contains(nameof(SourceContext)))
			dto.SourceContext = SourceContext;

		return dto;
	}

	public LogMessageDto ToClientDto()
		=> new()
			{
				IdLogMessage = IdLogMessage,
				LogLevel = LogLevel,
				ErrorCode = new ErrorCodeDto(ErrorCode),
				CreatedUtc = CreatedUtc,
				IsLogged = IsLogged,
				IsValidationError = IsValidationError,
				//OperationName = OperationName,
				//AggregateName = AggregateName,
				//AggregateIdentifier = AggregateIdentifier,
				ClientMessage = ClientMessage,
				PropertyName = PropertyName
			};

	private static readonly Action<ILogger, string, string, string, string, string, LogMessage, Exception?> _logTrace
		= LoggerMessage.Define<string, string, string, string, string, LogMessage>(
			LogLevel.Trace,
			new EventId(1000, nameof(LogMessage)),
			"Code: {Code} | Message: {Message} | Detail: {Detail} | Properties: {Properties} | PropertyName: {PropertyName} | ID: {@LogMessage}");

	private static readonly Action<ILogger, string, string, string, string, string, LogMessage, Exception?> _logDebug
		= LoggerMessage.Define<string, string, string, string, string, LogMessage>(
			LogLevel.Debug,
			new EventId(1001, nameof(LogMessage)),
			"Code: {Code} | Message: {Message} | Detail: {Detail} | Properties: {Properties} | PropertyName: {PropertyName} | ID: {@LogMessage}");


	private static readonly Action<ILogger, string, string, string, string, string, LogMessage, Exception?> _logInformation
		= LoggerMessage.Define<string, string, string, string, string, LogMessage>(
			LogLevel.Information,
			new EventId(1002, nameof(LogMessage)),
			"Code: {Code} | Message: {Message} | Detail: {Detail} | Properties: {Properties} | PropertyName: {PropertyName} | ID: {@LogMessage}");


	private static readonly Action<ILogger, string, string, string, string, string, LogMessage, Exception?> _logWarning
		= LoggerMessage.Define<string, string, string, string, string, LogMessage>(
			LogLevel.Warning,
			new EventId(1003, nameof(LogMessage)),
			"Code: {Code} | Message: {Message} | Detail: {Detail} | Properties: {Properties} | PropertyName: {PropertyName} | ID: {@LogMessage}");


	private static readonly Action<ILogger, string, string, string, string, string, LogMessage, Exception?> _logError
		= LoggerMessage.Define<string, string, string, string, string, LogMessage>(
			LogLevel.Error,
			new EventId(1004, nameof(ErrorMessage)),
			"Code: {Code} | Message: {Message} | Detail: {Detail} | Properties: {Properties} | PropertyName: {PropertyName} | ID: {@LogMessage}");


	private static readonly Action<ILogger, string, string, string, string, string, LogMessage, Exception?> _logCritical
		= LoggerMessage.Define<string, string, string, string, string, LogMessage>(
			LogLevel.Critical,
			new EventId(1005, nameof(ErrorMessage)),
			"Code: {Code} | Message: {Message} | Detail: {Detail} | Properties: {Properties} | PropertyName: {PropertyName} | ID: {@LogMessage}");

	public void Log(ILogger logger)
	{
		Throw.IfArgumentNull(logger);

		var operationAndAggregate = "";
		if (!string.IsNullOrWhiteSpace(OperationName))
		{
			if (!string.IsNullOrWhiteSpace(AggregateName))
			{
				if (!string.IsNullOrWhiteSpace(AggregateIdentifier))
				{
					operationAndAggregate = $"{nameof(OperationName)} = {OperationName} / {nameof(AggregateName)} = {AggregateName} / {nameof(AggregateIdentifier)} = {AggregateIdentifier}";
				}
				else
				{
					operationAndAggregate = $"{nameof(OperationName)} = {OperationName} / {nameof(AggregateName)} = {AggregateName}";
				}
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(AggregateIdentifier))
				{
					operationAndAggregate = $"{nameof(OperationName)} = {OperationName} / {nameof(AggregateIdentifier)} = {AggregateIdentifier}";
				}
				else
				{
					operationAndAggregate = $"{nameof(OperationName)} = {OperationName}";
				}
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(AggregateName))
			{
				if (!string.IsNullOrWhiteSpace(AggregateIdentifier))
				{
					operationAndAggregate = $"{nameof(AggregateName)} = {AggregateName} / {nameof(AggregateIdentifier)} = {AggregateIdentifier}";
				}
				else
				{
					operationAndAggregate = $"{nameof(AggregateName)} = {AggregateName}";
				}
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(AggregateIdentifier))
				{
					operationAndAggregate = $"{nameof(AggregateIdentifier)} = {AggregateIdentifier}";
				}
				else
				{
					operationAndAggregate = null;
				}
			}
		}


		Action<ILogger, string, string, string, string, string, LogMessage, Exception?> log =
			LogLevel switch
			{
				LogLevel.Trace => _logTrace,
				LogLevel.Debug => _logDebug,
				LogLevel.Information => _logInformation,
				LogLevel.Warning => _logWarning,
				LogLevel.Error => _logError,
				LogLevel.Critical => _logCritical,
				LogLevel.None => (logger, errorCode, message, detail, properties, propertyName, id, ex) => { },
				_ => (logger, errorCode, message, detail, properties, propertyName, id, ex) => { }
			};

		log(
			logger,
			ErrorCode?.Code!,
			InternalMessage ?? ErrorCode?.Message!,
			StringHelper.Combine(Detail!, ClientMessage!, " / "),
			0 < ScopeContext?.ContextProperties?.Count
				? (operationAndAggregate == null
					? string.Join(" / ", ScopeContext.ContextProperties.Select(x => $"{x.Key} = {x.Value}"))
					: $"{operationAndAggregate} / {string.Join(" / ", ScopeContext.ContextProperties.Select(x => $"{x.Key} = {x.Value}"))}")
				: operationAndAggregate!,
			DisplayPropertyName!,
			this,
			Exception);
	}

	public string? ToMessageText(bool includeStackTrace = true)
		=> includeStackTrace
			? InternalMessage
				.ConcatIfNotNullOrEmpty(Environment.NewLine, ClientMessage)
				.ConcatIfNotNullOrEmpty(Environment.NewLine, Detail)
				.ConcatIfNotNullOrEmpty(Environment.NewLine, StackTrace)
			: InternalMessage
				.ConcatIfNotNullOrEmpty(Environment.NewLine, ClientMessage)
				.ConcatIfNotNullOrEmpty(Environment.NewLine, Detail);
}
