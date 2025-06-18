using Legion.Extensions;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class TableInfoProvider : Legion.ADF.Messaging.Inbox.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _BlockedInboxMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[BlockedInboxMessageType]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType.IdBlockedInboxMessageType), typeof(Guid), "[IdBlockedInboxMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetBlockedInboxMessageTypeTableInfo()
		=> _BlockedInboxMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxInstanceTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxInstance]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance.Version), typeof(string), "[Version]", "nvarchar(15)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance.MaxDegreeOfQueueParallelism), typeof(int), "[MaxDegreeOfQueueParallelism]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxInstanceTableInfo()
		=> _InboxInstanceTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxMessage]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.IdInboxMessage), typeof(Guid), "[IdInboxMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.IdMessageType), typeof(Guid), "[IdMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.IdInboxMessageStatus), typeof(Guid), "[IdInboxMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "[LastProcessingTimeoutUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.RetryCount), typeof(int), "[RetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.TargetTopic), typeof(string), "[TargetTopic]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.TargetQueueName), typeof(string), "[TargetQueueName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxMessageTableInfo()
		=> _InboxMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxMessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxMessageArchive]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.IdInboxMessage), typeof(Guid), "[IdInboxMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.IdMessageType), typeof(Guid), "[IdMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.IdInboxMessageStatus), typeof(Guid), "[IdInboxMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.LastProcessingTimeoutUtc), typeof(DateTime?), "[LastProcessingTimeoutUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.RetryCount), typeof(int), "[RetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.TargetTopic), typeof(string), "[TargetTopic]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.TargetQueueName), typeof(string), "[TargetQueueName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxMessageArchiveTableInfo()
		=> _InboxMessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxMessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxMessageContent]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.IdInboxMessageContent), typeof(Guid), "[IdInboxMessageContent]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.MimeType), typeof(string), "[MimeType]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.Name), typeof(string), "[Name]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.IsCompressed), typeof(bool), "[IsCompressed]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxMessageContentTableInfo()
		=> _InboxMessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxMessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxMessageProcessingLog]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.IdInboxMessageProcessingLog), typeof(Guid), "[IdInboxMessageProcessingLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.IdInboxMessage), typeof(Guid), "[IdInboxMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.IdInboxMessageStatus), typeof(Guid), "[IdInboxMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxMessageProcessingLogTableInfo()
		=> _InboxMessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxMessageStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxMessageStatus]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus.IdInboxMessageStatus), typeof(Guid), "[IdInboxMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus.Name), typeof(string), "[Name]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxMessageStatusTableInfo()
		=> _InboxMessageStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxMessageType]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType.IdInboxMessageType), typeof(Guid), "[IdInboxMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType.Name), typeof(string), "[Name]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxMessageTypeTableInfo()
		=> _InboxMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxProcessingLog]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.IdInboxProcessingLog), typeof(Guid), "[IdInboxProcessingLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.IdInboxQueue), typeof(Guid?), "[IdInboxQueue]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxProcessingLogTableInfo()
		=> _InboxProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxQueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxQueue]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.IdInboxQueue), typeof(Guid), "[IdInboxQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.Name), typeof(string), "[Name]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.ReceivedEventNamespace), typeof(string), "[ReceivedEventNamespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.IdMessageType), typeof(Guid?), "[IdMessageType]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.IsActive), typeof(bool), "[IsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.MessagesBatchCount), typeof(int), "[MessagesBatchCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.IsSequentialFIFO), typeof(bool), "[IsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.MaxDegreeOfParallelism), typeof(int?), "[MaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.TimeoutForMessageProcessing), typeof(TimeSpan), "[TimeoutForMessageProcessing]", "time", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.MaxMessageProcessingRetryCount), typeof(int), "[MaxMessageProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.IdProcessingMode), typeof(Guid), "[IdProcessingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.IdSuspendingMode), typeof(Guid), "[IdSuspendingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue.IdInboxInstance), typeof(Guid), "[IdInboxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxQueueTableInfo()
		=> _InboxQueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _InboxQueueProcessingModeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"inbox", "[InboxQueueProcessingMode]",
				[
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode.IdInboxQueueProcessingMode), typeof(Guid), "[IdInboxQueueProcessingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode.Name), typeof(string), "[Name]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetInboxQueueProcessingModeTableInfo()
		=> _InboxQueueProcessingModeTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType), GetBlockedInboxMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxInstance), GetInboxInstanceTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxMessage), GetInboxMessageTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive), GetInboxMessageArchiveTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent), GetInboxMessageContentTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog), GetInboxMessageProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus), GetInboxMessageStatusTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType), GetInboxMessageTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog), GetInboxProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxQueue), GetInboxQueueTableInfo() },
			{ typeof(Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode), GetInboxQueueProcessingModeTableInfo() },
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
