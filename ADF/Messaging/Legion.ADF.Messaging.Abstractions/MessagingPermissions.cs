namespace Legion.ADF.Messaging;

public static class MessagingPermissions
{
	public enum DomainEvent
	{
		SaveDomainEventAsync
	}

	public enum InboxMessage
	{
		SaveInboxMessage,
		ArchivateInboxMessage
	}

	public enum InboxInstance
	{
		CreateInboxInstance
	}

	public enum InboxMessageType
	{
		CreateInboxMessageType
	}

	public enum InboxQueue
	{
		CreateInboxQueue
	}

	public enum BlockedInboxMessageType
	{
		SaveBlockedInboxMessageType
	}

	public enum OutboxMessage
	{
		SaveOutboxMessage,
		ArchivateOutboxMessage
	}

	public enum OutboxInstance
	{
		CreateOutboxInstance
	}

	public enum OutboxMessageType
	{
		CreateOutboxMessageType
	}

	public enum OutboxQueue
	{
		CreateOutboxQueue
	}

	public enum BlockedOutboxMessageType
	{
		SaveBlockedOutboxMessageType
	}

	public enum MessageBoxMessage
	{
		SaveMessage,
		ArchivateMessage
	}

	public enum MessageBoxInstance
	{
		CreateMessageBoxInstance
	}

	public enum MessageBoxMessageType
	{
		CreateMessageBoxMessageType
	}

	public enum MessageBoxQueue
	{
		CreateMessageBoxQueue
	}

	public enum MessageBoxTopic
	{
		CreateMessageBoxTopic
	}

	public enum BlockedMessageBoxMessageType
	{
		SaveBlockedMessageType
	}
}
