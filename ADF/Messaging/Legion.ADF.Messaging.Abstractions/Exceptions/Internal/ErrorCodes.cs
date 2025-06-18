using Legion.Exceptions;
using Legion.Extensions;

namespace Legion.ADF.Messaging.Exceptions.Internal;

public static partial class ErrorCodes
{
	public static partial class ConnectionStringProviderException
	{
		public static IErrorCode InvalidStoreId(string storeId)
			=> new ErrorCode(
				"ADFMsg_CONN-STR_0001",
				$"Invalid connection string strore ID = {storeId}");
	}

	public static partial class DomainEventsUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"ADFMsg_DomEvt_UoW_0001",
				$"Cannot create UnitOfWork"));
	}

	public static partial class InboxUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"ADFMsg_Inbox_UoW_0001",
				$"Cannot create UnitOfWork"));

		public static IErrorCode SaveFailed => _saveFailed.Value;
		private static readonly Lazy<IErrorCode> _saveFailed = new(() =>
			new ErrorCode(
				"ADFMsg_Inbox_UoW_0002",
				$"Cannot save InboxUnitOfWork"));
	}

	public static partial class InboxMessageContent
	{
		public static IErrorCode InvalidContent(Type contentType)
			=> new ErrorCode(
				"ADFMsg_Inbox_MsgType_0001",
				$"Invalid message conent type = {contentType?.ToFriendlyFullName()}");
	}

	public static partial class OutboxUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"ADFMsg_Outbox_UoW_0001",
				$"Cannot create UnitOfWork"));

		public static IErrorCode SaveFailed => _saveFailed.Value;
		private static readonly Lazy<IErrorCode> _saveFailed = new(() =>
			new ErrorCode(
				"ADFMsg_Outbox_UoW_0002",
				$"Cannot save OutboxUnitOfWork"));
	}

	public static partial class OutboxMessageContent
	{
		public static IErrorCode InvalidContent(Type contentType)
			=> new ErrorCode(
				"ADFMsg_Outbox_MsgType_0001",
				$"Invalid message conent type = {contentType?.ToFriendlyFullName()}");
	}

	public static partial class MessageBoxUnitOfWorkException
	{
		public static IErrorCode InvalidUoW => _invalidUoW.Value;
		private static readonly Lazy<IErrorCode> _invalidUoW = new(() =>
			new ErrorCode(
				"ADFMsg_MBox_UoW_0001",
				$"Cannot create UnitOfWork"));

		public static IErrorCode SaveFailed => _saveFailed.Value;
		private static readonly Lazy<IErrorCode> _saveFailed = new(() =>
			new ErrorCode(
				"ADFMsg_MBox_UoW_0002",
				$"Cannot save MessageBoxUnitOfWork"));
	}

	public static partial class MessageBoxMessageContent
	{
		public static IErrorCode InvalidContent(Type contentType)
			=> new ErrorCode(
				"ADFMsg_MBox_MsgType_0001",
				$"Invalid message conent type = {contentType?.ToFriendlyFullName()}");
	}

	public static partial class DomainEventProcessingService
	{
		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"ADFMsg_DomEvt_SVC_0001",
				$"Unhandled exception in DomainEventProcessingService"));

		public static IErrorCode CanNotSetState(string status)
			=> new ErrorCode(
				"ADFMsg_DomEvt_SVC_0002",
				$"DomainEventProcessingService cannot set status {status}");

		public static IErrorCode InvalidDomainEventType(Type? deserializedDomainEventType)
			=> new ErrorCode(
				"ADFMsg_DomEvt_SVC_0003",
				$"Invalid {nameof(Legion.Model.IDomainEvent)} type = {deserializedDomainEventType?.FullName ?? "NULL"}");

		public static IErrorCode DomainEventHasNoHandler(Type? deserializedDomainEventType)
			=> new ErrorCode(
				"ADFMsg_DomEvt_SVC_0004",
				$"No handler was found for event type = {deserializedDomainEventType?.FullName ?? "NULL"}");

		public static IErrorCode BlockedDomainEvent(string @namespace)
			=> new ErrorCode(
				"ADFMsg_DomEvt_SVC_0005",
				$"Domain event namespace = {@namespace} is blocked");
	}

	public static partial class InboxMessageProcessingService
	{
		public static IErrorCode CannotCreateInboxInstance => _cannotCreateInboxInstance.Value;
		private static readonly Lazy<IErrorCode> _cannotCreateInboxInstance = new(() =>
			new ErrorCode(
				"ADFMsg_Inbox_SVC_0000",
				$"Cannot create InboxInstance"));

		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"ADFMsg_Inbox_SVC_0001",
				$"Unhandled exception in InboxMessageProcessingService"));

		public static IErrorCode CanNotSetState(string status)
			=> new ErrorCode(
				"ADFMsg_Inbox_SVC_0002",
				$"InboxMessageProcessingService cannot set status {status}");

		public static IErrorCode InvalidInboxQueueReceivedEventType(string receivedEventNamespace)
			=> new ErrorCode(
				"ADFMsg_Inbox_SVC_0003",
				$"Invalid queue received event type = {receivedEventNamespace ?? "NULL"}");

		public static IErrorCode InboxMessageHasUnknownType(string messageTypeNamespace)
			=> new ErrorCode(
				"ADFMsg_Inbox_SVC_0004",
				$"Inbox message has unknown type {messageTypeNamespace ?? "NULL"}");

		public static IErrorCode InboxMessageHasNoHandler(string receivedEventNamespace)
			=> new ErrorCode(
				"ADFMsg_Inbox_SVC_0005",
				$"No handler was found for queue received event type = {receivedEventNamespace ?? "NULL"}");

		public static IErrorCode FailedToWriteProcessingLog => _failedToWriteProcessingLog.Value;
		private static readonly Lazy<IErrorCode> _failedToWriteProcessingLog = new(() =>
			new ErrorCode(
				"ADFMsg_Inbox_SVC_0006",
				$"Failed to write Inbox processing log"));

		public static IErrorCode NoQueue => _noQueue.Value;
		private static readonly Lazy<IErrorCode> _noQueue = new(() =>
			new ErrorCode(
				"ADFMsg_Inbox_SVC_0007",
				$"No Inbox queue"));
	}

	public static partial class OutboxMessageProcessingService
	{
		public static IErrorCode CannotCreateOutboxInstance => _cannotCreateOutboxInstance.Value;
		private static readonly Lazy<IErrorCode> _cannotCreateOutboxInstance = new(() =>
			new ErrorCode(
				"ADFMsg_Outbox_SVC_0000",
				$"Cannot create OutboxInstance"));

		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"ADFMsg_Outbox_SVC_0001",
				$"Unhandled exception in OutboxMessageProcessingService"));

		public static IErrorCode CanNotSetState(string status)
			=> new ErrorCode(
				"ADFMsg_Outbox_SVC_0002",
				$"OutboxMessageProcessingService cannot set status {status}");

		public static IErrorCode InvalidOutboxQueueReceivedEventType(string receivedEventNamespace)
			=> new ErrorCode(
				"ADFMsg_Outbox_SVC_0003",
				$"Invalid queue received event type = {receivedEventNamespace ?? "NULL"}");

		public static IErrorCode OutboxMessageHasUnknownType(string messageTypeNamespace)
			=> new ErrorCode(
				"ADFMsg_Outbox_SVC_0004",
				$"Outbox message has unknown type {messageTypeNamespace ?? "NULL"}");

		public static IErrorCode OutboxMessageHasNoHandler(string receivedEventNamespace)
			=> new ErrorCode(
				"ADFMsg_Outbox_SVC_0005",
				$"No handler was found for queue received event type = {receivedEventNamespace ?? "NULL"}");

		public static IErrorCode FailedToWriteProcessingLog => _failedToWriteProcessingLog.Value;
		private static readonly Lazy<IErrorCode> _failedToWriteProcessingLog = new(() =>
			new ErrorCode(
				"ADFMsg_Outbox_SVC_0006",
				$"Failed to write Outbox processing log"));

		public static IErrorCode NoQueue => _noQueue.Value;
		private static readonly Lazy<IErrorCode> _noQueue = new(() =>
			new ErrorCode(
				"ADFMsg_Outbox_SVC_0007",
				$"No Outbox queue"));
	}

	public static partial class MessageBoxProcessingService
	{
		public static IErrorCode CannotCreateMessageBoxInstance => _cannotCreateMessageBoxInstance.Value;
		private static readonly Lazy<IErrorCode> _cannotCreateMessageBoxInstance = new(() =>
			new ErrorCode(
				"ADFMsg_MBox_SVC_0000",
				$"Cannot create MessageBoxInstance"));

		public static IErrorCode Default => _default.Value;
		private static readonly Lazy<IErrorCode> _default = new(() =>
			new ErrorCode(
				"ADFMsg_MBox_SVC_0001",
				$"Unhandled exception in MessageBoxProcessingService"));

		public static IErrorCode CanNotSetState(string status)
			=> new ErrorCode(
				"ADFMsg_MBox_SVC_0002",
				$"MessageBoxProcessingService cannot set status {status}");

		public static IErrorCode InvalidMessageBoxQueueReceivedEventType(string receivedEventNamespace)
			=> new ErrorCode(
				"ADFMsg_MBox_SVC_0003",
				$"Invalid queue received event type = {receivedEventNamespace ?? "NULL"}");

		public static IErrorCode MessageHasUnknownType(string messageTypeNamespace)
			=> new ErrorCode(
				"ADFMsg_MBox_SVC_0004",
				$"MessageBox message has unknown type {messageTypeNamespace ?? "NULL"}");

		public static IErrorCode MessageHasNoHandler(string receivedEventNamespace)
			=> new ErrorCode(
				"ADFMsg_MBox_SVC_0005",
				$"No handler was found for queue received event type = {receivedEventNamespace ?? "NULL"}");

		public static IErrorCode FailedToWriteProcessingLog => _failedToWriteProcessingLog.Value;
		private static readonly Lazy<IErrorCode> _failedToWriteProcessingLog = new(() =>
			new ErrorCode(
				"ADFMsg_MBox_SVC_0006",
				$"Failed to write MessageBox processing log"));

		public static IErrorCode NoQueue => _noQueue.Value;
		private static readonly Lazy<IErrorCode> _noQueue = new(() =>
			new ErrorCode(
				"ADFMsg_MBox_SVC_0007",
				$"No MessageBox queue"));

		public static IErrorCode NoTopic => _noTopic.Value;
		private static readonly Lazy<IErrorCode> _noTopic = new(() =>
			new ErrorCode(
				"ADFMsg_MBox_SVC_0008",
				$"No MessageBox topic"));

		public static IErrorCode NoTopicSubscription(string topicName)
			=> new ErrorCode(
				"ADFMsg_MBox_SVC_0009",
				$"No MessageBox topic subscription for topic = {topicName}");
	}
}
