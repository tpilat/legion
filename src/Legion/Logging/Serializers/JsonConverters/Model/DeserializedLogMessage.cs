using Legion.Serializer;
using Legion.Text;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Legion.Logging.Serializers.JsonConverters.Model;

public class DeserializedLogMessage : ILogMessage
{
	private string? _operationName;

	public Guid IdLogMessage { get; set; }

	public LogLevel LogLevel { get; set; }

	public int IdLogLevel => (int)LogLevel;

	public DeserializedErrorCode DeserializedErrorCode { get; set; }
	public IErrorCode ErrorCode
	{
		get
		{
			if (DeserializedErrorCode == null)
				return new DeserializedErrorCode();

			return DeserializedErrorCode;
		}
		set { }
	}

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

	public string? StackTrace { get; set; }

	public string? Detail { get; set; }

	public bool IsLogged { get; set; }

	public string? PropertyName { get; set; }

	public object? ValidationFailure { get; set; }

	public string? DisplayPropertyName { get; set; }

	public bool DisableTransactionRollback { get; set; }

	public bool IsValidationError { get; set; }

	public string? SourceContext { get; set; }




	public Exception? Exception { get; set; }

	public string ClientMessageWithId => ToString(true, false, false);

	public string ClientMessageWithIdAndPropName => ToString(true, true, false);

	public string FullMessage => ToString(true, true, true);

	bool ILogMessage.ShouldSerializeClientMessageWithId()
		=> throw new NotImplementedException();

	bool ILogMessage.ShouldSerializeClientMessageWithIdAndPropName()
		=> throw new NotImplementedException();

	bool ILogMessage.ShouldSerializeException()
		=> throw new NotImplementedException();

	bool ILogMessage.ShouldSerializeFullMessage()
		=> throw new NotImplementedException();

	bool ILogMessage.ShouldSerializeValidationFailure()
		=> throw new NotImplementedException();

	LogMessageDto ILogMessage.ToClientDto()
		=> throw new NotImplementedException();

	IDictionary<string, object?> IDictionaryObject.ToDictionary(ISerializer? serializer)
		=> throw new NotImplementedException();

	LogMessageDto ILogMessage.ToDto(params string[] ignoredPropterties)
		=> throw new NotImplementedException();

	Exception ILogMessage.ToException()
		=> throw new NotImplementedException();

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

	public void Log(ILogger logger)
		=> throw new NotImplementedException();

	public override string ToString()
	{
		//return FullMessage;
		return IdLogMessage.ToString();
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
