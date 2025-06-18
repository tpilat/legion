#if NET6_0_OR_GREATER
using Legion.Enums;
using Legion.Logging.Serializers.JsonConverters.Model;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Legion.Logging.Serializers.JsonConverters;

public class ErrorMessageJsonConverter : JsonConverter<IErrorMessage>
{
	private static readonly Type _scopeContextType = typeof(IScopeContext);
	private static readonly Type _dateTimeOffset = typeof(DateTimeOffset);

	public override void Write(Utf8JsonWriter writer, IErrorMessage value, JsonSerializerOptions options)
	{
		throw new NotImplementedException("Read only converter");
	}

	public override IErrorMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

			var errorMessage = new DeserializedErrorMessage
			{
				DeserializedErrorCode = new DeserializedErrorCode()
			};

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					return errorMessage;
				}

				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					string? value;
					var propertyName = reader.GetString();
					reader.Read();
					switch (propertyName)
					{
						case var name when string.Equals(name, nameof(IErrorMessage.IdLogMessage), stringComparison):
							value = reader.GetString();
							errorMessage.IdLogMessage = Guid.TryParse(value, out var idLogMessage) ? idLogMessage : idLogMessage;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.IdLogLevel), stringComparison):
							if (reader.TokenType != JsonTokenType.Null && reader.TryGetInt32(out var idLogLevel))
								errorMessage.LogLevel = EnumHelper.ConvertIntToEnum<LogLevel>(idLogLevel);
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.CreatedUtc), stringComparison):
							errorMessage.CreatedUtc = reader.TokenType == JsonTokenType.Null ? default : ((JsonConverter<DateTimeOffset>)options.GetConverter(_dateTimeOffset)).Read(ref reader, _dateTimeOffset, options);
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.ScopeContext), stringComparison):
							errorMessage.ScopeContext = reader.TokenType == JsonTokenType.Null
								? null!
								: ((JsonConverter<IScopeContext>)options.GetConverter(_scopeContextType)).Read(ref reader, _scopeContextType, options)!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.ErrorCode), stringComparison):
							errorMessage.DeserializedErrorCode.Code = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.OperationName), stringComparison):
							errorMessage.OperationName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.AggregateName), stringComparison):
							errorMessage.AggregateName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.AggregateIdentifier), stringComparison):
							errorMessage.AggregateIdentifier = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.ClientMessage), stringComparison):
							errorMessage.ClientMessage = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.InternalMessage), stringComparison):
							errorMessage.InternalMessage = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.StackTrace), stringComparison):
							errorMessage.StackTrace = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.Detail), stringComparison):
							errorMessage.Detail = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.PropertyName), stringComparison):
							errorMessage.PropertyName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.DisplayPropertyName), stringComparison):
							errorMessage.DisplayPropertyName = reader.GetString()!;
							break;
						case var name when string.Equals(name, nameof(IErrorMessage.IsValidationError), stringComparison):
							errorMessage.IsValidationError = reader.TokenType != JsonTokenType.Null && reader.GetBoolean();
							break;
					}
				}
			}

			return default;
		}
	}
}
#endif
