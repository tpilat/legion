CREATE TABLE [devt].[BlockedDomainEventType]
(
	[IdBlockedDomainEventType] uniqueidentifier NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL
)
GO

CREATE TABLE [inbox].[BlockedInboxMessageType]
(
	[IdBlockedInboxMessageType] uniqueidentifier NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdInboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[BlockedMessageType]
(
	[IdBlockedMessageType] uniqueidentifier NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [outbox].[BlockedOutboxMessageType]
(
	[IdBlockedOutboxMessageType] uniqueidentifier NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdOutboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [devt].[DomainEvent]
(
	[IdDomainEvent] uniqueidentifier NOT NULL,
	[IdContent] uniqueidentifier NOT NULL,
	[IdDomainEventProcessingStatus] uniqueidentifier NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[Properties] nvarchar(max) NULL,
	[Publisher] nvarchar(511) NULL,
	[PublisherId] nvarchar(511) NULL,
	[CreatedUtc] datetime2 NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[LastProcessingTimeoutUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL,
	[Priority] int NOT NULL
)
GO

CREATE TABLE [devt].[DomainEventContent]
(
	[IdDomainEventContent] uniqueidentifier NOT NULL,
	[Content] nvarchar(max) NOT NULL
)
GO

CREATE TABLE [devt].[DomainEventProcessingLog]
(
	[IdDomainEventProcessingLog] uniqueidentifier NOT NULL,
	[IdDomainEvent] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdDomainEventProcessingStatus] uniqueidentifier NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL
)
GO

CREATE TABLE [devt].[DomainEventProcessingStatus]
(
	[IdDomainEventProcessingStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [inbox].[InboxInstance]
(
	[IdInboxInstance] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Version] nvarchar(15) NOT NULL,
	[MaxDegreeOfQueueParallelism] int NOT NULL,
	[IdLogLevel] int NOT NULL
)
GO

CREATE TABLE [inbox].[InboxMessage]
(
	[IdInboxMessage] uniqueidentifier NOT NULL,
	[IdMessageType] uniqueidentifier NOT NULL,
	[IdInboxMessageStatus] uniqueidentifier NOT NULL,
	[IdMessageContent] uniqueidentifier NULL,
	[IdInboxQueue] uniqueidentifier NOT NULL,
	[MessageId] nvarchar(511) NULL,
	[BusinessId] nvarchar(511) NULL,
	[CorrelationId] nvarchar(511) NULL,
	[SessionId] uniqueidentifier NULL,
	[SessionMessagePartId] bigint NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[Properties] nvarchar(max) NULL,
	[Publisher] nvarchar(511) NULL,
	[PublisherId] nvarchar(511) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[LastProcessingTimeoutUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL,
	[TargetTopic] nvarchar(1023) NULL,
	[TargetQueueName] nvarchar(1023) NULL,
	[IdInboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [inbox].[InboxMessageArchive]
(
	[IdInboxMessage] uniqueidentifier NOT NULL,
	[IdMessageType] uniqueidentifier NOT NULL,
	[IdInboxMessageStatus] uniqueidentifier NOT NULL,
	[IdMessageContent] uniqueidentifier NULL,
	[IdInboxQueue] uniqueidentifier NOT NULL,
	[MessageId] nvarchar(511) NULL,
	[BusinessId] nvarchar(511) NULL,
	[CorrelationId] nvarchar(511) NULL,
	[SessionId] uniqueidentifier NULL,
	[SessionMessagePartId] bigint NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[Properties] nvarchar(max) NULL,
	[Publisher] nvarchar(511) NULL,
	[PublisherId] nvarchar(511) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[LastProcessingTimeoutUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL,
	[TargetTopic] nvarchar(1023) NULL,
	[TargetQueueName] nvarchar(1023) NULL,
	[IdInboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [inbox].[InboxMessageContent]
(
	[IdInboxMessageContent] uniqueidentifier NOT NULL,
	[MimeType] nvarchar(1023) NOT NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[Name] nvarchar(511) NULL,
	[RelativePath] nvarchar(1023) NULL,
	[Metadata] nvarchar(max) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL
)
GO

CREATE TABLE [inbox].[InboxMessageProcessingLog]
(
	[IdInboxMessageProcessingLog] uniqueidentifier NOT NULL,
	[IdInboxMessage] uniqueidentifier NOT NULL,
	[IdInboxQueue] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdInboxMessageStatus] uniqueidentifier NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL,
	[IdInboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [inbox].[InboxMessageStatus]
(
	[IdInboxMessageStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [inbox].[InboxMessageType]
(
	[IdInboxMessageType] uniqueidentifier NOT NULL,
	[Code] nvarchar(127) NOT NULL,
	[Name] nvarchar(127) NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdInboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [inbox].[InboxProcessingLog]
(
	[IdInboxProcessingLog] uniqueidentifier NOT NULL,
	[IdInboxInstance] uniqueidentifier NOT NULL,
	[IdInboxQueue] uniqueidentifier NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdLogLevel] int NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL
)
GO

CREATE TABLE [inbox].[InboxQueue]
(
	[IdInboxQueue] uniqueidentifier NOT NULL,
	[Name] nvarchar(1023) NOT NULL,
	[ReceivedEventNamespace] nvarchar(1023) NOT NULL,
	[IdMessageType] uniqueidentifier NULL,
	[IsActive] bit NOT NULL,
	[MessagesBatchCount] int NOT NULL,
	[IsSequentialFIFO] bit NOT NULL,
	[MaxDegreeOfParallelism] int NULL,
	[TimeoutForMessageProcessing] time(7) NOT NULL,
	[MaxMessageProcessingRetryCount] int NOT NULL,
	[Properties] nvarchar(max) NULL,
	[IdProcessingMode] uniqueidentifier NOT NULL,
	[IdSuspendingMode] uniqueidentifier NOT NULL,
	[IdInboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [inbox].[InboxQueueProcessingMode]
(
	[IdInboxQueueProcessingMode] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [mbox].[Message]
(
	[IdMessage] uniqueidentifier NOT NULL,
	[IdMessageType] uniqueidentifier NOT NULL,
	[IdMessageStatus] uniqueidentifier NOT NULL,
	[IdMessageContent] uniqueidentifier NULL,
	[IdQueue] uniqueidentifier NULL,
	[IdTopic] uniqueidentifier NULL,
	[MessageId] nvarchar(511) NULL,
	[BusinessId] nvarchar(511) NULL,
	[CorrelationId] nvarchar(511) NULL,
	[SessionId] uniqueidentifier NULL,
	[SessionMessagePartId] bigint NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[Properties] nvarchar(max) NULL,
	[Publisher] nvarchar(511) NULL,
	[PublisherId] nvarchar(511) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ValidToUtc] datetime2(7) NULL,
	[Priority] int NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[MessageArchive]
(
	[IdMessage] uniqueidentifier NOT NULL,
	[IdMessageType] uniqueidentifier NOT NULL,
	[IdMessageStatus] uniqueidentifier NOT NULL,
	[IdMessageContent] uniqueidentifier NULL,
	[IdQueue] uniqueidentifier NULL,
	[IdTopic] uniqueidentifier NULL,
	[MessageId] nvarchar(511) NULL,
	[BusinessId] nvarchar(511) NULL,
	[CorrelationId] nvarchar(511) NULL,
	[SessionId] uniqueidentifier NULL,
	[SessionMessagePartId] bigint NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[Properties] nvarchar(max) NULL,
	[Publisher] nvarchar(511) NULL,
	[PublisherId] nvarchar(511) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ValidToUtc] datetime2(7) NULL,
	[Priority] int NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[MessageBoxInstance]
(
	[IdMessageBoxInstance] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Version] nvarchar(15) NOT NULL,
	[MaxDegreeOfQueueParallelism] int NOT NULL,
	[MaxDegreeOfTopicParallelism] int NOT NULL,
	[IdLogLevel] int NOT NULL
)
GO

CREATE TABLE [mbox].[MessageBoxProcessingLog]
(
	[IdMessageBoxProcessingLog] uniqueidentifier NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL,
	[IdQueue] uniqueidentifier NOT NULL,
	[IdTopic] uniqueidentifier NOT NULL,
	[IdTopicSubscription] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdLogLevel] int NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL
)
GO

CREATE TABLE [mbox].[MessageContent]
(
	[IdMessageContent] uniqueidentifier NOT NULL,
	[MimeType] nvarchar(1023) NOT NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[Name] nvarchar(511) NULL,
	[RelativePath] nvarchar(1023) NULL,
	[Metadata] nvarchar(max) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL
)
GO

CREATE TABLE [mbox].[MessageProcessingLog]
(
	[IdMessageProcessingLog] uniqueidentifier NOT NULL,
	[IdMessage] uniqueidentifier NOT NULL,
	[IdQueuedMessage] uniqueidentifier NULL,
	[IdSubscribedMessage] uniqueidentifier NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdMessageProcessingStatus] uniqueidentifier NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[MessageProcessingStatus]
(
	[IdMessageProcessingStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [mbox].[MessageStatus]
(
	[IdMessageStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [mbox].[MessageType]
(
	[IdMessageType] uniqueidentifier NOT NULL,
	[Code] nvarchar(127) NOT NULL,
	[Name] nvarchar(127) NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxInstance]
(
	[IdOutboxInstance] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Version] nvarchar(15) NOT NULL,
	[MaxDegreeOfQueueParallelism] int NOT NULL,
	[IdLogLevel] int NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxMessage]
(
	[IdOutboxMessage] uniqueidentifier NOT NULL,
	[IdMessageType] uniqueidentifier NOT NULL,
	[IdOutboxMessageStatus] uniqueidentifier NOT NULL,
	[IdMessageContent] uniqueidentifier NULL,
	[IdOutboxQueue] uniqueidentifier NOT NULL,
	[MessageId] nvarchar(511) NULL,
	[BusinessId] nvarchar(511) NULL,
	[CorrelationId] nvarchar(511) NULL,
	[SessionId] uniqueidentifier NULL,
	[SessionMessagePartId] bigint NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[Properties] nvarchar(max) NULL,
	[Publisher] nvarchar(511) NULL,
	[PublisherId] nvarchar(511) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[LastProcessingTimeoutUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL,
	[TargetTopic] nvarchar(1023) NULL,
	[TargetQueueName] nvarchar(1023) NULL,
	[IdOutboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxMessageArchive]
(
	[IdOutboxMessage] uniqueidentifier NOT NULL,
	[IdMessageType] uniqueidentifier NOT NULL,
	[IdOutboxMessageStatus] uniqueidentifier NOT NULL,
	[IdMessageContent] uniqueidentifier NULL,
	[IdOutboxQueue] uniqueidentifier NOT NULL,
	[MessageId] nvarchar(511) NULL,
	[BusinessId] nvarchar(511) NULL,
	[CorrelationId] nvarchar(511) NULL,
	[SessionId] uniqueidentifier NULL,
	[SessionMessagePartId] bigint NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[Properties] nvarchar(max) NULL,
	[Publisher] nvarchar(511) NULL,
	[PublisherId] nvarchar(511) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[LastProcessingTimeoutUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL,
	[TargetTopic] nvarchar(1023) NULL,
	[TargetQueueName] nvarchar(1023) NULL,
	[IdOutboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxMessageContent]
(
	[IdOutboxMessageContent] uniqueidentifier NOT NULL,
	[MimeType] nvarchar(1023) NOT NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[Name] nvarchar(511) NULL,
	[RelativePath] nvarchar(1023) NULL,
	[Metadata] nvarchar(max) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL
)
GO

CREATE TABLE [outbox].[OutboxMessageProcessingLog]
(
	[IdOutboxMessageProcessingLog] uniqueidentifier NOT NULL,
	[IdOutboxMessage] uniqueidentifier NOT NULL,
	[IdOutboxQueue] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdOutboxMessageStatus] uniqueidentifier NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL,
	[IdOutboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxMessageStatus]
(
	[IdOutboxMessageStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxMessageType]
(
	[IdOutboxMessageType] uniqueidentifier NOT NULL,
	[Code] nvarchar(127) NOT NULL,
	[Name] nvarchar(127) NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdOutboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxProcessingLog]
(
	[IdOutboxProcessingLog] uniqueidentifier NOT NULL,
	[IdOutboxInstance] uniqueidentifier NOT NULL,
	[IdOutboxQueue] uniqueidentifier NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdLogLevel] int NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL
)
GO

CREATE TABLE [outbox].[OutboxQueue]
(
	[IdOutboxQueue] uniqueidentifier NOT NULL,
	[Name] nvarchar(1023) NOT NULL,
	[ReceivedEventNamespace] nvarchar(1023) NOT NULL,
	[IdMessageType] uniqueidentifier NULL,
	[IsActive] bit NOT NULL,
	[IsSequentialFIFO] bit NOT NULL,
	[MessagesBatchCount] int NOT NULL,
	[MaxDegreeOfParallelism] int NULL,
	[TimeoutForMessageProcessing] time(7) NOT NULL,
	[MaxMessageProcessingRetryCount] int NOT NULL,
	[Properties] nvarchar(max) NULL,
	[IdProcessingMode] uniqueidentifier NOT NULL,
	[IdSuspendingMode] uniqueidentifier NOT NULL,
	[IdOutboxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [outbox].[OutboxQueueProcessingMode]
(
	[IdOutboxQueueProcessingMode] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [mbox].[Queue]
(
	[IdQueue] uniqueidentifier NOT NULL,
	[Name] nvarchar(1023) NOT NULL,
	[ReceivedEventNamespace] nvarchar(1023) NOT NULL,
	[IdMessageType] uniqueidentifier NULL,
	[IsActive] bit NOT NULL,
	[IsSequentialFIFO] bit NOT NULL,
	[MessagesBatchCount] int NOT NULL,
	[MaxDegreeOfParallelism] int NULL,
	[TimeoutForMessageProcessing] time(7) NOT NULL,
	[MaxMessageProcessingRetryCount] int NOT NULL,
	[Properties] nvarchar(max) NULL,
	[IdProcessingMode] uniqueidentifier NOT NULL,
	[IdSuspendingMode] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NULL,
	[IdOrchestration] uniqueidentifier NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[QueuedMessage]
(
	[IdQueuedMessage] uniqueidentifier NOT NULL,
	[IdQueue] uniqueidentifier NOT NULL,
	[IdMessage] uniqueidentifier NOT NULL,
	[IdMessageProcessingStatus] uniqueidentifier NOT NULL,
	[AssignedUtc] datetime2(7) NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[LastProcessingTimeoutUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[QueueProcessingMode]
(
	[IdQueueProcessingMode] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [mbox].[SubscribedMessage]
(
	[IdSubscribedMessage] uniqueidentifier NOT NULL,
	[IdTopicSubscription] uniqueidentifier NOT NULL,
	[IdMessage] uniqueidentifier NOT NULL,
	[IdMessageProcessingStatus] uniqueidentifier NOT NULL,
	[AssignedUtc] datetime2(7) NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[LastProcessingTimeoutUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[Topic]
(
	[IdTopic] uniqueidentifier NOT NULL,
	[Name] nvarchar(1023) NOT NULL,
	[IsActive] bit NOT NULL,
	[IsSequentialFIFO] bit NOT NULL,
	[MessagesBatchCount] int NOT NULL,
	[MaxDegreeOfParallelism] int NULL,
	[TimeoutForMessageProcessing] time(7) NOT NULL,
	[MaxMessageProcessingRetryCount] int NOT NULL,
	[Properties] nvarchar(max) NULL,
	[IdProcessingMode] uniqueidentifier NOT NULL,
	[IdSuspendingMode] uniqueidentifier NOT NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [mbox].[TopicSubscription]
(
	[IdTopicSubscription] uniqueidentifier NOT NULL,
	[IdTopic] uniqueidentifier NOT NULL,
	[SubscriptionName] nvarchar(511) NOT NULL,
	[ReceivedEventNamespace] nvarchar(1023) NOT NULL,
	[IsActive] bit NOT NULL,
	[IsSequentialFIFO] bit NOT NULL,
	[MessagesBatchCount] int NOT NULL,
	[MaxDegreeOfParallelism] int NULL,
	[TimeoutForMessageProcessing] time(7) NOT NULL,
	[MaxMessageProcessingRetryCount] int NOT NULL,
	[Properties] nvarchar(max) NULL,
	[IdProcessingMode] uniqueidentifier NOT NULL,
	[IdSuspendingMode] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NULL,
	[IdOrchestration] uniqueidentifier NULL,
	[IdMessageBoxInstance] uniqueidentifier NOT NULL
)
GO

ALTER TABLE [devt].[BlockedDomainEventType] 
 ADD CONSTRAINT [PK_BlockedDomainEventType]
	PRIMARY KEY CLUSTERED ([IdBlockedDomainEventType] ASC)
GO

ALTER TABLE [devt].[BlockedDomainEventType] 
 ADD CONSTRAINT [UQ_BlockedDomainEventType_Namespace] UNIQUE NONCLUSTERED ([Namespace] ASC)
GO

ALTER TABLE [inbox].[BlockedInboxMessageType] 
 ADD CONSTRAINT [PK_BlockedInboxMessageType]
	PRIMARY KEY CLUSTERED ([IdBlockedInboxMessageType] ASC)
GO

ALTER TABLE [inbox].[BlockedInboxMessageType] 
 ADD CONSTRAINT [UQ_BlockedInboxMessageType_Namespace] UNIQUE NONCLUSTERED ([Namespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_BlockedInboxMessageType_InboxInstance] 
 ON [inbox].[BlockedInboxMessageType] ([IdInboxInstance] ASC)
GO

ALTER TABLE [mbox].[BlockedMessageType] 
 ADD CONSTRAINT [PK_BlockedMessageType]
	PRIMARY KEY CLUSTERED ([IdBlockedMessageType] ASC)
GO

ALTER TABLE [mbox].[BlockedMessageType] 
 ADD CONSTRAINT [UQ_BlockedMessageType_Namespace] UNIQUE NONCLUSTERED ([Namespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_BlockedMessageType_MessageBoxInstance] 
 ON [mbox].[BlockedMessageType] ([IdMessageBoxInstance] ASC)
GO

ALTER TABLE [outbox].[BlockedOutboxMessageType] 
 ADD CONSTRAINT [PK_BlockedOutboxMessageType]
	PRIMARY KEY CLUSTERED ([IdBlockedOutboxMessageType] ASC)
GO

ALTER TABLE [outbox].[BlockedOutboxMessageType] 
 ADD CONSTRAINT [UQ_BlockedOutboxMessageType_Namespace] UNIQUE NONCLUSTERED ([Namespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_BlockedOutboxMessageType_OutboxInstance] 
 ON [outbox].[BlockedOutboxMessageType] ([IdOutboxInstance] ASC)
GO

ALTER TABLE [devt].[DomainEvent] 
 ADD CONSTRAINT [PK_DomainEvent]
	PRIMARY KEY CLUSTERED ([IdDomainEvent] ASC)
GO

ALTER TABLE [devt].[DomainEvent] 
 ADD CONSTRAINT [UQ_DomainEvent_IdContent] UNIQUE NONCLUSTERED ([IdContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_DomainEvent_DomainEventContent] 
 ON [devt].[DomainEvent] ([IdContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_DomainEvent_DomainEventProcessingStatus] 
 ON [devt].[DomainEvent] ([IdDomainEventProcessingStatus] ASC)
GO

ALTER TABLE [devt].[DomainEventContent] 
 ADD CONSTRAINT [PK_DomainEventContent]
	PRIMARY KEY CLUSTERED ([IdDomainEventContent] ASC)
GO

ALTER TABLE [devt].[DomainEventProcessingLog] 
 ADD CONSTRAINT [PK_DomainEventProcessingLog]
	PRIMARY KEY CLUSTERED ([IdDomainEventProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_DomainEventProcessingLog_DomainEvent] 
 ON [devt].[DomainEventProcessingLog] ([IdDomainEvent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_DomainEventProcessingLog_DomainEventProcessingStatus] 
 ON [devt].[DomainEventProcessingLog] ([IdDomainEventProcessingStatus] ASC)
GO

ALTER TABLE [devt].[DomainEventProcessingStatus] 
 ADD CONSTRAINT [PK_DomainEventProcessingStatus]
	PRIMARY KEY CLUSTERED ([IdDomainEventProcessingStatus] ASC)
GO

ALTER TABLE [inbox].[InboxInstance] 
 ADD CONSTRAINT [PK_InboxInstance]
	PRIMARY KEY CLUSTERED ([IdInboxInstance] ASC)
GO

ALTER TABLE [inbox].[InboxMessage] 
 ADD CONSTRAINT [PK_InboxMessage]
	PRIMARY KEY CLUSTERED ([IdInboxMessage] ASC)
GO

ALTER TABLE [inbox].[InboxMessage] 
 ADD CONSTRAINT [UQ_InboxMessage_IdMessageContent] UNIQUE NONCLUSTERED ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessage_InboxInstance] 
 ON [inbox].[InboxMessage] ([IdInboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessage_InboxMessageContent] 
 ON [inbox].[InboxMessage] ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessage_InboxMessageStatus] 
 ON [inbox].[InboxMessage] ([IdInboxMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessage_InboxQueue] 
 ON [inbox].[InboxMessage] ([IdInboxQueue] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessage_MessageType] 
 ON [inbox].[InboxMessage] ([IdMessageType] ASC)
GO

ALTER TABLE [inbox].[InboxMessageArchive] 
 ADD CONSTRAINT [PK_InboxMessageArchive]
	PRIMARY KEY CLUSTERED ([IdInboxMessage] ASC)
GO

ALTER TABLE [inbox].[InboxMessageArchive] 
 ADD CONSTRAINT [UQ_InboxMessageArchive_IdMessageContent] UNIQUE NONCLUSTERED ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageArchive_InboxInstance] 
 ON [inbox].[InboxMessageArchive] ([IdInboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageArchive_InboxMessageContent] 
 ON [inbox].[InboxMessageArchive] ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageArchive_InboxMessageStatus] 
 ON [inbox].[InboxMessageArchive] ([IdInboxMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageArchive_InboxQueue] 
 ON [inbox].[InboxMessageArchive] ([IdInboxQueue] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageArchive_MessageType] 
 ON [inbox].[InboxMessageArchive] ([IdMessageType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageArchive_Table1] 
 ON [inbox].[InboxMessageArchive] ([IdInboxInstance] ASC)
GO

ALTER TABLE [inbox].[InboxMessageContent] 
 ADD CONSTRAINT [PK_InboxMessageContent]
	PRIMARY KEY CLUSTERED ([IdInboxMessageContent] ASC)
GO

ALTER TABLE [inbox].[InboxMessageProcessingLog] 
 ADD CONSTRAINT [PK_InboxMessageProcessingLog]
	PRIMARY KEY CLUSTERED ([IdInboxMessageProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageProcessingLog_InboxInstance] 
 ON [inbox].[InboxMessageProcessingLog] ([IdInboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageProcessingLog_InboxMessageStatus] 
 ON [inbox].[InboxMessageProcessingLog] ([IdInboxMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageProcessingLog_InboxQueue] 
 ON [inbox].[InboxMessageProcessingLog] ([IdInboxQueue] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_InboxMessageProcessingLog_IdInboxMessage] 
 ON [inbox].[InboxMessageProcessingLog] ([IdInboxMessage] ASC)
GO

ALTER TABLE [inbox].[InboxMessageStatus] 
 ADD CONSTRAINT [PK_InboxMessageStatus]
	PRIMARY KEY CLUSTERED ([IdInboxMessageStatus] ASC)
GO

ALTER TABLE [inbox].[InboxMessageType] 
 ADD CONSTRAINT [PK_InboxMessageType]
	PRIMARY KEY CLUSTERED ([IdInboxMessageType] ASC)
GO

ALTER TABLE [inbox].[InboxMessageType] 
 ADD CONSTRAINT [UQ_InboxMessageType_Namespace] UNIQUE NONCLUSTERED ([Namespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxMessageType_InboxInstance] 
 ON [inbox].[InboxMessageType] ([IdInboxInstance] ASC)
GO

ALTER TABLE [inbox].[InboxProcessingLog] 
 ADD CONSTRAINT [PK_InboxProcessingLog]
	PRIMARY KEY CLUSTERED ([IdInboxProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxProcessingLog_InboxInstance] 
 ON [inbox].[InboxProcessingLog] ([IdInboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxProcessingLog_InboxQueue] 
 ON [inbox].[InboxProcessingLog] ([IdInboxQueue] ASC)
GO

ALTER TABLE [inbox].[InboxQueue] 
 ADD CONSTRAINT [PK_InboxQueue]
	PRIMARY KEY CLUSTERED ([IdInboxQueue] ASC)
GO

ALTER TABLE [inbox].[InboxQueue] 
 ADD CONSTRAINT [UQ_InboxQueue_Name] UNIQUE NONCLUSTERED ([Name] ASC)
GO

ALTER TABLE [inbox].[InboxQueue] 
 ADD CONSTRAINT [UQ_InboxQueue_ReceivedEventNamespace] UNIQUE NONCLUSTERED ([ReceivedEventNamespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxQueue_InboxInstance] 
 ON [inbox].[InboxQueue] ([IdInboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxQueue_InboxQueueProcessingMode] 
 ON [inbox].[InboxQueue] ([IdSuspendingMode] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxQueue_InboxQueueProcessingMode_02] 
 ON [inbox].[InboxQueue] ([IdProcessingMode] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_InboxQueue_MessageType] 
 ON [inbox].[InboxQueue] ([IdMessageType] ASC)
GO

ALTER TABLE [inbox].[InboxQueueProcessingMode] 
 ADD CONSTRAINT [PK_InboxQueueProcessingMode]
	PRIMARY KEY CLUSTERED ([IdInboxQueueProcessingMode] ASC)
GO

ALTER TABLE [mbox].[Message] 
 ADD CONSTRAINT [PK_Message]
	PRIMARY KEY CLUSTERED ([IdMessage] ASC)
GO

ALTER TABLE [mbox].[Message] 
 ADD CONSTRAINT [UQ_Message_IdMessageContent] UNIQUE NONCLUSTERED ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Message_MessageBoxInstance] 
 ON [mbox].[Message] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Message_MessageContent] 
 ON [mbox].[Message] ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Message_MessageStatus] 
 ON [mbox].[Message] ([IdMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Message_MessageType] 
 ON [mbox].[Message] ([IdMessageType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Message_Queue] 
 ON [mbox].[Message] ([IdQueue] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Message_Topic] 
 ON [mbox].[Message] ([IdTopic] ASC)
GO

ALTER TABLE [mbox].[MessageArchive] 
 ADD CONSTRAINT [PK_MessageArchive]
	PRIMARY KEY CLUSTERED ([IdMessage] ASC)
GO

ALTER TABLE [mbox].[MessageArchive] 
 ADD CONSTRAINT [UQ_MessageArchive_IdMessageContent] UNIQUE NONCLUSTERED ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageArchive_MessageBoxInstance] 
 ON [mbox].[MessageArchive] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageArchive_MessageContent] 
 ON [mbox].[MessageArchive] ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageArchive_MessageStatus] 
 ON [mbox].[MessageArchive] ([IdMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageArchive_MessageType] 
 ON [mbox].[MessageArchive] ([IdMessageType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageArchive_Queue] 
 ON [mbox].[MessageArchive] ([IdQueue] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageArchive_Topic] 
 ON [mbox].[MessageArchive] ([IdTopic] ASC)
GO

ALTER TABLE [mbox].[MessageBoxInstance] 
 ADD CONSTRAINT [PK_MessageBoxInstance]
	PRIMARY KEY CLUSTERED ([IdMessageBoxInstance] ASC)
GO

ALTER TABLE [mbox].[MessageBoxProcessingLog] 
 ADD CONSTRAINT [PK_MessageBoxProcessingLog]
	PRIMARY KEY CLUSTERED ([IdMessageBoxProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageBoxProcessingLog_MessageBoxInstance] 
 ON [mbox].[MessageBoxProcessingLog] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageBoxProcessingLog_Queue] 
 ON [mbox].[MessageBoxProcessingLog] ([IdQueue] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageBoxProcessingLog_Topic] 
 ON [mbox].[MessageBoxProcessingLog] ([IdTopic] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageBoxProcessingLog_TopicSubscription] 
 ON [mbox].[MessageBoxProcessingLog] ([IdTopicSubscription] ASC)
GO

ALTER TABLE [mbox].[MessageContent] 
 ADD CONSTRAINT [PK_MessageContent]
	PRIMARY KEY CLUSTERED ([IdMessageContent] ASC)
GO

ALTER TABLE [mbox].[MessageProcessingLog] 
 ADD CONSTRAINT [PK_MessageProcessingLog]
	PRIMARY KEY CLUSTERED ([IdMessageProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageProcessingLog_MessageBoxInstance] 
 ON [mbox].[MessageProcessingLog] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageProcessingLog_MessageProcessingStatus] 
 ON [mbox].[MessageProcessingLog] ([IdMessageProcessingStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageProcessingLog_QueuedMessage] 
 ON [mbox].[MessageProcessingLog] ([IdQueuedMessage] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageProcessingLog_SubscribedMessage] 
 ON [mbox].[MessageProcessingLog] ([IdSubscribedMessage] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_MessageProcessingLog_IdMessage] 
 ON [mbox].[MessageProcessingLog] ([IdMessage] ASC)
GO

ALTER TABLE [mbox].[MessageProcessingStatus] 
 ADD CONSTRAINT [PK_MessageProcessingStatus]
	PRIMARY KEY CLUSTERED ([IdMessageProcessingStatus] ASC)
GO

ALTER TABLE [mbox].[MessageStatus] 
 ADD CONSTRAINT [PK_MessageStatus]
	PRIMARY KEY CLUSTERED ([IdMessageStatus] ASC)
GO

ALTER TABLE [mbox].[MessageType] 
 ADD CONSTRAINT [PK_MessageType]
	PRIMARY KEY CLUSTERED ([IdMessageType] ASC)
GO

ALTER TABLE [mbox].[MessageType] 
 ADD CONSTRAINT [UQ_MessageType_Namespace] UNIQUE NONCLUSTERED ([Namespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_MessageType_MessageBoxInstance] 
 ON [mbox].[MessageType] ([IdMessageBoxInstance] ASC)
GO

ALTER TABLE [outbox].[OutboxInstance] 
 ADD CONSTRAINT [PK_OutboxInstance]
	PRIMARY KEY CLUSTERED ([IdOutboxInstance] ASC)
GO

ALTER TABLE [outbox].[OutboxMessage] 
 ADD CONSTRAINT [PK_OutboxMessage]
	PRIMARY KEY CLUSTERED ([IdOutboxMessage] ASC)
GO

ALTER TABLE [outbox].[OutboxMessage] 
 ADD CONSTRAINT [UQ_OutboxMessage_IdMessageContent] UNIQUE NONCLUSTERED ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessage_MessageType] 
 ON [outbox].[OutboxMessage] ([IdMessageType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessage_OutboxInstance] 
 ON [outbox].[OutboxMessage] ([IdOutboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessage_OutboxMessageContent] 
 ON [outbox].[OutboxMessage] ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessage_OutboxMessageStatus] 
 ON [outbox].[OutboxMessage] ([IdOutboxMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessage_OutboxQueue] 
 ON [outbox].[OutboxMessage] ([IdOutboxQueue] ASC)
GO

ALTER TABLE [outbox].[OutboxMessageArchive] 
 ADD CONSTRAINT [PK_OutboxMessageArchive]
	PRIMARY KEY CLUSTERED ([IdOutboxMessage] ASC)
GO

ALTER TABLE [outbox].[OutboxMessageArchive] 
 ADD CONSTRAINT [UQ_OutboxMessageArchive_IdMessageContent] UNIQUE NONCLUSTERED ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageArchive_MessageType] 
 ON [outbox].[OutboxMessageArchive] ([IdMessageType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageArchive_OutboxInstance] 
 ON [outbox].[OutboxMessageArchive] ([IdOutboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageArchive_OutboxMessageContent] 
 ON [outbox].[OutboxMessageArchive] ([IdMessageContent] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageArchive_OutboxMessageStatus] 
 ON [outbox].[OutboxMessageArchive] ([IdOutboxMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageArchive_OutboxQueue] 
 ON [outbox].[OutboxMessageArchive] ([IdOutboxQueue] ASC)
GO

ALTER TABLE [outbox].[OutboxMessageContent] 
 ADD CONSTRAINT [PK_OutboxMessageContent]
	PRIMARY KEY CLUSTERED ([IdOutboxMessageContent] ASC)
GO

ALTER TABLE [outbox].[OutboxMessageProcessingLog] 
 ADD CONSTRAINT [PK_OutboxMessageProcessingLog]
	PRIMARY KEY CLUSTERED ([IdOutboxMessageProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageProcessingLog_OutboxInstance] 
 ON [outbox].[OutboxMessageProcessingLog] ([IdOutboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageProcessingLog_OutboxMessageStatus] 
 ON [outbox].[OutboxMessageProcessingLog] ([IdOutboxMessageStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageProcessingLog_OutboxQueue] 
 ON [outbox].[OutboxMessageProcessingLog] ([IdOutboxQueue] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_OutboxMessageProcessingLog_IdOutboxMessage] 
 ON [outbox].[OutboxMessageProcessingLog] ([IdOutboxMessage] ASC)
GO

ALTER TABLE [outbox].[OutboxMessageStatus] 
 ADD CONSTRAINT [PK_OutboxMessageStatus]
	PRIMARY KEY CLUSTERED ([IdOutboxMessageStatus] ASC)
GO

ALTER TABLE [outbox].[OutboxMessageType] 
 ADD CONSTRAINT [PK_OutboxMessageType]
	PRIMARY KEY CLUSTERED ([IdOutboxMessageType] ASC)
GO

ALTER TABLE [outbox].[OutboxMessageType] 
 ADD CONSTRAINT [UQ_OutboxMessageType_Namespace] UNIQUE NONCLUSTERED ([Namespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxMessageType_OutboxInstance] 
 ON [outbox].[OutboxMessageType] ([IdOutboxInstance] ASC)
GO

ALTER TABLE [outbox].[OutboxProcessingLog] 
 ADD CONSTRAINT [PK_OutboxProcessingLog]
	PRIMARY KEY CLUSTERED ([IdOutboxProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxProcessingLog_OutboxInstance] 
 ON [outbox].[OutboxProcessingLog] ([IdOutboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxProcessingLog_OutboxQueue] 
 ON [outbox].[OutboxProcessingLog] ([IdOutboxQueue] ASC)
GO

ALTER TABLE [outbox].[OutboxQueue] 
 ADD CONSTRAINT [PK_OutboxQueue]
	PRIMARY KEY CLUSTERED ([IdOutboxQueue] ASC)
GO

ALTER TABLE [outbox].[OutboxQueue] 
 ADD CONSTRAINT [UQ_OutboxQueue_Name] UNIQUE NONCLUSTERED ([Name] ASC)
GO

ALTER TABLE [outbox].[OutboxQueue] 
 ADD CONSTRAINT [UQ_OutboxQueue_ReceivedEventNamespace] UNIQUE NONCLUSTERED ([ReceivedEventNamespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxQueue_MessageType] 
 ON [outbox].[OutboxQueue] ([IdMessageType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxQueue_OutboxInstance] 
 ON [outbox].[OutboxQueue] ([IdOutboxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxQueue_OutboxQueueProcessingMode] 
 ON [outbox].[OutboxQueue] ([IdProcessingMode] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OutboxQueue_OutboxQueueProcessingMode_02] 
 ON [outbox].[OutboxQueue] ([IdSuspendingMode] ASC)
GO

ALTER TABLE [outbox].[OutboxQueueProcessingMode] 
 ADD CONSTRAINT [PK_OutboxQueueProcessingMode]
	PRIMARY KEY CLUSTERED ([IdOutboxQueueProcessingMode] ASC)
GO

ALTER TABLE [mbox].[Queue] 
 ADD CONSTRAINT [PK_Queue]
	PRIMARY KEY CLUSTERED ([IdQueue] ASC)
GO

ALTER TABLE [mbox].[Queue] 
 ADD CONSTRAINT [UQ_Queue_Name] UNIQUE NONCLUSTERED ([Name] ASC)
GO

ALTER TABLE [mbox].[Queue] 
 ADD CONSTRAINT [UQ_Queue_ReceivedEventNamespace] UNIQUE NONCLUSTERED ([ReceivedEventNamespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Queue_MessageBoxInstance] 
 ON [mbox].[Queue] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Queue_MessageType] 
 ON [mbox].[Queue] ([IdMessageType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Queue_QueueProcessingMode] 
 ON [mbox].[Queue] ([IdProcessingMode] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Queue_QueueProcessingMode_02] 
 ON [mbox].[Queue] ([IdSuspendingMode] ASC)
GO

ALTER TABLE [mbox].[QueuedMessage] 
 ADD CONSTRAINT [PK_QueuedMessage]
	PRIMARY KEY CLUSTERED ([IdQueuedMessage] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_QueuedMessage_MessageBoxInstance] 
 ON [mbox].[QueuedMessage] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_QueuedMessage_MessageProcessingStatus] 
 ON [mbox].[QueuedMessage] ([IdMessageProcessingStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_QueuedMessage_Queue] 
 ON [mbox].[QueuedMessage] ([IdQueue] ASC)
GO

ALTER TABLE [mbox].[QueueProcessingMode] 
 ADD CONSTRAINT [PK_QueueProcessingMode]
	PRIMARY KEY CLUSTERED ([IdQueueProcessingMode] ASC)
GO

ALTER TABLE [mbox].[SubscribedMessage] 
 ADD CONSTRAINT [PK_SubscribedMessage]
	PRIMARY KEY CLUSTERED ([IdSubscribedMessage] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_SubscribedMessage_MessageBoxInstance] 
 ON [mbox].[SubscribedMessage] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_SubscribedMessage_MessageProcessingStatus] 
 ON [mbox].[SubscribedMessage] ([IdMessageProcessingStatus] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_SubscribedMessage_TopicSubscription] 
 ON [mbox].[SubscribedMessage] ([IdTopicSubscription] ASC)
GO

ALTER TABLE [mbox].[Topic] 
 ADD CONSTRAINT [PK_Topic]
	PRIMARY KEY CLUSTERED ([IdTopic] ASC)
GO

ALTER TABLE [mbox].[Topic] 
 ADD CONSTRAINT [UQ_Topic_Name] UNIQUE NONCLUSTERED ([Name] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Topic_MessageBoxInstance] 
 ON [mbox].[Topic] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Topic_QueueProcessingMode] 
 ON [mbox].[Topic] ([IdProcessingMode] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Topic_QueueProcessingMode_02] 
 ON [mbox].[Topic] ([IdSuspendingMode] ASC)
GO

ALTER TABLE [mbox].[TopicSubscription] 
 ADD CONSTRAINT [PK_TopicSubscription]
	PRIMARY KEY CLUSTERED ([IdTopicSubscription] ASC)
GO

ALTER TABLE [mbox].[TopicSubscription] 
 ADD CONSTRAINT [UQ_TopicSubscription_IdTopic_SubscriptionName] UNIQUE NONCLUSTERED ([IdTopic] ASC,[SubscriptionName] ASC)
GO

ALTER TABLE [mbox].[TopicSubscription] 
 ADD CONSTRAINT [UQ_TopicSubscription_ReceivedEventNamespace] UNIQUE NONCLUSTERED ([ReceivedEventNamespace] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_TopicSubscription_MessageBoxInstance] 
 ON [mbox].[TopicSubscription] ([IdMessageBoxInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_TopicSubscription_QueueProcessingMode] 
 ON [mbox].[TopicSubscription] ([IdProcessingMode] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_TopicSubscription_QueueProcessingMode_02] 
 ON [mbox].[TopicSubscription] ([IdSuspendingMode] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_TopicSubscription_Topic] 
 ON [mbox].[TopicSubscription] ([IdTopic] ASC)
GO

ALTER TABLE [inbox].[BlockedInboxMessageType] ADD CONSTRAINT [FK_BlockedInboxMessageType_InboxInstance]
	FOREIGN KEY ([IdInboxInstance]) REFERENCES [inbox].[InboxInstance] ([IdInboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[BlockedMessageType] ADD CONSTRAINT [FK_BlockedMessageType_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[BlockedOutboxMessageType] ADD CONSTRAINT [FK_BlockedOutboxMessageType_OutboxInstance]
	FOREIGN KEY ([IdOutboxInstance]) REFERENCES [outbox].[OutboxInstance] ([IdOutboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [devt].[DomainEvent] ADD CONSTRAINT [FK_DomainEvent_IdDomainEventContent]
	FOREIGN KEY ([IdContent]) REFERENCES [devt].[DomainEventContent] ([IdDomainEventContent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [devt].[DomainEvent] ADD CONSTRAINT [FK_DomainEvent_IdDomainEventProcessingStatus]
	FOREIGN KEY ([IdDomainEventProcessingStatus]) REFERENCES [devt].[DomainEventProcessingStatus] ([IdDomainEventProcessingStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [devt].[DomainEventProcessingLog] ADD CONSTRAINT [FK_DomainEventProcessingLog_IdDomainEvent]
	FOREIGN KEY ([IdDomainEvent]) REFERENCES [devt].[DomainEvent] ([IdDomainEvent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [devt].[DomainEventProcessingLog] ADD CONSTRAINT [FK_DomainEventProcessingLog_IdDomainEventProcessingStatus]
	FOREIGN KEY ([IdDomainEventProcessingStatus]) REFERENCES [devt].[DomainEventProcessingStatus] ([IdDomainEventProcessingStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessage] ADD CONSTRAINT [FK_InboxMessage_IdInboxInstance]
	FOREIGN KEY ([IdInboxInstance]) REFERENCES [inbox].[InboxInstance] ([IdInboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessage] ADD CONSTRAINT [FK_InboxMessage_IdInboxMessageStatus]
	FOREIGN KEY ([IdInboxMessageStatus]) REFERENCES [inbox].[InboxMessageStatus] ([IdInboxMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessage] ADD CONSTRAINT [FK_InboxMessage_IdInboxQueue]
	FOREIGN KEY ([IdInboxQueue]) REFERENCES [inbox].[InboxQueue] ([IdInboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessage] ADD CONSTRAINT [FK_InboxMessage_IdMessageContent]
	FOREIGN KEY ([IdMessageContent]) REFERENCES [inbox].[InboxMessageContent] ([IdInboxMessageContent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessage] ADD CONSTRAINT [FK_InboxMessage_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [inbox].[InboxMessageType] ([IdInboxMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageArchive] ADD CONSTRAINT [FK_InboxMessageArchive_IdInboxInstance]
	FOREIGN KEY ([IdInboxInstance]) REFERENCES [inbox].[InboxInstance] ([IdInboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageArchive] ADD CONSTRAINT [FK_InboxMessageArchive_IdInboxMessageStatus]
	FOREIGN KEY ([IdInboxMessageStatus]) REFERENCES [inbox].[InboxMessageStatus] ([IdInboxMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageArchive] ADD CONSTRAINT [FK_InboxMessageArchive_IdInboxQueue]
	FOREIGN KEY ([IdInboxQueue]) REFERENCES [inbox].[InboxQueue] ([IdInboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageArchive] ADD CONSTRAINT [FK_InboxMessageArchive_IdMessageContent]
	FOREIGN KEY ([IdMessageContent]) REFERENCES [inbox].[InboxMessageContent] ([IdInboxMessageContent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageArchive] ADD CONSTRAINT [FK_InboxMessageArchive_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [inbox].[InboxMessageType] ([IdInboxMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageProcessingLog] ADD CONSTRAINT [FK_InboxMessageProcessingLog_IdInboxInstance]
	FOREIGN KEY ([IdInboxInstance]) REFERENCES [inbox].[InboxInstance] ([IdInboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageProcessingLog] ADD CONSTRAINT [FK_InboxMessageProcessingLog_IdInboxMessageStatus]
	FOREIGN KEY ([IdInboxMessageStatus]) REFERENCES [inbox].[InboxMessageStatus] ([IdInboxMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageProcessingLog] ADD CONSTRAINT [FK_InboxMessageProcessingLog_IdInboxQueue]
	FOREIGN KEY ([IdInboxQueue]) REFERENCES [inbox].[InboxQueue] ([IdInboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxMessageType] ADD CONSTRAINT [FK_InboxMessageType_IdInboxInstance]
	FOREIGN KEY ([IdInboxInstance]) REFERENCES [inbox].[InboxInstance] ([IdInboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxProcessingLog] ADD CONSTRAINT [FK_InboxProcessingLog_IdInboxInstance]
	FOREIGN KEY ([IdInboxInstance]) REFERENCES [inbox].[InboxInstance] ([IdInboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxProcessingLog] ADD CONSTRAINT [FK_InboxProcessingLog_InboxQueue]
	FOREIGN KEY ([IdInboxQueue]) REFERENCES [inbox].[InboxQueue] ([IdInboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxQueue] ADD CONSTRAINT [FK_InboxQueue_IdInboxInstance]
	FOREIGN KEY ([IdInboxInstance]) REFERENCES [inbox].[InboxInstance] ([IdInboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxQueue] ADD CONSTRAINT [FK_InboxQueue_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [inbox].[InboxMessageType] ([IdInboxMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxQueue] ADD CONSTRAINT [FK_InboxQueue_IdProcessingMode]
	FOREIGN KEY ([IdProcessingMode]) REFERENCES [inbox].[InboxQueueProcessingMode] ([IdInboxQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [inbox].[InboxQueue] ADD CONSTRAINT [FK_InboxQueue_IdSuspendingMode]
	FOREIGN KEY ([IdSuspendingMode]) REFERENCES [inbox].[InboxQueueProcessingMode] ([IdInboxQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Message] ADD CONSTRAINT [FK_Message_IdMessageContent]
	FOREIGN KEY ([IdMessageContent]) REFERENCES [mbox].[MessageContent] ([IdMessageContent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Message] ADD CONSTRAINT [FK_Message_IdMessageStatus]
	FOREIGN KEY ([IdMessageStatus]) REFERENCES [mbox].[MessageStatus] ([IdMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Message] ADD CONSTRAINT [FK_Message_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [mbox].[MessageType] ([IdMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Message] ADD CONSTRAINT [FK_Message_IdQueue]
	FOREIGN KEY ([IdQueue]) REFERENCES [mbox].[Queue] ([IdQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Message] ADD CONSTRAINT [FK_Message_IdTopic]
	FOREIGN KEY ([IdTopic]) REFERENCES [mbox].[Topic] ([IdTopic]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Message] ADD CONSTRAINT [FK_Message_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageArchive] ADD CONSTRAINT [FK_MessageArchive_IdMessageContent]
	FOREIGN KEY ([IdMessageContent]) REFERENCES [mbox].[MessageContent] ([IdMessageContent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageArchive] ADD CONSTRAINT [FK_MessageArchive_IdMessageStatus]
	FOREIGN KEY ([IdMessageStatus]) REFERENCES [mbox].[MessageStatus] ([IdMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageArchive] ADD CONSTRAINT [FK_MessageArchive_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [mbox].[MessageType] ([IdMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageArchive] ADD CONSTRAINT [FK_MessageArchive_IdQueue]
	FOREIGN KEY ([IdQueue]) REFERENCES [mbox].[Queue] ([IdQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageArchive] ADD CONSTRAINT [FK_MessageArchive_IdTopic]
	FOREIGN KEY ([IdTopic]) REFERENCES [mbox].[Topic] ([IdTopic]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageArchive] ADD CONSTRAINT [FK_MessageArchive_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageBoxProcessingLog] ADD CONSTRAINT [FK_MessageBoxProcessingLog_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageBoxProcessingLog] ADD CONSTRAINT [FK_MessageBoxProcessingLog_Queue]
	FOREIGN KEY ([IdQueue]) REFERENCES [mbox].[Queue] ([IdQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageBoxProcessingLog] ADD CONSTRAINT [FK_MessageBoxProcessingLog_Topic]
	FOREIGN KEY ([IdTopic]) REFERENCES [mbox].[Topic] ([IdTopic]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageBoxProcessingLog] ADD CONSTRAINT [FK_MessageBoxProcessingLog_TopicSubscription]
	FOREIGN KEY ([IdTopicSubscription]) REFERENCES [mbox].[TopicSubscription] ([IdTopicSubscription]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageProcessingLog] ADD CONSTRAINT [FK_MessageProcessingLog_IdMessageProcessingStatus]
	FOREIGN KEY ([IdMessageProcessingStatus]) REFERENCES [mbox].[MessageProcessingStatus] ([IdMessageProcessingStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageProcessingLog] ADD CONSTRAINT [FK_MessageProcessingLog_IdQueuedMessage]
	FOREIGN KEY ([IdQueuedMessage]) REFERENCES [mbox].[QueuedMessage] ([IdQueuedMessage]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageProcessingLog] ADD CONSTRAINT [FK_MessageProcessingLog_IdSubscribedMessage]
	FOREIGN KEY ([IdSubscribedMessage]) REFERENCES [mbox].[SubscribedMessage] ([IdSubscribedMessage]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageProcessingLog] ADD CONSTRAINT [FK_MessageProcessingLog_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[MessageType] ADD CONSTRAINT [FK_MessageType_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessage] ADD CONSTRAINT [FK_OutboxMessage_IdMessageContent]
	FOREIGN KEY ([IdMessageContent]) REFERENCES [outbox].[OutboxMessageContent] ([IdOutboxMessageContent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessage] ADD CONSTRAINT [FK_OutboxMessage_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [outbox].[OutboxMessageType] ([IdOutboxMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessage] ADD CONSTRAINT [FK_OutboxMessage_IdOutboxInstance]
	FOREIGN KEY ([IdOutboxInstance]) REFERENCES [outbox].[OutboxInstance] ([IdOutboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessage] ADD CONSTRAINT [FK_OutboxMessage_IdOutboxMessageStatus]
	FOREIGN KEY ([IdOutboxMessageStatus]) REFERENCES [outbox].[OutboxMessageStatus] ([IdOutboxMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessage] ADD CONSTRAINT [FK_OutboxMessage_IdOutboxQueue]
	FOREIGN KEY ([IdOutboxQueue]) REFERENCES [outbox].[OutboxQueue] ([IdOutboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageArchive] ADD CONSTRAINT [FK_OutboxMessageArchive_IdMessageContent]
	FOREIGN KEY ([IdMessageContent]) REFERENCES [outbox].[OutboxMessageContent] ([IdOutboxMessageContent]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageArchive] ADD CONSTRAINT [FK_OutboxMessageArchive_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [outbox].[OutboxMessageType] ([IdOutboxMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageArchive] ADD CONSTRAINT [FK_OutboxMessageArchive_IdOutboxInstance]
	FOREIGN KEY ([IdOutboxInstance]) REFERENCES [outbox].[OutboxInstance] ([IdOutboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageArchive] ADD CONSTRAINT [FK_OutboxMessageArchive_IdOutboxMessageStatus]
	FOREIGN KEY ([IdOutboxMessageStatus]) REFERENCES [outbox].[OutboxMessageStatus] ([IdOutboxMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageArchive] ADD CONSTRAINT [FK_OutboxMessageArchive_IdOutboxQueue]
	FOREIGN KEY ([IdOutboxQueue]) REFERENCES [outbox].[OutboxQueue] ([IdOutboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageProcessingLog] ADD CONSTRAINT [FK_OutboxMessageProcessingLog_IdOutboxInstance]
	FOREIGN KEY ([IdOutboxInstance]) REFERENCES [outbox].[OutboxInstance] ([IdOutboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageProcessingLog] ADD CONSTRAINT [FK_OutboxMessageProcessingLog_IdOutboxMessageStatus]
	FOREIGN KEY ([IdOutboxMessageStatus]) REFERENCES [outbox].[OutboxMessageStatus] ([IdOutboxMessageStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageProcessingLog] ADD CONSTRAINT [FK_OutboxMessageProcessingLog_IdOutboxQueue]
	FOREIGN KEY ([IdOutboxQueue]) REFERENCES [outbox].[OutboxQueue] ([IdOutboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxMessageType] ADD CONSTRAINT [FK_OutboxMessageType_IdOutboxInstance]
	FOREIGN KEY ([IdOutboxInstance]) REFERENCES [outbox].[OutboxInstance] ([IdOutboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxProcessingLog] ADD CONSTRAINT [FK_OutboxProcessingLog_IdOutboxInstance]
	FOREIGN KEY ([IdOutboxInstance]) REFERENCES [outbox].[OutboxInstance] ([IdOutboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxProcessingLog] ADD CONSTRAINT [FK_OutboxProcessingLog_OutboxQueue]
	FOREIGN KEY ([IdOutboxQueue]) REFERENCES [outbox].[OutboxQueue] ([IdOutboxQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxQueue] ADD CONSTRAINT [FK_OutboxQueue_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [outbox].[OutboxMessageType] ([IdOutboxMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxQueue] ADD CONSTRAINT [FK_OutboxQueue_IdOutboxInstance]
	FOREIGN KEY ([IdOutboxInstance]) REFERENCES [outbox].[OutboxInstance] ([IdOutboxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxQueue] ADD CONSTRAINT [FK_OutboxQueue_IdProcessingMode]
	FOREIGN KEY ([IdProcessingMode]) REFERENCES [outbox].[OutboxQueueProcessingMode] ([IdOutboxQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [outbox].[OutboxQueue] ADD CONSTRAINT [FK_OutboxQueue_IdSuspendingMode]
	FOREIGN KEY ([IdSuspendingMode]) REFERENCES [outbox].[OutboxQueueProcessingMode] ([IdOutboxQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Queue] ADD CONSTRAINT [FK_Queue_IdMessageType]
	FOREIGN KEY ([IdMessageType]) REFERENCES [mbox].[MessageType] ([IdMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Queue] ADD CONSTRAINT [FK_Queue_IdProcessingMode]
	FOREIGN KEY ([IdProcessingMode]) REFERENCES [mbox].[QueueProcessingMode] ([IdQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Queue] ADD CONSTRAINT [FK_Queue_IdSuspendingMode]
	FOREIGN KEY ([IdSuspendingMode]) REFERENCES [mbox].[QueueProcessingMode] ([IdQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Queue] ADD CONSTRAINT [FK_Queue_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[QueuedMessage] ADD CONSTRAINT [FK_QueuedMessage_IdMessageProcessingStatus]
	FOREIGN KEY ([IdMessageProcessingStatus]) REFERENCES [mbox].[MessageProcessingStatus] ([IdMessageProcessingStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[QueuedMessage] ADD CONSTRAINT [FK_QueuedMessage_IdQueue]
	FOREIGN KEY ([IdQueue]) REFERENCES [mbox].[Queue] ([IdQueue]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[QueuedMessage] ADD CONSTRAINT [FK_QueuedMessage_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[SubscribedMessage] ADD CONSTRAINT [FK_SubscribedMessage_IdMessageProcessingStatus]
	FOREIGN KEY ([IdMessageProcessingStatus]) REFERENCES [mbox].[MessageProcessingStatus] ([IdMessageProcessingStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[SubscribedMessage] ADD CONSTRAINT [FK_SubscribedMessage_IdTopicSubscription]
	FOREIGN KEY ([IdTopicSubscription]) REFERENCES [mbox].[TopicSubscription] ([IdTopicSubscription]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[SubscribedMessage] ADD CONSTRAINT [FK_SubscribedMessage_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Topic] ADD CONSTRAINT [FK_Topic_IdProcessingMode]
	FOREIGN KEY ([IdProcessingMode]) REFERENCES [mbox].[QueueProcessingMode] ([IdQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Topic] ADD CONSTRAINT [FK_Topic_IdSuspendingMode]
	FOREIGN KEY ([IdSuspendingMode]) REFERENCES [mbox].[QueueProcessingMode] ([IdQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[Topic] ADD CONSTRAINT [FK_Topic_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[TopicSubscription] ADD CONSTRAINT [FK_TopicSubscription_IdProcessingMode]
	FOREIGN KEY ([IdProcessingMode]) REFERENCES [mbox].[QueueProcessingMode] ([IdQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[TopicSubscription] ADD CONSTRAINT [FK_TopicSubscription_IdSuspendingMode]
	FOREIGN KEY ([IdSuspendingMode]) REFERENCES [mbox].[QueueProcessingMode] ([IdQueueProcessingMode]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[TopicSubscription] ADD CONSTRAINT [FK_TopicSubscription_IdTopic]
	FOREIGN KEY ([IdTopic]) REFERENCES [mbox].[Topic] ([IdTopic]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [mbox].[TopicSubscription] ADD CONSTRAINT [FK_TopicSubscription_MessageBoxInstance]
	FOREIGN KEY ([IdMessageBoxInstance]) REFERENCES [mbox].[MessageBoxInstance] ([IdMessageBoxInstance]) ON DELETE No Action ON UPDATE No Action
GO
