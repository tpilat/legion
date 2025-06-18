using Legion.Exceptions;
using Legion.Serializer;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Legion.Logging;

public class LogMessageDto : ILogMessage
{
	private string? _operationName;

	public Guid IdLogMessage { get; set; }
	public LogLevel LogLevel { get; set; }
	public int IdLogLevel => (int)LogLevel;

	public ErrorCodeDto? ErrorCode { get; set; }
	IErrorCode? ILogMessage.ErrorCode { get => ErrorCode; set => ErrorCode = new ErrorCodeDto(value); }
	public DateTimeOffset CreatedUtc { get; set; }
	public IScopeContext ScopeContext { get; set; }
	public string? OperationName
	{
		get => _operationName ?? ScopeContext?.GetLastTraceFrame();
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public LogMessageDto()
	{
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public LogMessage ToLogMessage(params string[] ignoredPropterties)
	{
		ignoredPropterties ??= [];
		var logMessage = new LogMessage(ScopeContext, ErrorCode);

		if (!ignoredPropterties.Contains(nameof(IdLogMessage)))
			logMessage.IdLogMessage = IdLogMessage;

		if (!ignoredPropterties.Contains(nameof(LogLevel)))
			logMessage.LogLevel = LogLevel;

		if (!ignoredPropterties.Contains(nameof(CreatedUtc)))
			logMessage.CreatedUtc = CreatedUtc;

		if (!ignoredPropterties.Contains(nameof(IsLogged)))
			logMessage.IsLogged = IsLogged;

		if (!ignoredPropterties.Contains(nameof(IsValidationError)))
			logMessage.IsValidationError = IsValidationError;

		if (!ignoredPropterties.Contains(nameof(ScopeContext)))
			logMessage.ScopeContext = ScopeContext;

		if (!ignoredPropterties.Contains(nameof(OperationName)))
			logMessage.OperationName = OperationName;

		if (!ignoredPropterties.Contains(nameof(AggregateName)))
			logMessage.AggregateName = AggregateName;

		if (!ignoredPropterties.Contains(nameof(AggregateIdentifier)))
			logMessage.AggregateIdentifier = AggregateIdentifier;

		if (!ignoredPropterties.Contains(nameof(ClientMessage)))
			logMessage.ClientMessage = ClientMessage;

		if (!ignoredPropterties.Contains(nameof(InternalMessage)))
			logMessage.InternalMessage = InternalMessage;

		if (!ignoredPropterties.Contains(nameof(Exception)))
			logMessage.Exception = Exception;

		if (!ignoredPropterties.Contains(nameof(StackTrace)))
			logMessage.StackTrace = StackTrace;

		if (!ignoredPropterties.Contains(nameof(Detail)))
			logMessage.Detail = Detail;

		if (!ignoredPropterties.Contains(nameof(PropertyName)))
			logMessage.PropertyName = PropertyName;

		if (!ignoredPropterties.Contains(nameof(ValidationFailure)))
			logMessage.ValidationFailure = ValidationFailure;

		if (!ignoredPropterties.Contains(nameof(DisplayPropertyName)))
			logMessage.DisplayPropertyName = DisplayPropertyName;

		if (!ignoredPropterties.Contains(nameof(SourceContext)))
			logMessage.SourceContext = SourceContext;

		return logMessage;
	}

	public IDictionary<string, object?> ToDictionary(Serializer.ISerializer? serializer = null)
	{
		var dict = new Dictionary<string, object?>
		{
			{ nameof(IdLogMessage), IdLogMessage },
			//{ nameof(LogLevel), LogLevel },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IsValidationError), IsValidationError }
		};

		if (ScopeContext != null)
			dict.Add(nameof(ScopeContext.RuntimeUniqueKey), ScopeContext.RuntimeUniqueKey);

		if (ErrorCode != null)
		{
			dict.Add($"{nameof(ErrorCode)}.{nameof(ErrorCode.Code)}", ErrorCode.Code);
			dict.Add($"{nameof(ErrorCode)}.{nameof(ErrorCode.Message)}", ErrorCode.Message);
			dict.Add($"{nameof(ErrorCode)}.{nameof(ErrorCode.Description)}", ErrorCode.Description);
		}

		if (ScopeContext?.IdApplicationEntry.HasValue == true)
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

		if (!string.IsNullOrWhiteSpace(ScopeContext?.SourceSystemName))
			dict.Add(nameof(ScopeContext.SourceSystemName), ScopeContext!.SourceSystemName);

		if (ScopeContext?.TraceFrameStack != null)
			dict.Add(nameof(ScopeContext.TraceFrameStack), $"{ScopeContext.TraceFrameStack}");

		if (!string.IsNullOrWhiteSpace(StackTrace))
			dict.Add(nameof(StackTrace), StackTrace);

		if (!string.IsNullOrWhiteSpace(Detail))
			dict.Add(nameof(Detail), Detail);

		if (ScopeContext?.IdUser.HasValue == true)
			dict.Add(nameof(ScopeContext.IdUser), ScopeContext.IdUser);

		if (!string.IsNullOrWhiteSpace(PropertyName))
			dict.Add(nameof(PropertyName), PropertyName);

		if (!string.IsNullOrWhiteSpace(DisplayPropertyName))
			dict.Add(nameof(DisplayPropertyName), DisplayPropertyName);

		if (ValidationFailure != null)
			dict.Add(nameof(ValidationFailure), ValidationFailure.ToString());

		if (ScopeContext?.CorrelationId.HasValue == true)
			dict.Add(nameof(ScopeContext.CorrelationId), ScopeContext.CorrelationId.Value);

		if (ScopeContext?.ContextProperties != null && 0 < ScopeContext.ContextProperties.Count)
#if NETSTANDARD2_0 || NETSTANDARD2_1
			dict.Add(nameof(ScopeContext.ContextProperties), Newtonsoft.Json.JsonConvert.SerializeObject(ScopeContext.ContextProperties));
#elif NET6_0_OR_GREATER
			dict.Add(nameof(ScopeContext.ContextProperties), JsonSerializerHelper.Serialize(ScopeContext.ContextProperties));
#endif

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
			sb.Append($"{ErrorCode!.Code}:");

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
						sb.Append($" | {string.Join("|", ScopeContext.ContextProperties.Select(x => $"{x.Key} = {x.Value}"))}");
					else
						sb.Append($" | {string.Join("|", ScopeContext.ContextProperties.Select(x => $"{x.Key} = {x.Value}"))}");
				}
			}
		}

		return sb.ToString();
	}

	LogMessageDto ILogMessage.ToDto(params string[] ignoredPropterties)
	{
		Throw.NotSupportedException();
		return null;
	}

	LogMessageDto ILogMessage.ToClientDto()
	{
		Throw.NotSupportedException();
		return null;
	}

	public void Log(ILogger logger)
		=> ToLogMessage().Log(logger);
}
