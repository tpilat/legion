CREATE TABLE devt."BlockedDomainEventType"
(
	"IdBlockedDomainEventType" uuid NOT NULL,
	"Namespace" varchar(1023) NOT NULL
);

CREATE TABLE inbox."BlockedInboxMessageType"
(
	"IdBlockedInboxMessageType" uuid NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdInboxInstance" uuid NOT NULL
);

CREATE TABLE mbox."BlockedMessageType"
(
	"IdBlockedMessageType" uuid NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE outbox."BlockedOutboxMessageType"
(
	"IdBlockedOutboxMessageType" uuid NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdOutboxInstance" uuid NOT NULL
);

CREATE TABLE devt."DomainEvent"
(
	"IdDomainEvent" uuid NOT NULL,
	"IdContent" uuid NOT NULL,
	"IdDomainEventProcessingStatus" uuid NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Publisher" varchar(511) NULL,
	"PublisherId" varchar(511) NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"LastProcessingTimeoutUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"Priority" integer NOT NULL
);

CREATE TABLE devt."DomainEventContent"
(
	"IdDomainEventContent" uuid NOT NULL,
	"Content" jsonb NOT NULL
);

CREATE TABLE devt."DomainEventProcessingLog"
(
	"IdDomainEventProcessingLog" uuid NOT NULL,
	"IdDomainEvent" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdDomainEventProcessingStatus" uuid NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL
);

CREATE TABLE devt."DomainEventProcessingStatus"
(
	"IdDomainEventProcessingStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE inbox."InboxInstance"
(
	"IdInboxInstance" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"Name" varchar(255) NOT NULL,
	"Version" varchar(15) NOT NULL,
	"MaxDegreeOfQueueParallelism" integer NOT NULL,
	"IdLogLevel" integer NOT NULL
);

CREATE TABLE inbox."InboxMessage"
(
	"IdInboxMessage" uuid NOT NULL,
	"IdMessageType" uuid NOT NULL,
	"IdInboxMessageStatus" uuid NOT NULL,
	"IdMessageContent" uuid NULL,
	"IdInboxQueue" uuid NOT NULL,
	"MessageId" varchar(511) NULL,
	"BusinessId" varchar(511) NULL,
	"CorrelationId" varchar(511) NULL,
	"SessionId" uuid NULL,
	"SessionMessagePartId" bigint NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Publisher" varchar(511) NULL,
	"PublisherId" varchar(511) NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"LastProcessingTimeoutUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"TargetTopic" varchar(1023) NULL,
	"TargetQueueName" varchar(1023) NULL,
	"IdInboxInstance" uuid NOT NULL
);

CREATE TABLE inbox."InboxMessageArchive"
(
	"IdInboxMessage" uuid NOT NULL,
	"IdMessageType" uuid NOT NULL,
	"IdInboxMessageStatus" uuid NOT NULL,
	"IdMessageContent" uuid NULL,
	"IdInboxQueue" uuid NOT NULL,
	"MessageId" varchar(511) NULL,
	"BusinessId" varchar(511) NULL,
	"CorrelationId" varchar(511) NULL,
	"SessionId" uuid NULL,
	"SessionMessagePartId" bigint NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Publisher" varchar(511) NULL,
	"PublisherId" varchar(511) NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"LastProcessingTimeoutUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"TargetTopic" varchar(1023) NULL,
	"TargetQueueName" varchar(1023) NULL,
	"IdInboxInstance" uuid NOT NULL
);

CREATE TABLE inbox."InboxMessageContent"
(
	"IdInboxMessageContent" uuid NOT NULL,
	"MimeType" varchar(1023) NOT NULL,
	"ContentEncoding" varchar(63) NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL
);

CREATE TABLE inbox."InboxMessageProcessingLog"
(
	"IdInboxMessageProcessingLog" uuid NOT NULL,
	"IdInboxMessage" uuid NOT NULL,
	"IdInboxQueue" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdInboxMessageStatus" uuid NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL,
	"IdInboxInstance" uuid NOT NULL
);

CREATE TABLE inbox."InboxMessageStatus"
(
	"IdInboxMessageStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE inbox."InboxMessageType"
(
	"IdInboxMessageType" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdInboxInstance" uuid NOT NULL
);

CREATE TABLE inbox."InboxProcessingLog"
(
	"IdInboxProcessingLog" uuid NOT NULL,
	"IdInboxInstance" uuid NOT NULL,
	"IdInboxQueue" uuid NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL
);

CREATE TABLE inbox."InboxQueue"
(
	"IdInboxQueue" uuid NOT NULL,
	"Name" varchar(1023) NOT NULL,
	"ReceivedEventNamespace" varchar(1023) NOT NULL,
	"IdMessageType" uuid NULL,
	"IsActive" boolean NOT NULL,
	"IsSequentialFIFO" boolean NOT NULL,
	"MessagesBatchCount" integer NOT NULL,
	"MaxDegreeOfParallelism" integer NULL,
	"TimeoutForMessageProcessing" interval NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL,
	"Properties" jsonb NULL,
	"IdProcessingMode" uuid NOT NULL,
	"IdSuspendingMode" uuid NOT NULL,
	"IdInboxInstance" uuid NOT NULL
);

CREATE TABLE inbox."InboxQueueProcessingMode"
(
	"IdInboxQueueProcessingMode" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE mbox."Message"
(
	"IdMessage" uuid NOT NULL,
	"IdMessageType" uuid NOT NULL,
	"IdMessageStatus" uuid NOT NULL,
	"IdMessageContent" uuid NULL,
	"IdQueue" uuid NULL,
	"IdTopic" uuid NULL,
	"MessageId" varchar(511) NULL,
	"BusinessId" varchar(511) NULL,
	"CorrelationId" varchar(511) NULL,
	"SessionId" uuid NULL,
	"SessionMessagePartId" bigint NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Publisher" varchar(511) NULL,
	"PublisherId" varchar(511) NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ValidToUtc" timestamp with time zone NULL,
	"Priority" integer NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE mbox."MessageArchive"
(
	"IdMessage" uuid NOT NULL,
	"IdMessageType" uuid NOT NULL,
	"IdMessageStatus" uuid NOT NULL,
	"IdMessageContent" uuid NULL,
	"IdQueue" uuid NULL,
	"IdTopic" uuid NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"MessageId" varchar(511) NULL,
	"BusinessId" varchar(511) NULL,
	"CorrelationId" varchar(511) NULL,
	"SessionId" uuid NULL,
	"SessionMessagePartId" bigint NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Publisher" varchar(511) NULL,
	"PublisherId" varchar(511) NULL,
	"ValidToUtc" timestamp with time zone NULL,
	"Priority" integer NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE mbox."MessageBoxInstance"
(
	"IdMessageBoxInstance" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"Name" varchar(255) NOT NULL,
	"Version" varchar(15) NOT NULL,
	"MaxDegreeOfQueueParallelism" integer NOT NULL,
	"MaxDegreeOfTopicParallelism" integer NOT NULL,
	"IdLogLevel" integer NOT NULL
);

CREATE TABLE mbox."MessageBoxProcessingLog"
(
	"IdMessageBoxProcessingLog" uuid NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL,
	"IdQueue" uuid NULL,
	"IdTopic" uuid NULL,
	"IdTopicSubscription" uuid NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL
);

CREATE TABLE mbox."MessageContent"
(
	"IdMessageContent" uuid NOT NULL,
	"MimeType" varchar(1023) NOT NULL,
	"ContentEncoding" varchar(63) NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL
);

CREATE TABLE mbox."MessageProcessingLog"
(
	"IdMessageProcessingLog" uuid NOT NULL,
	"IdMessage" uuid NOT NULL,
	"IdQueuedMessage" uuid NULL,
	"IdSubscribedMessage" uuid NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdMessageProcessingStatus" uuid NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE mbox."MessageProcessingStatus"
(
	"IdMessageProcessingStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE mbox."MessageStatus"
(
	"IdMessageStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE mbox."MessageType"
(
	"IdMessageType" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE outbox."OutboxInstance"
(
	"IdOutboxInstance" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"Name" varchar(255) NOT NULL,
	"Version" varchar(15) NOT NULL,
	"MaxDegreeOfQueueParallelism" integer NOT NULL,
	"IdLogLevel" integer NOT NULL
);

CREATE TABLE outbox."OutboxMessage"
(
	"IdOutboxMessage" uuid NOT NULL,
	"IdMessageType" uuid NOT NULL,
	"IdOutboxMessageStatus" uuid NOT NULL,
	"IdMessageContent" uuid NULL,
	"IdOutboxQueue" uuid NOT NULL,
	"MessageId" varchar(511) NULL,
	"BusinessId" varchar(511) NULL,
	"CorrelationId" varchar(511) NULL,
	"SessionId" uuid NULL,
	"SessionMessagePartId" bigint NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Publisher" varchar(511) NULL,
	"PublisherId" varchar(511) NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"LastProcessingTimeoutUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"TargetTopic" varchar(1023) NULL,
	"TargetQueueName" varchar(1023) NULL,
	"IdOutboxInstance" uuid NOT NULL
);

CREATE TABLE outbox."OutboxMessageArchive"
(
	"IdOutboxMessage" uuid NOT NULL,
	"IdMessageType" uuid NOT NULL,
	"IdOutboxMessageStatus" uuid NOT NULL,
	"IdMessageContent" uuid NULL,
	"IdOutboxQueue" uuid NOT NULL,
	"MessageId" varchar(511) NULL,
	"BusinessId" varchar(511) NULL,
	"CorrelationId" varchar(511) NULL,
	"SessionId" uuid NULL,
	"SessionMessagePartId" bigint NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Publisher" varchar(511) NULL,
	"PublisherId" varchar(511) NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"LastProcessingTimeoutUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"TargetTopic" varchar(1023) NULL,
	"TargetQueueName" varchar(1023) NULL,
	"IdOutboxInstance" uuid NOT NULL
);

CREATE TABLE outbox."OutboxMessageContent"
(
	"IdOutboxMessageContent" uuid NOT NULL,
	"MimeType" varchar(1023) NOT NULL,
	"ContentEncoding" varchar(63) NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL
);

CREATE TABLE outbox."OutboxMessageProcessingLog"
(
	"IdOutboxMessageProcessingLog" uuid NOT NULL,
	"IdOutboxMessage" uuid NOT NULL,
	"IdOutboxQueue" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdOutboxMessageStatus" uuid NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL,
	"IdOutboxInstance" uuid NOT NULL
);

CREATE TABLE outbox."OutboxMessageStatus"
(
	"IdOutboxMessageStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE outbox."OutboxMessageType"
(
	"IdOutboxMessageType" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdOutboxInstance" uuid NOT NULL
);

CREATE TABLE outbox."OutboxProcessingLog"
(
	"IdOutboxProcessingLog" uuid NOT NULL,
	"IdOutboxInstance" uuid NOT NULL,
	"IdOutboxQueue" uuid NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL
);

CREATE TABLE outbox."OutboxQueue"
(
	"IdOutboxQueue" uuid NOT NULL,
	"Name" varchar(1023) NOT NULL,
	"ReceivedEventNamespace" varchar(1023) NOT NULL,
	"IdMessageType" uuid NULL,
	"IsActive" boolean NOT NULL,
	"IsSequentialFIFO" boolean NOT NULL,
	"MessagesBatchCount" integer NOT NULL,
	"MaxDegreeOfParallelism" integer NULL,
	"TimeoutForMessageProcessing" interval NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL,
	"Properties" jsonb NULL,
	"IdProcessingMode" uuid NOT NULL,
	"IdSuspendingMode" uuid NOT NULL,
	"IdOutboxInstance" uuid NOT NULL
);

CREATE TABLE outbox."OutboxQueueProcessingMode"
(
	"IdOutboxQueueProcessingMode" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE mbox."Queue"
(
	"IdQueue" uuid NOT NULL,
	"Name" varchar(1023) NOT NULL,
	"ReceivedEventNamespace" varchar(1023) NOT NULL,
	"IdMessageType" uuid NULL,
	"IsActive" boolean NOT NULL,
	"IsSequentialFIFO" boolean NOT NULL,
	"MessagesBatchCount" integer NOT NULL,
	"MaxDegreeOfParallelism" integer NULL,
	"TimeoutForMessageProcessing" interval NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL,
	"Properties" jsonb NULL,
	"IdProcessingMode" uuid NOT NULL,
	"IdSuspendingMode" uuid NOT NULL,
	"IdJob" uuid NULL,
	"IdOrchestration" uuid NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE mbox."QueuedMessage"
(
	"IdQueuedMessage" uuid NOT NULL,
	"IdQueue" uuid NOT NULL,
	"IdMessage" uuid NOT NULL,
	"IdMessageProcessingStatus" uuid NOT NULL,
	"AssignedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"LastProcessingTimeoutUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE mbox."QueueProcessingMode"
(
	"IdQueueProcessingMode" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE mbox."SubscribedMessage"
(
	"IdSubscribedMessage" uuid NOT NULL,
	"IdTopicSubscription" uuid NOT NULL,
	"IdMessage" uuid NOT NULL,
	"IdMessageProcessingStatus" uuid NOT NULL,
	"AssignedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"LastProcessingTimeoutUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE mbox."Topic"
(
	"IdTopic" uuid NOT NULL,
	"Name" varchar(1023) NOT NULL,
	"IsActive" boolean NOT NULL,
	"IsSequentialFIFO" boolean NOT NULL,
	"MessagesBatchCount" integer NOT NULL,
	"MaxDegreeOfParallelism" integer NULL,
	"TimeoutForMessageProcessing" interval NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL,
	"Properties" jsonb NULL,
	"IdProcessingMode" uuid NOT NULL,
	"IdSuspendingMode" uuid NOT NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

CREATE TABLE mbox."TopicSubscription"
(
	"IdTopicSubscription" uuid NOT NULL,
	"IdTopic" uuid NOT NULL,
	"SubscriptionName" varchar(511) NOT NULL,
	"ReceivedEventNamespace" varchar(1023) NOT NULL,
	"IsActive" boolean NOT NULL,
	"IsSequentialFIFO" boolean NOT NULL,
	"MessagesBatchCount" integer NOT NULL,
	"MaxDegreeOfParallelism" integer NULL,
	"TimeoutForMessageProcessing" interval NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL,
	"Properties" jsonb NULL,
	"IdProcessingMode" uuid NOT NULL,
	"IdSuspendingMode" uuid NOT NULL,
	"IdJob" uuid NULL,
	"IdOrchestration" uuid NULL,
	"IdMessageBoxInstance" uuid NOT NULL
);

ALTER TABLE devt."BlockedDomainEventType" ADD CONSTRAINT "PK_BlockedDomainEventType"
	PRIMARY KEY ("IdBlockedDomainEventType");

ALTER TABLE devt."BlockedDomainEventType" 
  ADD CONSTRAINT "UQ_BlockedDomainEventType_Namespace" UNIQUE ("Namespace");

ALTER TABLE inbox."BlockedInboxMessageType" ADD CONSTRAINT "PK_BlockedInboxMessageType"
	PRIMARY KEY ("IdBlockedInboxMessageType");

ALTER TABLE inbox."BlockedInboxMessageType" 
  ADD CONSTRAINT "UQ_BlockedInboxMessageType_Namespace" UNIQUE ("Namespace");

CREATE INDEX "IXFK_BlockedInboxMessageType_InboxInstance" ON inbox."BlockedInboxMessageType" ("IdInboxInstance" ASC);

ALTER TABLE mbox."BlockedMessageType" ADD CONSTRAINT "PK_BlockedMessageType"
	PRIMARY KEY ("IdBlockedMessageType");

ALTER TABLE mbox."BlockedMessageType" 
  ADD CONSTRAINT "UQ_BlockedMessageType_Namespace" UNIQUE ("Namespace");

CREATE INDEX "IXFK_BlockedMessageType_MessageBoxInstance" ON mbox."BlockedMessageType" ("IdMessageBoxInstance" ASC);

ALTER TABLE outbox."BlockedOutboxMessageType" ADD CONSTRAINT "PK_BlockedOutboxMessageType"
	PRIMARY KEY ("IdBlockedOutboxMessageType");

ALTER TABLE outbox."BlockedOutboxMessageType" 
  ADD CONSTRAINT "UQ_BlockedOutboxMessageType_Namespace" UNIQUE ("Namespace");

CREATE INDEX "IXFK_BlockedOutboxMessageType_OutboxInstance" ON outbox."BlockedOutboxMessageType" ("IdOutboxInstance" ASC);

ALTER TABLE devt."DomainEvent" ADD CONSTRAINT "PK_DomainEvent"
	PRIMARY KEY ("IdDomainEvent");

ALTER TABLE devt."DomainEvent" 
  ADD CONSTRAINT "UQ_DomainEvent_IdContent" UNIQUE ("IdContent");

CREATE INDEX "IXFK_DomainEvent_DomainEventContent" ON devt."DomainEvent" ("IdContent" ASC);

CREATE INDEX "IXFK_DomainEvent_DomainEventProcessingStatus" ON devt."DomainEvent" ("IdDomainEventProcessingStatus" ASC);

ALTER TABLE devt."DomainEventContent" ADD CONSTRAINT "PK_DomainEventContent"
	PRIMARY KEY ("IdDomainEventContent");

ALTER TABLE devt."DomainEventProcessingLog" ADD CONSTRAINT "PK_DomainEventProcessingLog"
	PRIMARY KEY ("IdDomainEventProcessingLog");

CREATE INDEX "IXFK_DomainEventProcessingLog_DomainEvent" ON devt."DomainEventProcessingLog" ("IdDomainEvent" ASC);

CREATE INDEX "IXFK_DomainEventProcessingLog_DomainEventProcessingStatus" ON devt."DomainEventProcessingLog" ("IdDomainEventProcessingStatus" ASC);

ALTER TABLE devt."DomainEventProcessingStatus" ADD CONSTRAINT "PK_DomainEventProcessingStatus"
	PRIMARY KEY ("IdDomainEventProcessingStatus");

ALTER TABLE inbox."InboxInstance" ADD CONSTRAINT "PK_InboxProcessorInstance"
	PRIMARY KEY ("IdInboxInstance");

ALTER TABLE inbox."InboxMessage" ADD CONSTRAINT "PK_InboxMessage"
	PRIMARY KEY ("IdInboxMessage");

ALTER TABLE inbox."InboxMessage" 
  ADD CONSTRAINT "UQ_InboxMessage_IdMessageContent" UNIQUE ("IdMessageContent");

CREATE INDEX "IXFK_InboxMessage_InboxInstance" ON inbox."InboxMessage" ("IdInboxInstance" ASC);

CREATE INDEX "IXFK_InboxMessage_InboxMessageContent" ON inbox."InboxMessage" ("IdMessageContent" ASC);

CREATE INDEX "IXFK_InboxMessage_InboxMessageStatus" ON inbox."InboxMessage" ("IdInboxMessageStatus" ASC);

CREATE INDEX "IXFK_InboxMessage_InboxQueue" ON inbox."InboxMessage" ("IdInboxQueue" ASC);

CREATE INDEX "IXFK_InboxMessage_MessageType" ON inbox."InboxMessage" ("IdMessageType" ASC);

ALTER TABLE inbox."InboxMessageArchive" ADD CONSTRAINT "PK_InboxMessageArchive"
	PRIMARY KEY ("IdInboxMessage");

ALTER TABLE inbox."InboxMessageArchive" 
  ADD CONSTRAINT "UQ_InboxMessageArchive_IdMessageContent" UNIQUE ("IdMessageContent");

CREATE INDEX "IXFK_InboxMessageArchive_InboxInstance" ON inbox."InboxMessageArchive" ("IdInboxInstance" ASC);

CREATE INDEX "IXFK_InboxMessageArchive_InboxMessageContent" ON inbox."InboxMessageArchive" ("IdMessageContent" ASC);

CREATE INDEX "IXFK_InboxMessageArchive_InboxMessageStatus" ON inbox."InboxMessageArchive" ("IdInboxMessageStatus" ASC);

CREATE INDEX "IXFK_InboxMessageArchive_InboxQueue" ON inbox."InboxMessageArchive" ("IdInboxQueue" ASC);

CREATE INDEX "IXFK_InboxMessageArchive_MessageType" ON inbox."InboxMessageArchive" ("IdMessageType" ASC);

ALTER TABLE inbox."InboxMessageContent" ADD CONSTRAINT "PK_InboxMessageContent"
	PRIMARY KEY ("IdInboxMessageContent");

ALTER TABLE inbox."InboxMessageProcessingLog" ADD CONSTRAINT "PK_InboxMessageProcessingLog"
	PRIMARY KEY ("IdInboxMessageProcessingLog");

CREATE INDEX "IXFK_InboxMessageProcessingLog_InboxInstance" ON inbox."InboxMessageProcessingLog" ("IdInboxInstance" ASC);

CREATE INDEX "IXFK_InboxMessageProcessingLog_InboxMessageStatus" ON inbox."InboxMessageProcessingLog" ("IdInboxMessageStatus" ASC);

CREATE INDEX "IXFK_InboxMessageProcessingLog_InboxQueue" ON inbox."InboxMessageProcessingLog" ("IdInboxQueue" ASC);

CREATE INDEX "IX_InboxMessageProcessingLog_IdInboxMessage" ON inbox."InboxMessageProcessingLog" ("IdInboxMessage" ASC);

ALTER TABLE inbox."InboxMessageStatus" ADD CONSTRAINT "PK_InboxMessageStatus"
	PRIMARY KEY ("IdInboxMessageStatus");

ALTER TABLE inbox."InboxMessageType" ADD CONSTRAINT "PK_InboxMessageType"
	PRIMARY KEY ("IdInboxMessageType");

ALTER TABLE inbox."InboxMessageType" 
  ADD CONSTRAINT "UQ_InboxMessageType_Namespace" UNIQUE ("Namespace");

CREATE INDEX "IXFK_InboxMessageType_InboxInstance" ON inbox."InboxMessageType" ("IdInboxInstance" ASC);

ALTER TABLE inbox."InboxProcessingLog" ADD CONSTRAINT "PK_InboxProcessingLog"
	PRIMARY KEY ("IdInboxProcessingLog");

CREATE INDEX "IXFK_InboxProcessingLog_InboxInstance" ON inbox."InboxProcessingLog" ("IdInboxInstance" ASC);

CREATE INDEX "IXFK_InboxProcessingLog_InboxQueue" ON inbox."InboxProcessingLog" ("IdInboxQueue" ASC);

ALTER TABLE inbox."InboxQueue" ADD CONSTRAINT "PK_InboxQueue"
	PRIMARY KEY ("IdInboxQueue");

ALTER TABLE inbox."InboxQueue" 
  ADD CONSTRAINT "UQ_InboxQueue_Name" UNIQUE ("Name");

ALTER TABLE inbox."InboxQueue" 
  ADD CONSTRAINT "UQ_InboxQueue_ReceivedEventNamespace" UNIQUE ("ReceivedEventNamespace");

CREATE INDEX "IXFK_InboxQueue_InboxInstance" ON inbox."InboxQueue" ("IdInboxInstance" ASC);

CREATE INDEX "IXFK_InboxQueue_InboxQueueProcessingMode" ON inbox."InboxQueue" ("IdSuspendingMode" ASC);

CREATE INDEX "IXFK_InboxQueue_InboxQueueProcessingMode_02" ON inbox."InboxQueue" ("IdProcessingMode" ASC);

CREATE INDEX "IXFK_InboxQueue_MessageType" ON inbox."InboxQueue" ("IdMessageType" ASC);

ALTER TABLE inbox."InboxQueueProcessingMode" ADD CONSTRAINT "PK_InboxQueueProcessingMode"
	PRIMARY KEY ("IdInboxQueueProcessingMode");

ALTER TABLE mbox."Message" ADD CONSTRAINT "PK_Message"
	PRIMARY KEY ("IdMessage");

ALTER TABLE mbox."Message" 
  ADD CONSTRAINT "UQ_Message_IdMessageContent" UNIQUE ("IdMessageContent");

CREATE INDEX "IXFK_Message_MessageBoxInstance" ON mbox."Message" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_Message_MessageContent" ON mbox."Message" ("IdMessageContent" ASC);

CREATE INDEX "IXFK_Message_MessageStatus" ON mbox."Message" ("IdMessageStatus" ASC);

CREATE INDEX "IXFK_Message_MessageType" ON mbox."Message" ("IdMessageType" ASC);

CREATE INDEX "IXFK_Message_Queue" ON mbox."Message" ("IdQueue" ASC);

CREATE INDEX "IXFK_Message_Topic" ON mbox."Message" ("IdTopic" ASC);

ALTER TABLE mbox."MessageArchive" ADD CONSTRAINT "PK_MessageArchive"
	PRIMARY KEY ("IdMessage");

ALTER TABLE mbox."MessageArchive" 
  ADD CONSTRAINT "UQ_MessageArchive_IdMessageContent" UNIQUE ("IdMessageContent");

CREATE INDEX "IXFK_MessageArchive_MessageBoxInstance" ON mbox."MessageArchive" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_MessageArchive_MessageContent" ON mbox."MessageArchive" ("IdMessageContent" ASC);

CREATE INDEX "IXFK_MessageArchive_MessageStatus" ON mbox."MessageArchive" ("IdMessageStatus" ASC);

CREATE INDEX "IXFK_MessageArchive_MessageType" ON mbox."MessageArchive" ("IdMessageType" ASC);

CREATE INDEX "IXFK_MessageArchive_Queue" ON mbox."MessageArchive" ("IdQueue" ASC);

CREATE INDEX "IXFK_MessageArchive_Topic" ON mbox."MessageArchive" ("IdTopic" ASC);

ALTER TABLE mbox."MessageBoxInstance" ADD CONSTRAINT "PK_IdBlockedMessageBoxMessageType"
	PRIMARY KEY ("IdMessageBoxInstance");

ALTER TABLE mbox."MessageBoxProcessingLog" ADD CONSTRAINT "PK_MessageBoxProcessingLog"
	PRIMARY KEY ("IdMessageBoxProcessingLog");

CREATE INDEX "IXFK_MessageBoxProcessingLog_MessageBoxInstance" ON mbox."MessageBoxProcessingLog" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_MessageBoxProcessingLog_Queue" ON mbox."MessageBoxProcessingLog" ("IdQueue" ASC);

CREATE INDEX "IXFK_MessageBoxProcessingLog_Topic" ON mbox."MessageBoxProcessingLog" ("IdTopic" ASC);

CREATE INDEX "IXFK_MessageBoxProcessingLog_TopicSubscription" ON mbox."MessageBoxProcessingLog" ("IdTopicSubscription" ASC);

ALTER TABLE mbox."MessageContent" ADD CONSTRAINT "PK_MessageContent"
	PRIMARY KEY ("IdMessageContent");

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "PK_MessageProcessingLog"
	PRIMARY KEY ("IdMessageProcessingLog");

CREATE INDEX "IXFK_MessageProcessingLog_MessageBoxInstance" ON mbox."MessageProcessingLog" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_MessageProcessingLog_MessageProcessingStatus" ON mbox."MessageProcessingLog" ("IdMessageProcessingStatus" ASC);

CREATE INDEX "IXFK_MessageProcessingLog_QueuedMessage" ON mbox."MessageProcessingLog" ("IdQueuedMessage" ASC);

CREATE INDEX "IXFK_MessageProcessingLog_SubscribedMessage" ON mbox."MessageProcessingLog" ("IdSubscribedMessage" ASC);

CREATE INDEX "IX_MessageProcessingLog_IdMessage" ON mbox."MessageProcessingLog" ("IdMessage" ASC);

ALTER TABLE mbox."MessageProcessingStatus" ADD CONSTRAINT "PK_MessageProcessingStatus"
	PRIMARY KEY ("IdMessageProcessingStatus");

ALTER TABLE mbox."MessageStatus" ADD CONSTRAINT "PK_MessageStatus"
	PRIMARY KEY ("IdMessageStatus");

ALTER TABLE mbox."MessageType" ADD CONSTRAINT "PK_MessageType"
	PRIMARY KEY ("IdMessageType");

ALTER TABLE mbox."MessageType" 
  ADD CONSTRAINT "UQ_MessageType_Namespace" UNIQUE ("Namespace");

CREATE INDEX "IXFK_MessageType_MessageBoxInstance" ON mbox."MessageType" ("IdMessageBoxInstance" ASC);

ALTER TABLE outbox."OutboxInstance" ADD CONSTRAINT "PK_OutboxInstance"
	PRIMARY KEY ("IdOutboxInstance");

ALTER TABLE outbox."OutboxMessage" ADD CONSTRAINT "PK_OutboxMessage"
	PRIMARY KEY ("IdOutboxMessage");

ALTER TABLE outbox."OutboxMessage" 
  ADD CONSTRAINT "UQ_OutboxMessage_IdMessageContent" UNIQUE ("IdMessageContent");

CREATE INDEX "IXFK_OutboxMessage_MessageType" ON outbox."OutboxMessage" ("IdMessageType" ASC);

CREATE INDEX "IXFK_OutboxMessage_OutboxInstance" ON outbox."OutboxMessage" ("IdOutboxInstance" ASC);

CREATE INDEX "IXFK_OutboxMessage_OutboxMessageContent" ON outbox."OutboxMessage" ("IdMessageContent" ASC);

CREATE INDEX "IXFK_OutboxMessage_OutboxMessageStatus" ON outbox."OutboxMessage" ("IdOutboxMessageStatus" ASC);

CREATE INDEX "IXFK_OutboxMessage_OutboxQueue" ON outbox."OutboxMessage" ("IdOutboxQueue" ASC);

ALTER TABLE outbox."OutboxMessageArchive" ADD CONSTRAINT "PK_OutboxMessageArchive"
	PRIMARY KEY ("IdOutboxMessage");

ALTER TABLE outbox."OutboxMessageArchive" 
  ADD CONSTRAINT "UQ_OutboxMessageArchive_IdMessageContent" UNIQUE ("IdMessageContent");

CREATE INDEX "IXFK_OutboxMessageArchive_MessageType" ON outbox."OutboxMessageArchive" ("IdMessageType" ASC);

CREATE INDEX "IXFK_OutboxMessageArchive_OutboxInstance" ON outbox."OutboxMessageArchive" ("IdOutboxInstance" ASC);

CREATE INDEX "IXFK_OutboxMessageArchive_OutboxMessageContent" ON outbox."OutboxMessageArchive" ("IdMessageContent" ASC);

CREATE INDEX "IXFK_OutboxMessageArchive_OutboxMessageStatus" ON outbox."OutboxMessageArchive" ("IdOutboxMessageStatus" ASC);

CREATE INDEX "IXFK_OutboxMessageArchive_OutboxQueue" ON outbox."OutboxMessageArchive" ("IdOutboxQueue" ASC);

ALTER TABLE outbox."OutboxMessageContent" ADD CONSTRAINT "PK_OutboxMessageContent"
	PRIMARY KEY ("IdOutboxMessageContent");

ALTER TABLE outbox."OutboxMessageProcessingLog" ADD CONSTRAINT "PK_OutboxMessageProcessingLog"
	PRIMARY KEY ("IdOutboxMessageProcessingLog");

CREATE INDEX "IXFK_OutboxMessageProcessingLog_OutboxInstance" ON outbox."OutboxMessageProcessingLog" ("IdOutboxInstance" ASC);

CREATE INDEX "IXFK_OutboxMessageProcessingLog_OutboxMessageStatus" ON outbox."OutboxMessageProcessingLog" ("IdOutboxMessageStatus" ASC);

CREATE INDEX "IXFK_OutboxMessageProcessingLog_OutboxQueue" ON outbox."OutboxMessageProcessingLog" ("IdOutboxQueue" ASC);

CREATE INDEX "IX_OutboxMessageProcessingLog_IdOutboxMessage" ON outbox."OutboxMessageProcessingLog" ("IdOutboxMessage" ASC);

ALTER TABLE outbox."OutboxMessageStatus" ADD CONSTRAINT "PK_OutboxMessageStatus"
	PRIMARY KEY ("IdOutboxMessageStatus");

ALTER TABLE outbox."OutboxMessageType" ADD CONSTRAINT "PK_OutboxMessageType"
	PRIMARY KEY ("IdOutboxMessageType");

ALTER TABLE outbox."OutboxMessageType" 
  ADD CONSTRAINT "UQ_OutboxMessageType_Namespace" UNIQUE ("Namespace");

CREATE INDEX "IXFK_OutboxMessageType_OutboxInstance" ON outbox."OutboxMessageType" ("IdOutboxInstance" ASC);

ALTER TABLE outbox."OutboxProcessingLog" ADD CONSTRAINT "PK_OutboxProcessingLog"
	PRIMARY KEY ("IdOutboxProcessingLog");

CREATE INDEX "IXFK_OutboxProcessingLog_OutboxInstance" ON outbox."OutboxProcessingLog" ("IdOutboxInstance" ASC);

CREATE INDEX "IXFK_OutboxProcessingLog_OutboxQueue" ON outbox."OutboxProcessingLog" ("IdOutboxQueue" ASC);

ALTER TABLE outbox."OutboxQueue" ADD CONSTRAINT "PK_OutboxQueue"
	PRIMARY KEY ("IdOutboxQueue");

ALTER TABLE outbox."OutboxQueue" 
  ADD CONSTRAINT "UQ_OutboxQueue_Name" UNIQUE ("Name");

ALTER TABLE outbox."OutboxQueue" 
  ADD CONSTRAINT "UQ_OutboxQueue_ReceivedEventNamespace" UNIQUE ("ReceivedEventNamespace");

CREATE INDEX "IXFK_OutboxQueue_MessageType" ON outbox."OutboxQueue" ("IdMessageType" ASC);

CREATE INDEX "IXFK_OutboxQueue_OutboxInstance" ON outbox."OutboxQueue" ("IdOutboxInstance" ASC);

CREATE INDEX "IXFK_OutboxQueue_OutboxQueueProcessingMode" ON outbox."OutboxQueue" ("IdProcessingMode" ASC);

CREATE INDEX "IXFK_OutboxQueue_OutboxQueueProcessingMode_02" ON outbox."OutboxQueue" ("IdSuspendingMode" ASC);

ALTER TABLE outbox."OutboxQueueProcessingMode" ADD CONSTRAINT "PK_OutboxQueueProcessingMode"
	PRIMARY KEY ("IdOutboxQueueProcessingMode");

ALTER TABLE mbox."Queue" ADD CONSTRAINT "PK_Queue"
	PRIMARY KEY ("IdQueue");

ALTER TABLE mbox."Queue" 
  ADD CONSTRAINT "UQ_Queue_Name" UNIQUE ("Name");

ALTER TABLE mbox."Queue" 
  ADD CONSTRAINT "UQ_Queue_ReceivedEventNamespace" UNIQUE ("ReceivedEventNamespace");

CREATE INDEX "IXFK_Queue_MessageBoxInstance" ON mbox."Queue" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_Queue_MessageType" ON mbox."Queue" ("IdMessageType" ASC);

CREATE INDEX "IXFK_Queue_QueueProcessingMode" ON mbox."Queue" ("IdProcessingMode" ASC);

CREATE INDEX "IXFK_Queue_QueueProcessingMode_02" ON mbox."Queue" ("IdSuspendingMode" ASC);

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "PK_QueuedMessage"
	PRIMARY KEY ("IdQueuedMessage");

CREATE INDEX "IXFK_QueuedMessage_MessageBoxInstance" ON mbox."QueuedMessage" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_QueuedMessage_MessageProcessingStatus" ON mbox."QueuedMessage" ("IdMessageProcessingStatus" ASC);

CREATE INDEX "IXFK_QueuedMessage_Queue" ON mbox."QueuedMessage" ("IdQueue" ASC);

ALTER TABLE mbox."QueueProcessingMode" ADD CONSTRAINT "PK_QueueProcessingMode"
	PRIMARY KEY ("IdQueueProcessingMode");

ALTER TABLE mbox."SubscribedMessage" ADD CONSTRAINT "PK_SubscribedMessage"
	PRIMARY KEY ("IdSubscribedMessage");

CREATE INDEX "IXFK_SubscribedMessage_MessageBoxInstance" ON mbox."SubscribedMessage" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_SubscribedMessage_MessageProcessingStatus" ON mbox."SubscribedMessage" ("IdMessageProcessingStatus" ASC);

CREATE INDEX "IXFK_SubscribedMessage_TopicSubscription" ON mbox."SubscribedMessage" ("IdTopicSubscription" ASC);

ALTER TABLE mbox."Topic" ADD CONSTRAINT "PK_Topic"
	PRIMARY KEY ("IdTopic");

ALTER TABLE mbox."Topic" 
  ADD CONSTRAINT "UQ_Topic_name" UNIQUE ("Name");

CREATE INDEX "IXFK_Topic_MessageBoxInstance" ON mbox."Topic" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_Topic_QueueProcessingMode" ON mbox."Topic" ("IdProcessingMode" ASC);

CREATE INDEX "IXFK_Topic_QueueProcessingMode_02" ON mbox."Topic" ("IdSuspendingMode" ASC);

ALTER TABLE mbox."TopicSubscription" ADD CONSTRAINT "PK_TopicSubscription"
	PRIMARY KEY ("IdTopicSubscription");

ALTER TABLE mbox."TopicSubscription" 
  ADD CONSTRAINT "UQ_TopicSubscription_IdTopic_SubscriptionName" UNIQUE ("IdTopic","SubscriptionName");

ALTER TABLE mbox."TopicSubscription" 
  ADD CONSTRAINT "UQ_TopicSubscription_ReceivedEventNamespace" UNIQUE ("ReceivedEventNamespace");

CREATE INDEX "IXFK_TopicSubscription_MessageBoxInstance" ON mbox."TopicSubscription" ("IdMessageBoxInstance" ASC);

CREATE INDEX "IXFK_TopicSubscription_QueueProcessingMode" ON mbox."TopicSubscription" ("IdProcessingMode" ASC);

CREATE INDEX "IXFK_TopicSubscription_QueueProcessingMode_02" ON mbox."TopicSubscription" ("IdSuspendingMode" ASC);

CREATE INDEX "IXFK_TopicSubscription_Topic" ON mbox."TopicSubscription" ("IdTopic" ASC);

ALTER TABLE inbox."BlockedInboxMessageType" ADD CONSTRAINT "FK_BlockedInboxMessageType_InboxInstance"
	FOREIGN KEY ("IdInboxInstance") REFERENCES inbox."InboxInstance" ("IdInboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."BlockedMessageType" ADD CONSTRAINT "FK_BlockedMessageType_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."BlockedOutboxMessageType" ADD CONSTRAINT "FK_BlockedOutboxMessageType_OutboxInstance"
	FOREIGN KEY ("IdOutboxInstance") REFERENCES outbox."OutboxInstance" ("IdOutboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE devt."DomainEvent" ADD CONSTRAINT "FK_DomainEvent_IdDomainEventContent"
	FOREIGN KEY ("IdContent") REFERENCES devt."DomainEventContent" ("IdDomainEventContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE devt."DomainEvent" ADD CONSTRAINT "FK_DomainEvent_IdDomainEventProcessingStatus"
	FOREIGN KEY ("IdDomainEventProcessingStatus") REFERENCES devt."DomainEventProcessingStatus" ("IdDomainEventProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE devt."DomainEventProcessingLog" ADD CONSTRAINT "FK_DomainEventProcessingLog_IdDomainEvent"
	FOREIGN KEY ("IdDomainEvent") REFERENCES devt."DomainEvent" ("IdDomainEvent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE devt."DomainEventProcessingLog" ADD CONSTRAINT "FK_DomainEventProcessingLog_IdDomainEventProcessingStatus"
	FOREIGN KEY ("IdDomainEventProcessingStatus") REFERENCES devt."DomainEventProcessingStatus" ("IdDomainEventProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessage" ADD CONSTRAINT "FK_InboxMessage_IdInboxInstance"
	FOREIGN KEY ("IdInboxInstance") REFERENCES inbox."InboxInstance" ("IdInboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessage" ADD CONSTRAINT "FK_InboxMessage_IdInboxMessageStatus"
	FOREIGN KEY ("IdInboxMessageStatus") REFERENCES inbox."InboxMessageStatus" ("IdInboxMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessage" ADD CONSTRAINT "FK_InboxMessage_IdInboxQueue"
	FOREIGN KEY ("IdInboxQueue") REFERENCES inbox."InboxQueue" ("IdInboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessage" ADD CONSTRAINT "FK_InboxMessage_IdMessageContent"
	FOREIGN KEY ("IdMessageContent") REFERENCES inbox."InboxMessageContent" ("IdInboxMessageContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessage" ADD CONSTRAINT "FK_InboxMessage_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES inbox."InboxMessageType" ("IdInboxMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageArchive" ADD CONSTRAINT "FK_InboxMessageArchive_IdInboxInstance"
	FOREIGN KEY ("IdInboxInstance") REFERENCES inbox."InboxInstance" ("IdInboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageArchive" ADD CONSTRAINT "FK_InboxMessageArchive_IdInboxMessageStatus"
	FOREIGN KEY ("IdInboxMessageStatus") REFERENCES inbox."InboxMessageStatus" ("IdInboxMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageArchive" ADD CONSTRAINT "FK_InboxMessageArchive_IdInboxQueue"
	FOREIGN KEY ("IdInboxQueue") REFERENCES inbox."InboxQueue" ("IdInboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageArchive" ADD CONSTRAINT "FK_InboxMessageArchive_IdMessageContent"
	FOREIGN KEY ("IdMessageContent") REFERENCES inbox."InboxMessageContent" ("IdInboxMessageContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageArchive" ADD CONSTRAINT "FK_InboxMessageArchive_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES inbox."InboxMessageType" ("IdInboxMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageProcessingLog" ADD CONSTRAINT "FK_InboxMessageProcessingLog_IdInboxInstance"
	FOREIGN KEY ("IdInboxInstance") REFERENCES inbox."InboxInstance" ("IdInboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageProcessingLog" ADD CONSTRAINT "FK_InboxMessageProcessingLog_IdInboxMessageStatus"
	FOREIGN KEY ("IdInboxMessageStatus") REFERENCES inbox."InboxMessageStatus" ("IdInboxMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageProcessingLog" ADD CONSTRAINT "FK_InboxMessageProcessingLog_IdInboxQueue"
	FOREIGN KEY ("IdInboxQueue") REFERENCES inbox."InboxQueue" ("IdInboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxMessageType" ADD CONSTRAINT "FK_InboxMessageType_IdInboxInstance"
	FOREIGN KEY ("IdInboxInstance") REFERENCES inbox."InboxInstance" ("IdInboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxProcessingLog" ADD CONSTRAINT "FK_InboxProcessingLog_IdInboxInstance"
	FOREIGN KEY ("IdInboxInstance") REFERENCES inbox."InboxInstance" ("IdInboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxProcessingLog" ADD CONSTRAINT "FK_InboxProcessingLog_IdInboxQueue"
	FOREIGN KEY ("IdInboxQueue") REFERENCES inbox."InboxQueue" ("IdInboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxQueue" ADD CONSTRAINT "FK_InboxQueue_IdInboxInstance"
	FOREIGN KEY ("IdInboxInstance") REFERENCES inbox."InboxInstance" ("IdInboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxQueue" ADD CONSTRAINT "FK_InboxQueue_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES inbox."InboxMessageType" ("IdInboxMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxQueue" ADD CONSTRAINT "FK_InboxQueue_IdProcessingMode"
	FOREIGN KEY ("IdProcessingMode") REFERENCES inbox."InboxQueueProcessingMode" ("IdInboxQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE inbox."InboxQueue" ADD CONSTRAINT "FK_InboxQueue_IdSuspendingMode"
	FOREIGN KEY ("IdSuspendingMode") REFERENCES inbox."InboxQueueProcessingMode" ("IdInboxQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdMessageContent"
	FOREIGN KEY ("IdMessageContent") REFERENCES mbox."MessageContent" ("IdMessageContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdMessageStatus"
	FOREIGN KEY ("IdMessageStatus") REFERENCES mbox."MessageStatus" ("IdMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES mbox."MessageType" ("IdMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdQueue"
	FOREIGN KEY ("IdQueue") REFERENCES mbox."Queue" ("IdQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdTopic"
	FOREIGN KEY ("IdTopic") REFERENCES mbox."Topic" ("IdTopic") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageArchive" ADD CONSTRAINT "FK_MessageArchive_IdMessageContent"
	FOREIGN KEY ("IdMessageContent") REFERENCES mbox."MessageContent" ("IdMessageContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageArchive" ADD CONSTRAINT "FK_MessageArchive_IdMessageStatus"
	FOREIGN KEY ("IdMessageStatus") REFERENCES mbox."MessageStatus" ("IdMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageArchive" ADD CONSTRAINT "FK_MessageArchive_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES mbox."MessageType" ("IdMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageArchive" ADD CONSTRAINT "FK_MessageArchive_IdQueue"
	FOREIGN KEY ("IdQueue") REFERENCES mbox."Queue" ("IdQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageArchive" ADD CONSTRAINT "FK_MessageArchive_IdTopic"
	FOREIGN KEY ("IdTopic") REFERENCES mbox."Topic" ("IdTopic") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageArchive" ADD CONSTRAINT "FK_MessageArchive_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageBoxProcessingLog" ADD CONSTRAINT "FK_MessageBoxProcessingLog_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageBoxProcessingLog" ADD CONSTRAINT "FK_MessageBoxProcessingLog_Queue"
	FOREIGN KEY ("IdQueue") REFERENCES mbox."Queue" ("IdQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageBoxProcessingLog" ADD CONSTRAINT "FK_MessageBoxProcessingLog_Topic"
	FOREIGN KEY ("IdTopic") REFERENCES mbox."Topic" ("IdTopic") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageBoxProcessingLog" ADD CONSTRAINT "FK_MessageBoxProcessingLog_TopicSubscription"
	FOREIGN KEY ("IdTopicSubscription") REFERENCES mbox."TopicSubscription" ("IdTopicSubscription") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "FK_MessageProcessingLog_IdMessageProcessingStatus"
	FOREIGN KEY ("IdMessageProcessingStatus") REFERENCES mbox."MessageProcessingStatus" ("IdMessageProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "FK_MessageProcessingLog_IdQueuedMessage"
	FOREIGN KEY ("IdQueuedMessage") REFERENCES mbox."QueuedMessage" ("IdQueuedMessage") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "FK_MessageProcessingLog_IdSubscribedMessage"
	FOREIGN KEY ("IdSubscribedMessage") REFERENCES mbox."SubscribedMessage" ("IdSubscribedMessage") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "FK_MessageProcessingLog_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageType" ADD CONSTRAINT "FK_MessageType_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessage" ADD CONSTRAINT "FK_OutboxMessage_IdMessageContent"
	FOREIGN KEY ("IdMessageContent") REFERENCES outbox."OutboxMessageContent" ("IdOutboxMessageContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessage" ADD CONSTRAINT "FK_OutboxMessage_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES outbox."OutboxMessageType" ("IdOutboxMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessage" ADD CONSTRAINT "FK_OutboxMessage_IdOutboxInstance"
	FOREIGN KEY ("IdOutboxInstance") REFERENCES outbox."OutboxInstance" ("IdOutboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessage" ADD CONSTRAINT "FK_OutboxMessage_IdOutboxMessageStatus"
	FOREIGN KEY ("IdOutboxMessageStatus") REFERENCES outbox."OutboxMessageStatus" ("IdOutboxMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessage" ADD CONSTRAINT "FK_OutboxMessage_IdOutboxQueue"
	FOREIGN KEY ("IdOutboxQueue") REFERENCES outbox."OutboxQueue" ("IdOutboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageArchive" ADD CONSTRAINT "FK_OutboxMessageArchive_IdMessageContent"
	FOREIGN KEY ("IdMessageContent") REFERENCES outbox."OutboxMessageContent" ("IdOutboxMessageContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageArchive" ADD CONSTRAINT "FK_OutboxMessageArchive_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES outbox."OutboxMessageType" ("IdOutboxMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageArchive" ADD CONSTRAINT "FK_OutboxMessageArchive_IdOutboxInstance"
	FOREIGN KEY ("IdOutboxInstance") REFERENCES outbox."OutboxInstance" ("IdOutboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageArchive" ADD CONSTRAINT "FK_OutboxMessageArchive_IdOutboxMessageStatus"
	FOREIGN KEY ("IdOutboxMessageStatus") REFERENCES outbox."OutboxMessageStatus" ("IdOutboxMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageArchive" ADD CONSTRAINT "FK_OutboxMessageArchive_IdOutboxQueue"
	FOREIGN KEY ("IdOutboxQueue") REFERENCES outbox."OutboxQueue" ("IdOutboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageProcessingLog" ADD CONSTRAINT "FK_OutboxMessageProcessingLog_IdOutboxInstance"
	FOREIGN KEY ("IdOutboxInstance") REFERENCES outbox."OutboxInstance" ("IdOutboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageProcessingLog" ADD CONSTRAINT "FK_OutboxMessageProcessingLog_IdOutboxMessageStatus"
	FOREIGN KEY ("IdOutboxMessageStatus") REFERENCES outbox."OutboxMessageStatus" ("IdOutboxMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageProcessingLog" ADD CONSTRAINT "FK_OutboxMessageProcessingLog_IdOutboxQueue"
	FOREIGN KEY ("IdOutboxQueue") REFERENCES outbox."OutboxQueue" ("IdOutboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxMessageType" ADD CONSTRAINT "FK_OutboxMessageType_IdOutboxInstance"
	FOREIGN KEY ("IdOutboxInstance") REFERENCES outbox."OutboxInstance" ("IdOutboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxProcessingLog" ADD CONSTRAINT "FK_OutboxProcessingLog_IdOutboxInstance"
	FOREIGN KEY ("IdOutboxInstance") REFERENCES outbox."OutboxInstance" ("IdOutboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxProcessingLog" ADD CONSTRAINT "FK_OutboxProcessingLog_IdOutboxQueue"
	FOREIGN KEY ("IdOutboxQueue") REFERENCES outbox."OutboxQueue" ("IdOutboxQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxQueue" ADD CONSTRAINT "FK_OutboxQueue_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES outbox."OutboxMessageType" ("IdOutboxMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxQueue" ADD CONSTRAINT "FK_OutboxQueue_IdOutboxInstance"
	FOREIGN KEY ("IdOutboxInstance") REFERENCES outbox."OutboxInstance" ("IdOutboxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxQueue" ADD CONSTRAINT "FK_OutboxQueue_IdProcessingMode"
	FOREIGN KEY ("IdProcessingMode") REFERENCES outbox."OutboxQueueProcessingMode" ("IdOutboxQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE outbox."OutboxQueue" ADD CONSTRAINT "FK_OutboxQueue_IdSuspendingMode"
	FOREIGN KEY ("IdSuspendingMode") REFERENCES outbox."OutboxQueueProcessingMode" ("IdOutboxQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES mbox."MessageType" ("IdMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_IdProcessingMode"
	FOREIGN KEY ("IdProcessingMode") REFERENCES mbox."QueueProcessingMode" ("IdQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_IdSuspendingMode"
	FOREIGN KEY ("IdSuspendingMode") REFERENCES mbox."QueueProcessingMode" ("IdQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "FK_QueuedMessage_IdMessageProcessingStatus"
	FOREIGN KEY ("IdMessageProcessingStatus") REFERENCES mbox."MessageProcessingStatus" ("IdMessageProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "FK_QueuedMessage_IdQueue"
	FOREIGN KEY ("IdQueue") REFERENCES mbox."Queue" ("IdQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "FK_QueuedMessage_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."SubscribedMessage" ADD CONSTRAINT "FK_SubscribedMessage_IdMessageProcessingStatus"
	FOREIGN KEY ("IdMessageProcessingStatus") REFERENCES mbox."MessageProcessingStatus" ("IdMessageProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."SubscribedMessage" ADD CONSTRAINT "FK_SubscribedMessage_IdTopicSubscription"
	FOREIGN KEY ("IdTopicSubscription") REFERENCES mbox."TopicSubscription" ("IdTopicSubscription") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."SubscribedMessage" ADD CONSTRAINT "FK_SubscribedMessage_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Topic" ADD CONSTRAINT "FK_Topic_IdProcessingMode"
	FOREIGN KEY ("IdProcessingMode") REFERENCES mbox."QueueProcessingMode" ("IdQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Topic" ADD CONSTRAINT "FK_Topic_IdSuspendingMode"
	FOREIGN KEY ("IdSuspendingMode") REFERENCES mbox."QueueProcessingMode" ("IdQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Topic" ADD CONSTRAINT "FK_Topic_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."TopicSubscription" ADD CONSTRAINT "FK_TopicSubscription_IdProcessingMode"
	FOREIGN KEY ("IdProcessingMode") REFERENCES mbox."QueueProcessingMode" ("IdQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."TopicSubscription" ADD CONSTRAINT "FK_TopicSubscription_IdSuspendingMode"
	FOREIGN KEY ("IdSuspendingMode") REFERENCES mbox."QueueProcessingMode" ("IdQueueProcessingMode") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."TopicSubscription" ADD CONSTRAINT "FK_TopicSubscription_IdTopic"
	FOREIGN KEY ("IdTopic") REFERENCES mbox."Topic" ("IdTopic") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."TopicSubscription" ADD CONSTRAINT "FK_TopicSubscription_MessageBoxInstance"
	FOREIGN KEY ("IdMessageBoxInstance") REFERENCES mbox."MessageBoxInstance" ("IdMessageBoxInstance") ON DELETE No Action ON UPDATE No Action;
