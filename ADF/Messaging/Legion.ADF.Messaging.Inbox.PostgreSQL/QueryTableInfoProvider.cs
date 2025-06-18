using Legion.Extensions;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.Messaging.Inbox.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwBlockedInboxMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "\"VwBlockedInboxMessageType\"",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.IdBlockedInboxMessageType), typeof(Guid), "\"IdBlockedInboxMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.IdInboxInstance), typeof(Guid), "\"IdInboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwBlockedInboxMessageTypeTableInfo()
		=> _VwBlockedInboxMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "\"VwInboxMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxMessage), typeof(Guid), "\"IdInboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxMessageStatus), typeof(Guid), "\"IdInboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.InboxMessageStatusCode), typeof(string), "\"InboxMessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.InboxMessageStatusName), typeof(string), "\"InboxMessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxQueue), typeof(Guid?), "\"IdInboxQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.TargetTopic), typeof(string), "\"TargetTopic\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.TargetQueueName), typeof(string), "\"TargetQueueName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxInstance), typeof(Guid), "\"IdInboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageTableInfo()
		=> _VwInboxMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "\"VwInboxMessageArchive\"",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxMessage), typeof(Guid), "\"IdInboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxMessageStatus), typeof(Guid), "\"IdInboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.InboxMessageStatusCode), typeof(string), "\"InboxMessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.InboxMessageStatusName), typeof(string), "\"InboxMessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxQueue), typeof(Guid), "\"IdInboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.TargetTopic), typeof(string), "\"TargetTopic\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.TargetQueueName), typeof(string), "\"TargetQueueName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxInstance), typeof(Guid), "\"IdInboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageArchiveTableInfo()
		=> _VwInboxMessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "\"VwInboxMessageContent\"",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.IdInboxMessageContent), typeof(Guid), "\"IdInboxMessageContent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageContentTableInfo()
		=> _VwInboxMessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "\"VwInboxMessageProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxMessageProcessingLog), typeof(Guid), "\"IdInboxMessageProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxMessage), typeof(Guid), "\"IdInboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxQueue), typeof(Guid), "\"IdInboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxMessageStatus), typeof(Guid), "\"IdInboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.InboxMessageStatusCode), typeof(string), "\"InboxMessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.InboxMessageStatusName), typeof(string), "\"InboxMessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxInstance), typeof(Guid), "\"IdInboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageProcessingLogTableInfo()
		=> _VwInboxMessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxQueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "\"VwInboxQueue\"",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdInboxQueue), typeof(Guid), "\"IdInboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.Name), typeof(string), "\"Name\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.ReceivedEventNamespace), typeof(string), "\"ReceivedEventNamespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdMessageType), typeof(Guid?), "\"IdMessageType\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdInboxInstance), typeof(Guid), "\"IdInboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.ProcessingModeCode), typeof(string), "\"ProcessingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.ProcessingModeName), typeof(string), "\"ProcessingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.SuspendingModeCode), typeof(string), "\"SuspendingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.SuspendingModeName), typeof(string), "\"SuspendingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxQueueTableInfo()
		=> _VwInboxQueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxQueueMessagesTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "\"VwInboxQueueMessages\"",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.IdInboxQueue), typeof(Guid), "\"IdInboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.InboxQueueName), typeof(string), "\"InboxQueueName\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.CreatedMessageCount), typeof(long), "\"CreatedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.ProcessingMessageCount), typeof(long), "\"ProcessingMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.ProcessedMessageCount), typeof(long), "\"ProcessedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.FailedMessageCount), typeof(long), "\"FailedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.SuspendedMessageCount), typeof(long), "\"SuspendedMessageCount\"", "bigint", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxQueueMessagesTableInfo()
		=> _VwInboxQueueMessagesTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType), GetVwBlockedInboxMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage), GetVwInboxMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive), GetVwInboxMessageArchiveTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent), GetVwInboxMessageContentTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog), GetVwInboxMessageProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue), GetVwInboxQueueTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages), GetVwInboxQueueMessagesTableInfo() },
		});

	public IReadOnlyDictionary<Type, Legion.Database.Metamodel.Info.TableInfo> TableInfoDictionary => _tableInfoDictionary.Value;

	public Legion.Database.Metamodel.Info.TableInfo GetTableInfo<T>()
		=> GetTableInfo(typeof(T));

	public Legion.Database.Metamodel.Info.TableInfo GetTableInfo(Type type)
	{
		if (TableInfoDictionary.TryGetValue(type, out var tableInfo))
			return tableInfo;

		Legion.Throw.InvalidOperationException($"Invalid entity type = {type.ToFriendlyFullName()}");
		return null;
	}
}
