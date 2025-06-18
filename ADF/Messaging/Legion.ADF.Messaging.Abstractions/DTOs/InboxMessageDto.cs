using Legion.Extensions;
using Legion.Serializer;
using System.Text;

namespace Legion.ADF.Messaging.DTOs;

public class InboxMessageDto
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
	public string? TargetTopic { get; set; }
	public string? TargetQueueName { get; set; }
	public DTOs.Content Content { get; set; }

	public InboxMessageDto()
	{	
	}

	public static InboxMessageDto CreateJsonMessage<T>(
		T message,
		string messageId,
		string? publisher = null,
		string? publisherId = null,
		string? targetTopic = null,
		string? targetQueueName = null)
		where T : class
	{
		Throw.IfArgumentNull(message);
		Throw.IfArgumentNullOrWhiteSpace(messageId);

		var json = JsonSerializerHelper.Serialize(message, GlobalCache.JsonSerializerSettings_WithRecursiveObjs);

		return new InboxMessageDto
		{
			MessageTypeNamespace = typeof(T).GetSimplifiedAssemblyQualifiedName(),
			MessageId = messageId,
			Publisher = publisher,
			PublisherId = publisherId,
			TargetTopic = targetTopic,
			TargetQueueName = targetQueueName,
			Content = new DTOs.JsonContent(json)
		};
	}

	public static InboxMessageDto CreateStringMessage(
		string @namespace,
		string message,
		string messageId,
		string? publisher = null,
		string? publisherId = null,
		string? targetTopic = null,
		string? targetQueueName = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(messageId);

		return new InboxMessageDto
		{
			MessageTypeNamespace = @namespace,
			MessageId = messageId,
			Publisher = publisher,
			PublisherId = publisherId,
			TargetTopic = targetTopic,
			TargetQueueName = targetQueueName,
			Content = new DTOs.StringContent(message, Encoding.UTF8)
		};
	}
}
