using Legion.Extensions;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.Messaging.MessageBox.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwBlockedMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwBlockedMessageType]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.IdBlockedMessageType), typeof(Guid), "[IdBlockedMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType.IdMessageBoxInstance), typeof(Guid), "[IdMessageBoxInstance]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwBlockedMessageTypeTableInfo()
		=> _VwBlockedMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwMessage]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessage), typeof(Guid), "[IdMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessageType), typeof(Guid), "[IdMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessageStatus), typeof(Guid), "[IdMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageStatusCode), typeof(string), "[MessageStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageStatusName), typeof(string), "[MessageStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdQueue), typeof(Guid?), "[IdQueue]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.IdTopic), typeof(Guid?), "[IdTopic]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.ValidToUtc), typeof(DateTime?), "[ValidToUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.Priority), typeof(int), "[Priority]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessage.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageTableInfo()
		=> _VwMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageArchiveTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwMessageArchive]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessage), typeof(Guid), "[IdMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessageType), typeof(Guid), "[IdMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessageStatus), typeof(Guid), "[IdMessageStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageStatusCode), typeof(string), "[MessageStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageStatusName), typeof(string), "[MessageStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdQueue), typeof(Guid?), "[IdQueue]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.IdTopic), typeof(Guid?), "[IdTopic]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.ValidToUtc), typeof(DateTime?), "[ValidToUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.Priority), typeof(int), "[Priority]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageArchiveTableInfo()
		=> _VwMessageArchiveTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwMessageContent]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.IdMessageContent), typeof(Guid), "[IdMessageContent]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.MimeType), typeof(string), "[MimeType]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.Name), typeof(string), "[Name]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.IsCompressed), typeof(bool), "[IsCompressed]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageContent.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageContentTableInfo()
		=> _VwMessageContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwMessageProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwMessageProcessingLog]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdMessageProcessingLog), typeof(Guid), "[IdMessageProcessingLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdMessage), typeof(Guid), "[IdMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdQueuedMessage), typeof(Guid?), "[IdQueuedMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdSubscribedMessage), typeof(Guid?), "[IdSubscribedMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdMessageProcessingStatus), typeof(Guid), "[IdMessageProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.MessageProcessingStatusCode), typeof(string), "[MessageProcessingStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.MessageProcessingStatusName), typeof(string), "[MessageProcessingStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwMessageProcessingLogTableInfo()
		=> _VwMessageProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwQueueTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwQueue]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdQueue), typeof(Guid), "[IdQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.Name), typeof(string), "[Name]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdMessageType), typeof(Guid?), "[IdMessageType]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IsActive), typeof(bool), "[IsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IsSequentialFIFO), typeof(bool), "[IsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessagesBatchCount), typeof(int), "[MessagesBatchCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MaxDegreeOfParallelism), typeof(int?), "[MaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.TimeoutForMessageProcessing), typeof(TimeSpan), "[TimeoutForMessageProcessing]", "time", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MaxMessageProcessingRetryCount), typeof(int), "[MaxMessageProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdProcessingMode), typeof(Guid), "[IdProcessingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.ProcessingModeCode), typeof(string), "[ProcessingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.ProcessingModeName), typeof(string), "[ProcessingModeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdSuspendingMode), typeof(Guid), "[IdSuspendingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.SuspendingModeCode), typeof(string), "[SuspendingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.SuspendingModeName), typeof(string), "[SuspendingModeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdJob), typeof(Guid?), "[IdJob]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueue.IdOrchestration), typeof(Guid?), "[IdOrchestration]", "uniqueidentifier", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwQueueTableInfo()
		=> _VwQueueTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwQueuedMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwQueuedMessage]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdQueuedMessage), typeof(Guid), "[IdQueuedMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdQueue), typeof(Guid), "[IdQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessage), typeof(Guid), "[IdMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageProcessingStatus), typeof(Guid), "[IdMessageProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageProcessingStatusCode), typeof(string), "[MessageProcessingStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageProcessingStatusName), typeof(string), "[MessageProcessingStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.AssignedUtc), typeof(DateTime), "[AssignedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "[LastProcessingTimeoutUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.RetryCount), typeof(int), "[RetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdJob), typeof(Guid?), "[IdJob]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdOrchestration), typeof(Guid?), "[IdOrchestration]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IsArchived), typeof(int), "[IsArchived]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageType), typeof(Guid?), "[IdMessageType]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageStatus), typeof(Guid?), "[IdMessageStatus]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageStatusCode), typeof(string), "[MessageStatusCode]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageStatusName), typeof(string), "[MessageStatusName]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdQueueMessage), typeof(Guid?), "[IdQueueMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.IdTopicMessage), typeof(Guid?), "[IdTopicMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.CreatedUtc), typeof(DateTime?), "[CreatedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.TraceCorrelationId), typeof(Guid?), "[TraceCorrelationId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.ValidToUtc), typeof(DateTime?), "[ValidToUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.Priority), typeof(int?), "[Priority]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwQueuedMessageTableInfo()
		=> _VwQueuedMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwQueueMessagesTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwQueueMessages]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IdQueue), typeof(Guid), "[IdQueue]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.QueueName), typeof(string), "[QueueName]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IsActive), typeof(bool), "[IsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IsSequentialFIFO), typeof(bool), "[IsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.MaxDegreeOfParallelism), typeof(int?), "[MaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IdJob), typeof(Guid?), "[IdJob]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.IdOrchestration), typeof(Guid?), "[IdOrchestration]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.AssignedMessageCount), typeof(long), "[AssignedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.ProcessingMessageCount), typeof(long), "[ProcessingMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.ProcessedMessageCount), typeof(long), "[ProcessedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.FailedMessageCount), typeof(long), "[FailedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages.SuspendedMessageCount), typeof(long), "[SuspendedMessageCount]", "bigint", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwQueueMessagesTableInfo()
		=> _VwQueueMessagesTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwSubscribedMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwSubscribedMessage]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdSubscribedMessage), typeof(Guid), "[IdSubscribedMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdTopicSubscription), typeof(Guid), "[IdTopicSubscription]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdTopic), typeof(Guid), "[IdTopic]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessage), typeof(Guid), "[IdMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageProcessingStatus), typeof(Guid), "[IdMessageProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageProcessingStatusCode), typeof(string), "[MessageProcessingStatusCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageProcessingStatusName), typeof(string), "[MessageProcessingStatusName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.AssignedUtc), typeof(DateTime), "[AssignedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.LastProcessingTimeoutUtc), typeof(DateTime?), "[LastProcessingTimeoutUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.RetryCount), typeof(int), "[RetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdJob), typeof(Guid?), "[IdJob]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdOrchestration), typeof(Guid?), "[IdOrchestration]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IsArchived), typeof(int), "[IsArchived]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageType), typeof(Guid?), "[IdMessageType]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageStatus), typeof(Guid?), "[IdMessageStatus]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageStatusCode), typeof(string), "[MessageStatusCode]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageStatusName), typeof(string), "[MessageStatusName]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdMessageContent), typeof(Guid?), "[IdMessageContent]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdQueueMessage), typeof(Guid?), "[IdQueueMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.IdTopicMessage), typeof(Guid?), "[IdTopicMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.CreatedUtc), typeof(DateTime?), "[CreatedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageId), typeof(string), "[MessageId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.BusinessId), typeof(string), "[BusinessId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.CorrelationId), typeof(string), "[CorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.SessionId), typeof(Guid?), "[SessionId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.SessionMessagePartId), typeof(long?), "[SessionMessagePartId]", "bigint", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.TraceCorrelationId), typeof(Guid?), "[TraceCorrelationId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.ValidToUtc), typeof(DateTime?), "[ValidToUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.Priority), typeof(int?), "[Priority]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageTypeCode), typeof(string), "[MessageTypeCode]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageTypeName), typeof(string), "[MessageTypeName]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage.MessageTypeNamespace), typeof(string), "[MessageTypeNamespace]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwSubscribedMessageTableInfo()
		=> _VwSubscribedMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwTopicTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwTopic]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IdTopic), typeof(Guid), "[IdTopic]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.Name), typeof(string), "[Name]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IsActive), typeof(bool), "[IsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IsSequentialFIFO), typeof(bool), "[IsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.MessagesBatchCount), typeof(int), "[MessagesBatchCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.MaxDegreeOfParallelism), typeof(int?), "[MaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.TimeoutForMessageProcessing), typeof(TimeSpan), "[TimeoutForMessageProcessing]", "time", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.MaxMessageProcessingRetryCount), typeof(int), "[MaxMessageProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IdProcessingMode), typeof(Guid), "[IdProcessingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.ProcessingModeCode), typeof(string), "[ProcessingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.ProcessingModeName), typeof(string), "[ProcessingModeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.IdSuspendingMode), typeof(Guid), "[IdSuspendingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.SuspendingModeCode), typeof(string), "[SuspendingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopic.SuspendingModeName), typeof(string), "[SuspendingModeName]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwTopicTableInfo()
		=> _VwTopicTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwTopicSubscriptionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwTopicSubscription]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdTopicSubscription), typeof(Guid), "[IdTopicSubscription]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdTopic), typeof(Guid), "[IdTopic]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.SubscriptionName), typeof(string), "[SubscriptionName]", "nvarchar(511)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IsActive), typeof(bool), "[IsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IsSequentialFIFO), typeof(bool), "[IsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.MessagesBatchCount), typeof(int), "[MessagesBatchCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.MaxDegreeOfParallelism), typeof(int?), "[MaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.TimeoutForMessageProcessing), typeof(TimeSpan), "[TimeoutForMessageProcessing]", "time", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.MaxMessageProcessingRetryCount), typeof(int), "[MaxMessageProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdProcessingMode), typeof(Guid), "[IdProcessingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.ProcessingModeCode), typeof(string), "[ProcessingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.ProcessingModeName), typeof(string), "[ProcessingModeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdSuspendingMode), typeof(Guid), "[IdSuspendingMode]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.SuspendingModeCode), typeof(string), "[SuspendingModeCode]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.SuspendingModeName), typeof(string), "[SuspendingModeName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdJob), typeof(Guid?), "[IdJob]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription.IdOrchestration), typeof(Guid?), "[IdOrchestration]", "uniqueidentifier", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwTopicSubscriptionTableInfo()
		=> _VwTopicSubscriptionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwTopicSubscriptionMessagesTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"mbox", "[VwTopicSubscriptionMessages]",
				[
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdTopicSubscription), typeof(Guid), "[IdTopicSubscription]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionName), typeof(string), "[SubscriptionName]", "nvarchar(511)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionIsActive), typeof(bool), "[SubscriptionIsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionIsSequentialFIFO), typeof(bool), "[SubscriptionIsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionMessagesBatchCount), typeof(int), "[SubscriptionMessagesBatchCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionMaxDegreeOfParallelism), typeof(int?), "[SubscriptionMaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionTimeoutForMessageProcessing), typeof(TimeSpan), "[SubscriptionTimeoutForMessageProcessing]", "time", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SubscriptionMaxMessageProcessingRetryCount), typeof(int), "[SubscriptionMaxMessageProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdJob), typeof(Guid?), "[IdJob]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdOrchestration), typeof(Guid?), "[IdOrchestration]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.IdTopic), typeof(Guid), "[IdTopic]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicName), typeof(string), "[TopicName]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopisIsActive), typeof(bool), "[TopisIsActive]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicIsSequentialFIFO), typeof(bool), "[TopicIsSequentialFIFO]", "bit", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicMessagesBatchCount), typeof(int), "[TopicMessagesBatchCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicMaxDegreeOfParallelism), typeof(int?), "[TopicMaxDegreeOfParallelism]", "int", true),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicTimeoutForMessageProcessing), typeof(TimeSpan), "[TopicTimeoutForMessageProcessing]", "time", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.TopicMaxMessageProcessingRetryCount), typeof(int), "[TopicMaxMessageProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.AssignedMessageCount), typeof(long), "[AssignedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.ProcessingMessageCount), typeof(long), "[ProcessingMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.ProcessedMessageCount), typeof(long), "[ProcessedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.FailedMessageCount), typeof(long), "[FailedMessageCount]", "bigint", false),
					new(nameof(Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages.SuspendedMessageCount), typeof(long), "[SuspendedMessageCount]", "bigint", false),
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
