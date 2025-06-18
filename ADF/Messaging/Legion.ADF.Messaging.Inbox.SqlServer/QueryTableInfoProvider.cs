using Legion.Extensions;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.Messaging.Inbox.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwBlockedInboxMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[VwBlockedInboxMessageType]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.IdBlockedInboxMessageType), typeof(Guid), "[IdBlockedInboxMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwBlockedInboxMessageTypeTableInfo()
		=> _VwBlockedInboxMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[VwInboxMessage]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxMessage), typeof(Guid), "[IdInboxMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdMessageType), typeof(Guid), "[IdMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxMessageStatus), typeof(Guid), "[IdInboxMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.InboxMessageStatusCode), typeof(string), "[InboxMessageStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.InboxMessageStatusName), typeof(string), "[InboxMessageStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "[LastProcessingTimeoutUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.RetryCount), typeof(int), "[RetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.TargetTopic), typeof(string), "[TargetTopic]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.TargetQueueName), typeof(string), "[TargetQueueName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessage.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageTableInfo()
		=> _VwInboxMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[VwInboxMessageArchive]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxMessage), typeof(Guid), "[IdInboxMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdMessageType), typeof(Guid), "[IdMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxMessageStatus), typeof(Guid), "[IdInboxMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.InboxMessageStatusCode), typeof(string), "[InboxMessageStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.InboxMessageStatusName), typeof(string), "[InboxMessageStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.LastProcessingTimeoutUtc), typeof(DateTime?), "[LastProcessingTimeoutUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.RetryCount), typeof(int), "[RetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.TargetTopic), typeof(string), "[TargetTopic]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.TargetQueueName), typeof(string), "[TargetQueueName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageArchiveTableInfo()
		=> _VwInboxMessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[VwInboxMessageContent]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.IdInboxMessageContent), typeof(Guid), "[IdInboxMessageContent]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.MimeType), typeof(string), "[MimeType]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.Name), typeof(string), "[Name]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.IsCompressed), typeof(bool), "[IsCompressed]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageContentTableInfo()
		=> _VwInboxMessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxMessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[VwInboxMessageProcessingLog]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxMessageProcessingLog), typeof(Guid), "[IdInboxMessageProcessingLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxMessage), typeof(Guid), "[IdInboxMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxMessageStatus), typeof(Guid), "[IdInboxMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.InboxMessageStatusCode), typeof(string), "[InboxMessageStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.InboxMessageStatusName), typeof(string), "[InboxMessageStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxMessageProcessingLogTableInfo()
		=> _VwInboxMessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxQueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[VwInboxQueue]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.Name), typeof(string), "[Name]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.ReceivedEventNamespace), typeof(string), "[ReceivedEventNamespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdMessageType), typeof(Guid?), "[IdMessageType]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IsActive), typeof(bool), "[IsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IsSequentialFIFO), typeof(bool), "[IsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessagesBatchCount), typeof(int), "[MessagesBatchCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MaxDegreeOfParallelism), typeof(int?), "[MaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.TimeoutForMessageProcessing), typeof(TimeSpan), "[TimeoutForMessageProcessing]", "time", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MaxMessageProcessingRetryCount), typeof(int), "[MaxMessageProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdProcessingMode), typeof(Guid), "[IdProcessingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.ProcessingModeCode), typeof(string), "[ProcessingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.ProcessingModeName), typeof(string), "[ProcessingModeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.IdSuspendingMode), typeof(Guid), "[IdSuspendingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.SuspendingModeCode), typeof(string), "[SuspendingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.SuspendingModeName), typeof(string), "[SuspendingModeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueue.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwInboxQueueTableInfo()
		=> _VwInboxQueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwInboxQueueMessagesTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[VwInboxQueueMessages]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.InboxQueueName), typeof(string), "[InboxQueueName]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.IsActive), typeof(bool), "[IsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.IsSequentialFIFO), typeof(bool), "[IsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.MaxDegreeOfParallelism), typeof(int?), "[MaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.CreatedMessageCount), typeof(long), "[CreatedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.ProcessingMessageCount), typeof(long), "[ProcessingMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.ProcessedMessageCount), typeof(long), "[ProcessedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.FailedMessageCount), typeof(long), "[FailedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages.SuspendedMessageCount), typeof(long), "[SuspendedMessageCount]", "bigint", false),
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
