using Legion.Extensions;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.Messaging.MessageBox.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwBlockedMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwBlockedMessageType\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.IdBlockedMessageType), typeof(Guid), "\"IdBlockedMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwBlockedMessageTypeTableInfo()
		=> _VwBlockedMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessageStatus), typeof(Guid), "\"IdMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageStatusCode), typeof(string), "\"MessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageStatusName), typeof(string), "\"MessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdQueue), typeof(Guid?), "\"IdQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdTopic), typeof(Guid?), "\"IdTopic\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.ValidToUtc), typeof(DateTime?), "\"ValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.Priority), typeof(int), "\"Priority\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageTableInfo()
		=> _VwMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwMessageArchive\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessageStatus), typeof(Guid), "\"IdMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageStatusCode), typeof(string), "\"MessageStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageStatusName), typeof(string), "\"MessageStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdQueue), typeof(Guid?), "\"IdQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdTopic), typeof(Guid?), "\"IdTopic\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.ValidToUtc), typeof(DateTime?), "\"ValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.Priority), typeof(int), "\"Priority\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageArchiveTableInfo()
		=> _VwMessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwMessageContent\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.IdMessageContent), typeof(Guid), "\"IdMessageContent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageContentTableInfo()
		=> _VwMessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwMessageProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdMessageProcessingLog), typeof(Guid), "\"IdMessageProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdQueuedMessage), typeof(Guid?), "\"IdQueuedMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdSubscribedMessage), typeof(Guid?), "\"IdSubscribedMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdMessageProcessingStatus), typeof(Guid), "\"IdMessageProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.MessageProcessingStatusCode), typeof(string), "\"MessageProcessingStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.MessageProcessingStatusName), typeof(string), "\"MessageProcessingStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageProcessingLogTableInfo()
		=> _VwMessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwQueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwQueue\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdQueue), typeof(Guid), "\"IdQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.Name), typeof(string), "\"Name\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdMessageType), typeof(Guid?), "\"IdMessageType\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.ProcessingModeCode), typeof(string), "\"ProcessingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.ProcessingModeName), typeof(string), "\"ProcessingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.SuspendingModeCode), typeof(string), "\"SuspendingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.SuspendingModeName), typeof(string), "\"SuspendingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwQueueTableInfo()
		=> _VwQueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwQueuedMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwQueuedMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdQueuedMessage), typeof(Guid), "\"IdQueuedMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdQueue), typeof(Guid), "\"IdQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageProcessingStatus), typeof(Guid), "\"IdMessageProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageProcessingStatusCode), typeof(string), "\"MessageProcessingStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageProcessingStatusName), typeof(string), "\"MessageProcessingStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.AssignedUtc), typeof(DateTime), "\"AssignedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IsArchived), typeof(int), "\"IsArchived\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageType), typeof(Guid?), "\"IdMessageType\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageStatus), typeof(Guid?), "\"IdMessageStatus\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageStatusCode), typeof(string), "\"MessageStatusCode\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageStatusName), typeof(string), "\"MessageStatusName\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdQueueMessage), typeof(Guid?), "\"IdQueueMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdTopicMessage), typeof(Guid?), "\"IdTopicMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.CreatedUtc), typeof(DateTime?), "\"CreatedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.TraceCorrelationId), typeof(Guid?), "\"TraceCorrelationId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.ValidToUtc), typeof(DateTime?), "\"ValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.Priority), typeof(int?), "\"Priority\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwQueuedMessageTableInfo()
		=> _VwQueuedMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwQueueMessagesTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwQueueMessages\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IdQueue), typeof(Guid), "\"IdQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.QueueName), typeof(string), "\"QueueName\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.AssignedMessageCount), typeof(long), "\"AssignedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.ProcessingMessageCount), typeof(long), "\"ProcessingMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.ProcessedMessageCount), typeof(long), "\"ProcessedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.FailedMessageCount), typeof(long), "\"FailedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.SuspendedMessageCount), typeof(long), "\"SuspendedMessageCount\"", "bigint", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwQueueMessagesTableInfo()
		=> _VwQueueMessagesTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwSubscribedMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwSubscribedMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdSubscribedMessage), typeof(Guid), "\"IdSubscribedMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdTopicSubscription), typeof(Guid), "\"IdTopicSubscription\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdTopic), typeof(Guid), "\"IdTopic\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageProcessingStatus), typeof(Guid), "\"IdMessageProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageProcessingStatusCode), typeof(string), "\"MessageProcessingStatusCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageProcessingStatusName), typeof(string), "\"MessageProcessingStatusName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.AssignedUtc), typeof(DateTime), "\"AssignedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IsArchived), typeof(int), "\"IsArchived\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageType), typeof(Guid?), "\"IdMessageType\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageStatus), typeof(Guid?), "\"IdMessageStatus\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageStatusCode), typeof(string), "\"MessageStatusCode\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageStatusName), typeof(string), "\"MessageStatusName\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdQueueMessage), typeof(Guid?), "\"IdQueueMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdTopicMessage), typeof(Guid?), "\"IdTopicMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.CreatedUtc), typeof(DateTime?), "\"CreatedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.TraceCorrelationId), typeof(Guid?), "\"TraceCorrelationId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.ValidToUtc), typeof(DateTime?), "\"ValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.Priority), typeof(int?), "\"Priority\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageTypeCode), typeof(string), "\"MessageTypeCode\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageTypeName), typeof(string), "\"MessageTypeName\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageTypeNamespace), typeof(string), "\"MessageTypeNamespace\"", "varchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwSubscribedMessageTableInfo()
		=> _VwSubscribedMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwTopicTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwTopic\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IdTopic), typeof(Guid), "\"IdTopic\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.Name), typeof(string), "\"Name\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.ProcessingModeCode), typeof(string), "\"ProcessingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.ProcessingModeName), typeof(string), "\"ProcessingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.SuspendingModeCode), typeof(string), "\"SuspendingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.SuspendingModeName), typeof(string), "\"SuspendingModeName\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwTopicTableInfo()
		=> _VwTopicTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwTopicSubscriptionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwTopicSubscription\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdTopicSubscription), typeof(Guid), "\"IdTopicSubscription\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdTopic), typeof(Guid), "\"IdTopic\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.SubscriptionName), typeof(string), "\"SubscriptionName\"", "varchar(511)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.ProcessingModeCode), typeof(string), "\"ProcessingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.ProcessingModeName), typeof(string), "\"ProcessingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.SuspendingModeCode), typeof(string), "\"SuspendingModeCode\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.SuspendingModeName), typeof(string), "\"SuspendingModeName\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwTopicSubscriptionTableInfo()
		=> _VwTopicSubscriptionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwTopicSubscriptionMessagesTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"VwTopicSubscriptionMessages\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdTopicSubscription), typeof(Guid), "\"IdTopicSubscription\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionName), typeof(string), "\"SubscriptionName\"", "varchar(511)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionIsActive), typeof(bool), "\"SubscriptionIsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionIsSequentialFIFO), typeof(bool), "\"SubscriptionIsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionMessagesBatchCount), typeof(int), "\"SubscriptionMessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionMaxDegreeOfParallelism), typeof(int?), "\"SubscriptionMaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionTimeoutForMessageProcessing), typeof(TimeSpan), "\"SubscriptionTimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionMaxMessageProcessingRetryCount), typeof(int), "\"SubscriptionMaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdTopic), typeof(Guid), "\"IdTopic\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicName), typeof(string), "\"TopicName\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopisIsActive), typeof(bool), "\"TopisIsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicIsSequentialFIFO), typeof(bool), "\"TopicIsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicMessagesBatchCount), typeof(int), "\"TopicMessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicMaxDegreeOfParallelism), typeof(int?), "\"TopicMaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicTimeoutForMessageProcessing), typeof(TimeSpan), "\"TopicTimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicMaxMessageProcessingRetryCount), typeof(int), "\"TopicMaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.AssignedMessageCount), typeof(long), "\"AssignedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.ProcessingMessageCount), typeof(long), "\"ProcessingMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.ProcessedMessageCount), typeof(long), "\"ProcessedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.FailedMessageCount), typeof(long), "\"FailedMessageCount\"", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SuspendedMessageCount), typeof(long), "\"SuspendedMessageCount\"", "bigint", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwTopicSubscriptionMessagesTableInfo()
		=> _VwTopicSubscriptionMessagesTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType), GetVwBlockedMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwMessage), GetVwMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive), GetVwMessageArchiveTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent), GetVwMessageContentTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog), GetVwMessageProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwQueue), GetVwQueueTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage), GetVwQueuedMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages), GetVwQueueMessagesTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage), GetVwSubscribedMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwTopic), GetVwTopicTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription), GetVwTopicSubscriptionTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages), GetVwTopicSubscriptionMessagesTableInfo() },
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
