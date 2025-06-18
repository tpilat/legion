using Legion.Extensions;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public class TableInfoProvider : Legion.ADF.Messaging.Outbox.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _BlockedOutboxMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"BlockedOutboxMessageType\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType.IdBlockedOutboxMessageType), typeof(Guid), "\"IdBlockedOutboxMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetBlockedOutboxMessageTypeTableInfo()
		=> _BlockedOutboxMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxInstanceTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxInstance\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance.Version), typeof(string), "\"Version\"", "varchar(15)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance.MaxDegreeOfQueueParallelism), typeof(int), "\"MaxDegreeOfQueueParallelism\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxInstanceTableInfo()
		=> _OutboxInstanceTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxMessage\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.IdOutboxMessage), typeof(Guid), "\"IdOutboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.IdOutboxMessageStatus), typeof(Guid), "\"IdOutboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.TargetTopic), typeof(string), "\"TargetTopic\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.TargetQueueName), typeof(string), "\"TargetQueueName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxMessageTableInfo()
		=> _OutboxMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxMessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxMessageArchive\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.IdOutboxMessage), typeof(Guid), "\"IdOutboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.IdMessageType), typeof(Guid), "\"IdMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.IdOutboxMessageStatus), typeof(Guid), "\"IdOutboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.IdMessageContent), typeof(Guid?), "\"IdMessageContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.MessageId), typeof(string), "\"MessageId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.BusinessId), typeof(string), "\"BusinessId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.CorrelationId), typeof(string), "\"CorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.SessionId), typeof(Guid?), "\"SessionId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.SessionMessagePartId), typeof(long?), "\"SessionMessagePartId\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.TargetTopic), typeof(string), "\"TargetTopic\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.TargetQueueName), typeof(string), "\"TargetQueueName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxMessageArchiveTableInfo()
		=> _OutboxMessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxMessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxMessageContent\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.IdOutboxMessageContent), typeof(Guid), "\"IdOutboxMessageContent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxMessageContentTableInfo()
		=> _OutboxMessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxMessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxMessageProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.IdOutboxMessageProcessingLog), typeof(Guid), "\"IdOutboxMessageProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.IdOutboxMessage), typeof(Guid), "\"IdOutboxMessage\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.IdOutboxMessageStatus), typeof(Guid), "\"IdOutboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxMessageProcessingLogTableInfo()
		=> _OutboxMessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxMessageStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxMessageStatus\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus.IdOutboxMessageStatus), typeof(Guid), "\"IdOutboxMessageStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxMessageStatusTableInfo()
		=> _OutboxMessageStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxMessageType\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType.IdOutboxMessageType), typeof(Guid), "\"IdOutboxMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType.Name), typeof(string), "\"Name\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxMessageTypeTableInfo()
		=> _OutboxMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.IdOutboxProcessingLog), typeof(Guid), "\"IdOutboxProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.IdOutboxQueue), typeof(Guid?), "\"IdOutboxQueue\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxProcessingLogTableInfo()
		=> _OutboxProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxQueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxQueue\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.IdOutboxQueue), typeof(Guid), "\"IdOutboxQueue\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.Name), typeof(string), "\"Name\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.ReceivedEventNamespace), typeof(string), "\"ReceivedEventNamespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.IdMessageType), typeof(Guid?), "\"IdMessageType\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.IsActive), typeof(bool), "\"IsActive\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.IsSequentialFIFO), typeof(bool), "\"IsSequentialFIFO\"", "boolean", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.MessagesBatchCount), typeof(int), "\"MessagesBatchCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.MaxDegreeOfParallelism), typeof(int?), "\"MaxDegreeOfParallelism\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.TimeoutForMessageProcessing), typeof(TimeSpan), "\"TimeoutForMessageProcessing\"", "interval", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.IdProcessingMode), typeof(Guid), "\"IdProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.IdSuspendingMode), typeof(Guid), "\"IdSuspendingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue.IdOutboxInstance), typeof(Guid), "\"IdOutboxInstance\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxQueueTableInfo()
		=> _OutboxQueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OutboxQueueProcessingModeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"outbox", "\"OutboxQueueProcessingMode\"",
				[
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode.IdOutboxQueueProcessingMode), typeof(Guid), "\"IdOutboxQueueProcessingMode\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOutboxQueueProcessingModeTableInfo()
		=> _OutboxQueueProcessingModeTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType), GetBlockedOutboxMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance), GetOutboxInstanceTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage), GetOutboxMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive), GetOutboxMessageArchiveTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent), GetOutboxMessageContentTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog), GetOutboxMessageProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus), GetOutboxMessageStatusTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType), GetOutboxMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog), GetOutboxProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue), GetOutboxQueueTableInfo() },
			{ typeof(Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode), GetOutboxQueueProcessingModeTableInfo() },
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
