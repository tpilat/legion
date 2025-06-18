using Legion.Extensions;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.Messaging.Outbox.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwBlockedOutboxMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"VwBlockedOutboxMessageType\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType.IdBlockedOutboxMessageType), typeof(Guid), "\"IdBlockedOutboxMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwBlockedOutboxMessageTypeTableInfo()
		=> _VwBlockedOutboxMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOutboxMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"VwOutboxMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.IdOutboxMessage), typeof(Guid), "\"IdOutboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.IdOutboxMessageStatus), typeof(Guid), "\"IdOutboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.OutboxMessageStatusCode), typeof(string), "\"OutboxMessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.OutboxMessageStatusName), typeof(string), "\"OutboxMessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.IdOutboxQueue), typeof(Guid?), "\"IdOutboxQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.TargetTopic), typeof(string), "\"TargetTopic\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.TargetQueueName), typeof(string), "\"TargetQueueName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOutboxMessageTableInfo()
		=> _VwOutboxMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOutboxMessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"VwOutboxMessageArchive\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.IdOutboxMessage), typeof(Guid), "\"IdOutboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.IdOutboxMessageStatus), typeof(Guid), "\"IdOutboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.OutboxMessageStatusCode), typeof(string), "\"OutboxMessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.OutboxMessageStatusName), typeof(string), "\"OutboxMessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.TargetTopic), typeof(string), "\"TargetTopic\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.TargetQueueName), typeof(string), "\"TargetQueueName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOutboxMessageArchiveTableInfo()
		=> _VwOutboxMessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOutboxMessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"VwOutboxMessageContent\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.IdOutboxMessageContent), typeof(Guid), "\"IdOutboxMessageContent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOutboxMessageContentTableInfo()
		=> _VwOutboxMessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOutboxMessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"VwOutboxMessageProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.IdOutboxMessageProcessingLog), typeof(Guid), "\"IdOutboxMessageProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.IdOutboxMessage), typeof(Guid), "\"IdOutboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.IdOutboxMessageStatus), typeof(Guid), "\"IdOutboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.OutboxMessageStatusCode), typeof(string), "\"OutboxMessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.OutboxMessageStatusName), typeof(string), "\"OutboxMessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOutboxMessageProcessingLogTableInfo()
		=> _VwOutboxMessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOutboxQueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"VwOutboxQueue\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.Name), typeof(string), "\"Name\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.ReceivedEventNamespace), typeof(string), "\"ReceivedEventNamespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.IdMessageType), typeof(Guid?), "\"IdMessageType\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.ProcessingModeCode), typeof(string), "\"ProcessingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.ProcessingModeName), typeof(string), "\"ProcessingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.SuspendingModeCode), typeof(string), "\"SuspendingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.SuspendingModeName), typeof(string), "\"SuspendingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOutboxQueueTableInfo()
		=> _VwOutboxQueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOutboxQueueMessagesTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"VwOutboxQueueMessages\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.OutboxQueueName), typeof(string), "\"OutboxQueueName\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.CreatedMessageCount), typeof(long), "\"CreatedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.ProcessingMessageCount), typeof(long), "\"ProcessingMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.ProcessedMessageCount), typeof(long), "\"ProcessedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.FailedMessageCount), typeof(long), "\"FailedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages.SuspendedMessageCount), typeof(long), "\"SuspendedMessageCount\"", "bigint", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOutboxQueueMessagesTableInfo()
		=> _VwOutboxQueueMessagesTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType), GetVwBlockedOutboxMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage), GetVwOutboxMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive), GetVwOutboxMessageArchiveTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent), GetVwOutboxMessageContentTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog), GetVwOutboxMessageProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue), GetVwOutboxQueueTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages), GetVwOutboxQueueMessagesTableInfo() },
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
