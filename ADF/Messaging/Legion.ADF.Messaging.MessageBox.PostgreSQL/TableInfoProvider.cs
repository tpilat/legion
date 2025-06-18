using Legion.Extensions;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class TableInfoProvider : Legion.ADF.Messaging.MessageBox.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _BlockedMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"BlockedMessageType\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType.IdBlockedMessageType), typeof(Guid), "\"IdBlockedMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetBlockedMessageTypeTableInfo()
		=> _BlockedMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"Message\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.IdMessageStatus), typeof(Guid), "\"IdMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.IdQueue), typeof(Guid?), "\"IdQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.IdTopic), typeof(Guid?), "\"IdTopic\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.ValidToUtc), typeof(DateTime?), "\"ValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.Priority), typeof(int), "\"Priority\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Message.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageTableInfo()
		=> _MessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageArchive\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.IdMessageStatus), typeof(Guid), "\"IdMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.IdQueue), typeof(Guid?), "\"IdQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.IdTopic), typeof(Guid?), "\"IdTopic\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.ValidToUtc), typeof(DateTime?), "\"ValidToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.Priority), typeof(int), "\"Priority\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageArchiveTableInfo()
		=> _MessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageBoxInstanceTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageBoxInstance\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance.Version), typeof(string), "\"Version\"", "varchar(15)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance.MaxDegreeOfQueueParallelism), typeof(int), "\"MaxDegreeOfQueueParallelism\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance.MaxDegreeOfTopicParallelism), typeof(int), "\"MaxDegreeOfTopicParallelism\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageBoxInstanceTableInfo()
		=> _MessageBoxInstanceTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageBoxProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageBoxProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.IdMessageBoxProcessingLog), typeof(Guid), "\"IdMessageBoxProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.IdQueue), typeof(Guid?), "\"IdQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.IdTopic), typeof(Guid?), "\"IdTopic\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.IdTopicSubscription), typeof(Guid?), "\"IdTopicSubscription\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageBoxProcessingLogTableInfo()
		=> _MessageBoxProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageContent\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.IdMessageContent), typeof(Guid), "\"IdMessageContent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageContentTableInfo()
		=> _MessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.IdMessageProcessingLog), typeof(Guid), "\"IdMessageProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.IdQueuedMessage), typeof(Guid?), "\"IdQueuedMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.IdSubscribedMessage), typeof(Guid?), "\"IdSubscribedMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.IdMessageProcessingStatus), typeof(Guid), "\"IdMessageProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageProcessingLogTableInfo()
		=> _MessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageProcessingStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageProcessingStatus\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus.IdMessageProcessingStatus), typeof(Guid), "\"IdMessageProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageProcessingStatusTableInfo()
		=> _MessageProcessingStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageStatus\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageStatus.IdMessageStatus), typeof(Guid), "\"IdMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageStatusTableInfo()
		=> _MessageStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _MessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"MessageType\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType.Name), typeof(string), "\"Name\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetMessageTypeTableInfo()
		=> _MessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _QueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"Queue\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IdQueue), typeof(Guid), "\"IdQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.Name), typeof(string), "\"Name\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.ReceivedEventNamespace), typeof(string), "\"ReceivedEventNamespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IdMessageType), typeof(Guid?), "\"IdMessageType\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Queue.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetQueueTableInfo()
		=> _QueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _QueuedMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"QueuedMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.IdQueuedMessage), typeof(Guid), "\"IdQueuedMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.IdQueue), typeof(Guid), "\"IdQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.IdMessageProcessingStatus), typeof(Guid), "\"IdMessageProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.AssignedUtc), typeof(DateTime), "\"AssignedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetQueuedMessageTableInfo()
		=> _QueuedMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _QueueProcessingModeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"QueueProcessingMode\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode.IdQueueProcessingMode), typeof(Guid), "\"IdQueueProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetQueueProcessingModeTableInfo()
		=> _QueueProcessingModeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _SubscribedMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"SubscribedMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.IdSubscribedMessage), typeof(Guid), "\"IdSubscribedMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.IdTopicSubscription), typeof(Guid), "\"IdTopicSubscription\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.IdMessageProcessingStatus), typeof(Guid), "\"IdMessageProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.AssignedUtc), typeof(DateTime), "\"AssignedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetSubscribedMessageTableInfo()
		=> _SubscribedMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _TopicTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"Topic\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.IdTopic), typeof(Guid), "\"IdTopic\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.Name), typeof(string), "\"Name\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.Topic.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetTopicTableInfo()
		=> _TopicTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _TopicSubscriptionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "\"TopicSubscription\"",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IdTopicSubscription), typeof(Guid), "\"IdTopicSubscription\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IdTopic), typeof(Guid), "\"IdTopic\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.SubscriptionName), typeof(string), "\"SubscriptionName\"", "varchar(511)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.ReceivedEventNamespace), typeof(string), "\"ReceivedEventNamespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IdJob), typeof(Guid?), "\"IdJob\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IdOrchestration), typeof(Guid?), "\"IdOrchestration\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription.IdMessageBoxInstance), typeof(Guid), "\"IdMessageBoxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetTopicSubscriptionTableInfo()
		=> _TopicSubscriptionTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType), GetBlockedMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.Message), GetMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive), GetMessageArchiveTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance), GetMessageBoxInstanceTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog), GetMessageBoxProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageContent), GetMessageContentTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog), GetMessageProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus), GetMessageProcessingStatusTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageStatus), GetMessageStatusTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.MessageType), GetMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.Queue), GetQueueTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage), GetQueuedMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode), GetQueueProcessingModeTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage), GetSubscribedMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.Topic), GetTopicTableInfo() },
			{ typeof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription), GetTopicSubscriptionTableInfo() },
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
