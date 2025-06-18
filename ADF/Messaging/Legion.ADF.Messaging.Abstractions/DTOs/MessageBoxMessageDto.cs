using Legion.Extensions;
using Legion.Serializer;
using System.Text;

namespace Legion.ADF.Messaging.DTOs;

public class MessageBoxMessageDto
{
	public string MessageTypeNamespace { get; set; }
	public string? MessageId { get; set; }
	public string? BusinessId { get; set; }
	public string? CorrelationId { get; set; }
	public Guid? SessionId { get; set; }
	public long? SessionMessagePartId { get; set; }
	public string? Properties { get; set; }
	public string? Publisher { get; set; }
	public string? PublisherId { get; set; }
	public DateTime? ValidToUtc { get; set; }
	public int? Priority { get; set; }
	public DTOs.Content Content { get; set; }

	public MessageBoxMessageDto()
	{	
	}

	public static MessageBoxMessageDto CreateJsonMessage<T>(
		T message,
		string messageId,
		string? publisher = null,
		string? publisherId = null)
		where T : class
	{
		Throw.IfArgumentNull(message);
		Throw.IfArgumentNullOrWhiteSpace(messageId);

		var json = JsonSerializerHelper.Serialize(message, GlobalCache.JsonSerializerSettings_WithRecursiveObjs);

		return new MessageBoxMessageDto
		{
			MessageTypeNamespace = typeof(T).GetSimplifiedAssemblyQualifiedName(),
			MessageId = messageId,
			Publisher = publisher,
			PublisherId = publisherId,
			Content = new DTOs.JsonContent(json)
		};
	}

	public static MessageBoxMessageDto CreateStringMessage(
		string @namespace,
		string message,
		string messageId,
		string? publisher = null,
		string? publisherId = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(messageId);

		return new MessageBoxMessageDto
		{
			MessageTypeNamespace = @namespace,
			MessageId = messageId,
			Publisher = publisher,
			PublisherId = publisherId,
			Content = new DTOs.StringContent(message, Encoding.UTF8)
		};
	}
}
