#if NET6_0_OR_GREATER
using Legion.Enums;
using Legion.Logging.Serializers.JsonConverters.Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Legion.Logging.Serializers.JsonConverters;

public class LogMessageJsonConverter : JsonConverter<ILogMessage>
{
	private static readonly Type _scopeContextType = typeof(IScopeContext);
	private static readonly Type _dateTimeOffset = typeof(DateTimeOffset);

	public override void Write(Utf8JsonWriter writer, ILogMessage value, JsonSerializerOptions options)
	{
		throw new NotImplementedException("Read only converter");
	}

	public override ILogMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return null;
		}

		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException();
		}
		else
		{
			var stringComparison = options.PropertyNameCaseInsensitive
				? StringComparison.OrdinalIgnoreCase
				: StringComparison.Ordinal;

			var logMessage = new DeserializedLogMessage
			{
				DeserializedErrorCode = new DeserializedErrorCode()
			};

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					return logMessage;
				}

				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					string? value;
					var propertyName = reader.GetString();
					reader.Read();
					switch (propertyName)
					{
						case var name when string.Equals(name, nameof(ILogMessage.IdLogMessage), stringComparison):
							value = reader.GetString();
							logMessage.IdLogMessage = Guid.TryParse(value, out var idLogMessage) ? idLogMessage : idLogMessage;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.IdLogLevel), stringComparison):
							if (reader.TokenType != JsonTokenType.Null && reader.TryGetInt32(out var idLogLevel))
								logMessage.LogLevel = EnumHelper.ConvertIntToEnum<LogLevel>(idLogLevel);
							break;
						case var name when string.Equals(name, nameof(ILogMessage.CreatedUtc), stringComparison):
							logMessage.CreatedUtc = reader.TokenType == JsonTokenType.Null ? default : ((JsonConverter<DateTimeOffset>)options.GetConverter(_dateTimeOffset)).Read(ref reader, _dateTimeOffset, options);
							break;
						case var name when string.Equals(name, nameof(ILogMessage.ScopeContext), stringComparison):
							logMessage.ScopeContext = reader.TokenType == JsonTokenType.Null
								? null!
								: ((JsonConverter<IScopeContext>)options.GetConverter(_scopeContextType)).Read(ref reader, _scopeContextType, options)!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.ErrorCode), stringComparison):
							logMessage.DeserializedErrorCode.Code = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.OperationName), stringComparison):
							logMessage.OperationName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.AggregateName), stringComparison):
							logMessage.AggregateName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.AggregateIdentifier), stringComparison):
							logMessage.AggregateIdentifier = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.ClientMessage), stringComparison):
							logMessage.ClientMessage = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.InternalMessage), stringComparison):
							logMessage.InternalMessage = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.StackTrace), stringComparison):
							logMessage.StackTrace = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.Detail), stringComparison):
							logMessage.Detail = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.PropertyName), stringComparison):
							logMessage.PropertyName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.DisplayPropertyName), stringComparison):
							logMessage.DisplayPropertyName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(ILogMessage.IsValidationError), stringComparison):
							logMessage.IsValidationError = reader.TokenType != JsonTokenType.Null && reader.GetBoolean();
							break;
					}
				}
			}

			return default;
		}
	}
}
#endif
