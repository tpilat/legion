using Microsoft.Extensions.Logging;

namespace Legion.Logging;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface ILogMessage : Serializer.IDictionaryObject
{
	Guid IdLogMessage { get; set; }

	LogLevel LogLevel { get; set; }

	int IdLogLevel { get; }

	IErrorCode? ErrorCode { get; set; }

	DateTimeOffset CreatedUtc { get; set; }

	IScopeContext ScopeContext { get; set; }

	string? OperationName { get; set; }

	string? AggregateName { get; set; }

	string? AggregateIdentifier { get; set; }

	string? ClientMessage { get; set; }

	string? InternalMessage { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	Exception? Exception { get; set; }
	bool ShouldSerializeException();

	string? StackTrace { get; set; }

	string? Detail { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	string ClientMessageWithId { get; }
	bool ShouldSerializeClientMessageWithId();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	string ClientMessageWithIdAndPropName { get; }
	bool ShouldSerializeClientMessageWithIdAndPropName();

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	string FullMessage { get; }
	bool ShouldSerializeFullMessage();

	bool IsLogged { get; set; }

	string? PropertyName { get; set; }

	string? SourceContext { get; set; }

	object? ValidationFailure { get; set; }

	string? DisplayPropertyName { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	bool IsValidationError { get; set; }
	bool ShouldSerializeValidationFailure();

	public bool DisableTransactionRollback { get; set; }

	string ToString(bool withId, bool withPropertyName, bool withDetail);

	Exception ToException();

	string ToMessage();

	LogMessageDto ToDto(params string[] ignoredPropterties);

	LogMessageDto ToClientDto();

	void Log(ILogger logger);

	string? ToMessageText(bool includeStackTrace = true);
}
