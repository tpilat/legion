CREATE TABLE comp."Adapter"
(
	"IdAdapter" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Description" varchar(1023) NULL,
	"IdAdapterStatus" uuid NOT NULL,
	"Class" varchar(2047) NOT NULL,
	"Properties" jsonb NULL,
	"PropertiesLoadedUtc" timestamp without time zone NULL,
	"IsInbound" boolean NOT NULL,
	"IsOutbound" boolean NOT NULL
);

CREATE TABLE comp."AdapterLog"
(
	"IdAdapterLog" uuid NOT NULL,
	"IdAdapter" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"LogCorrelationId" uuid NOT NULL,
	"IdAdapterStatus" uuid NOT NULL,
	"Detail" text NOT NULL,
	"Data" jsonb NULL,
	"IdLogMessage" uuid NULL,
	"IdMessageProcessingLog" uuid NULL
);

CREATE TABLE comp."AdapterRequest"
(
	"IdAdapterRequest" uuid NOT NULL,
	"IdAdapter" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"LogCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"Identifier" varchar(127) NOT NULL,
	"Url" varchar(2047) NOT NULL,
	"Method" varchar(15) NULL,
	"Headers" jsonb NULL,
	"ContentType" varchar(255) NULL
);

CREATE TABLE comp."AdapterRequestPayload"
(
	"IdAdapterRequestPayload" uuid NOT NULL,
	"IdAdapterRequest" uuid NOT NULL,
	"RequestContentType" varchar(63) NOT NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"ContentHeaders" jsonb NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL,
	"ContentEncoding" varchar(63) NULL,
	"MediaType" varchar(255) NULL,
	"MultipartFormDataContentName" varchar(511) NULL,
	"MultipartFormDataFileName" varchar(511) NULL,
	"JsonInputCSharpType" varchar(1023) NULL
);

CREATE TABLE comp."AdapterResponse"
(
	"IdAdapterResponse" uuid NOT NULL,
	"IdAdapterRequest" uuid NOT NULL,
	"IdAdapter" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"LogCorrelationId" uuid NOT NULL,
	"Properties" jsonb NULL,
	"StatusCode" integer NULL,
	"Headers" jsonb NULL,
	"ContentType" varchar(255) NULL,
	"Error" text NULL,
	"IdLogMessage" uuid NULL,
	"ElapsedMilliseconds" numeric NULL
);

CREATE TABLE comp."AdapterResponsePayload"
(
	"IdAdapterResponsePayload" uuid NOT NULL,
	"IdAdapterResponse" uuid NOT NULL,
	"ResponseContentType" varchar(63) NOT NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"ContentHeaders" jsonb NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL,
	"ContentEncoding" varchar(63) NULL,
	"MediaType" varchar(255) NULL,
	"MultipartFormDataContentName" varchar(511) NULL,
	"MultipartFormDataFileName" varchar(511) NULL,
	"JsonInputCSharpType" varchar(1023) NULL
);

CREATE TABLE comp."AdapterStatus"
(
	"IdAdapterStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE comp."Job"
(
	"IdJob" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Description" varchar(1023) NULL,
	"IdJobType" uuid NOT NULL,
	"IdJobStatus" uuid NOT NULL,
	"Class" varchar(2047) NOT NULL,
	"Properties" jsonb NULL,
	"PropertiesLoadedUtc" timestamp without time zone NULL,
	"DelayedStartInSeconds" integer NULL,
	"IdleTimeoutInSeconds" integer NULL,
	"CronExpression" varchar(63) NULL,
	"CronExpressionIncludeSeconds" boolean NULL,
	"LastExecutionUtc" timestamp without time zone NULL,
	"NextExecutionUtc" timestamp without time zone NOT NULL,
	"ExecutionEstimatedTimeInSeconds" integer NOT NULL,
	"DeclaringOfflineAfterMinutesOfInactivity" integer NOT NULL
);

CREATE TABLE comp."JobData"
(
	"IdJobData" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"Key" varchar(63) NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"LastModifiedUtc" timestamp without time zone NOT NULL,
	"ContentEncoding" varchar(63) NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelaltivePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL
);

CREATE TABLE comp."JobLog"
(
	"IdJobLog" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"LogCorrelationId" uuid NOT NULL,
	"IdJobStatus" uuid NOT NULL,
	"Detail" text NOT NULL,
	"Data" jsonb NULL,
	"IdLogMessage" uuid NULL,
	"IdMessageProcessingLog" uuid NULL
);

CREATE TABLE comp."JobStatus"
(
	"IdJobStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE comp."JobType"
(
	"IdJobType" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE mbox."Message"
(
	"IdMessage" uuid NOT NULL,
	"IdMessageType" uuid NOT NULL,
	"BusinessId" uuid NOT NULL,
	"IdMessageStatus" uuid NOT NULL,
	"SelfProperties" jsonb NULL,
	"ContextProperties" jsonb NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"IdPreviousMessage" uuid NULL,
	"ExternalId" varchar(511) NULL,
	"IdMessageContent" uuid NULL,
	"ValidToUtc" timestamp without time zone NULL,
	"Priority" integer NOT NULL
);

CREATE TABLE mbox."MessageContent"
(
	"IdMessageContent" uuid NOT NULL,
	"ContentType" varchar(255) NOT NULL,
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
	"IdQueuedMessage" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"LogCorrelationId" uuid NOT NULL,
	"IdMessageProcessingStatus" uuid NOT NULL,
	"Detail" text NOT NULL,
	"Data" jsonb NULL,
	"IdLogMessage" uuid NULL
);

CREATE TABLE mbox."MessageProcessingStatus"
(
	"IdMessageProcessingStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(63) NOT NULL
);

CREATE TABLE mbox."MessagePublishing"
(
	"IdMessagePublishing" uuid NOT NULL,
	"IdStepInstance" uuid NULL,
	"IdJob" uuid NULL,
	"IdAdapter" uuid NULL,
	"IdMessage" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL
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
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"MimeType" varchar(1023) NOT NULL
);

CREATE TABLE orch."Orchestration"
(
	"IdOrchestration" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Description" varchar(1023) NULL,
	"IsSingleton" boolean NOT NULL,
	"Class" varchar(2047) NOT NULL,
	"Properties" jsonb NULL,
	"PropertiesLoadedUtc" timestamp without time zone NULL,
	"TimeoutForMessageProcessingInSeconds" integer NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL,
	"Version" varchar(31) NOT NULL,
	"ValidTo" timestamp without time zone NULL
);

CREATE TABLE orch."OrchestrationInstance"
(
	"IdOrchestrationInstance" uuid NOT NULL,
	"IdOrchestration" uuid NOT NULL,
	"IdOrchestrationStatus" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL
);

CREATE TABLE orch."OrchestrationStatus"
(
	"IdOrchestrationStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE orch."OrchestrationStep"
(
	"IdOrchestrationStep" uuid NOT NULL,
	"IdOrchestration" uuid NOT NULL,
	"IsMainEntry" boolean NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"Description" varchar(1023) NULL,
	"Class" varchar(2047) NOT NULL,
	"Properties" jsonb NULL,
	"PropertiesLoadedUtc" timestamp without time zone NULL,
	"Order" integer NOT NULL
);

CREATE TABLE orch."OrchestrationStepInstance"
(
	"IdOrchestrationStepInstance" uuid NOT NULL,
	"IdOrchestration" uuid NOT NULL,
	"IdOrchestrationStep" uuid NOT NULL,
	"IdStepStatus" uuid NOT NULL,
	"LastProcessedUtc" timestamp without time zone NULL,
	"NextProcessingUtc" timestamp without time zone NULL,
	"RetryCount" integer NOT NULL,
	"SucceededUtc" timestamp without time zone NULL,
	"SuspendedUtc" timestamp without time zone NULL
);

CREATE TABLE orch."OrchestrationStepLog"
(
	"IdOrchestrationStepLog" uuid NOT NULL,
	"IdOrchestrationStepInstance" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"LogCorrelationId" uuid NOT NULL,
	"IdStepStatus" uuid NOT NULL,
	"Detail" text NOT NULL,
	"Data" jsonb NULL,
	"IdLogMessage" uuid NULL,
	"IdMessageProcessingLog" uuid NULL
);

CREATE TABLE orch."OrchestrationStepStatus"
(
	"IdOrchestrationStepStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE mbox."Queue"
(
	"IdQueue" uuid NOT NULL,
	"IdOrchestration" uuid NULL,
	"IdJob" uuid NULL,
	"IdAdapter" uuid NULL,
	"IdMessageType" uuid NOT NULL,
	"IsActive" boolean NOT NULL,
	"IsSequentialFIFO" boolean NOT NULL,
	"TimeoutForMessageProcessingInSeconds" integer NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL,
	"Properties" jsonb NULL,
	"PropertiesLoadedUtc" timestamp without time zone NULL
);

CREATE TABLE mbox."QueuedMessage"
(
	"IdQueuedMessage" uuid NOT NULL,
	"IdQueue" uuid NOT NULL,
	"IdMessage" uuid NOT NULL,
	"QueuedUtc" timestamp without time zone NOT NULL,
	"IdMessageProcessingStatus" uuid NOT NULL,
	"LastProcessedUtc" timestamp without time zone NOT NULL,
	"NextProcessingUtc" timestamp without time zone NOT NULL,
	"RetryCount" integer NOT NULL,
	"ProcessedUtc" timestamp without time zone NULL,
	"TerminatedUtc" timestamp without time zone NULL
);

CREATE TABLE orch."StepDirection"
(
	"IdStepDirection" uuid NOT NULL,
	"IdFromStep" uuid NOT NULL,
	"IdToStep" uuid NOT NULL,
	"CreatedUtc" timestamp without time zone NOT NULL
);

ALTER TABLE comp."Adapter" ADD CONSTRAINT "PK_Adapter"
	PRIMARY KEY ("IdAdapter");

CREATE INDEX "IXFK_Adapter_IdAdapterStatus" ON comp."Adapter" ("IdAdapterStatus" ASC);

ALTER TABLE comp."AdapterLog" ADD CONSTRAINT "PK_AdapterLog"
	PRIMARY KEY ("IdAdapterLog");

CREATE INDEX "IXFK_AdapterLog_IdAdapter" ON comp."AdapterLog" ("IdAdapter" ASC);

CREATE INDEX "IXFK_AdapterLog_IdAdapterStatus" ON comp."AdapterLog" ("IdAdapterStatus" ASC);

CREATE INDEX "IXFK_AdapterLog_IdMessageProcessingLog" ON comp."AdapterLog" ("IdMessageProcessingLog" ASC);

ALTER TABLE comp."AdapterRequest" ADD CONSTRAINT "PK_AdapterRequest"
	PRIMARY KEY ("IdAdapterRequest");

CREATE INDEX "IXFK_AdapterRequest_IdAdapter" ON comp."AdapterRequest" ("IdAdapter" ASC);

ALTER TABLE comp."AdapterRequestPayload" ADD CONSTRAINT "PK_AdapterRequestPayload"
	PRIMARY KEY ("IdAdapterRequestPayload");

CREATE INDEX "IXFK_AdapterRequestPayload_AdapterRequest" ON comp."AdapterRequestPayload" ("IdAdapterRequest" ASC);

ALTER TABLE comp."AdapterResponse" ADD CONSTRAINT "PK_AdapterResponse"
	PRIMARY KEY ("IdAdapterResponse");

CREATE INDEX "IXFK_AdapterResponse_IdAdapter" ON comp."AdapterResponse" ("IdAdapter" ASC);

CREATE INDEX "IXFK_AdapterResponse_IdAdapterRequest" ON comp."AdapterResponse" ("IdAdapterRequest" ASC);

ALTER TABLE comp."AdapterResponsePayload" ADD CONSTRAINT "PK_AdapterRespnosePayload"
	PRIMARY KEY ("IdAdapterResponsePayload");

CREATE INDEX "IXFK_AdapterResponsePayload_AdapterResponse" ON comp."AdapterResponsePayload" ("IdAdapterResponse" ASC);

ALTER TABLE comp."AdapterStatus" ADD CONSTRAINT "PK_AdapterStatus"
	PRIMARY KEY ("IdAdapterStatus");

ALTER TABLE comp."Job" ADD CONSTRAINT "PK_Job"
	PRIMARY KEY ("IdJob");

CREATE INDEX "IXFK_Job_IdJobStatus" ON comp."Job" ("IdJobStatus" ASC);

CREATE INDEX "IXFK_Job_IdJobType" ON comp."Job" ("IdJobType" ASC);

ALTER TABLE comp."JobData" ADD CONSTRAINT "PK_JobData"
	PRIMARY KEY ("IdJobData");

CREATE INDEX "IXFK_JobData_IdJob" ON comp."JobData" ("IdJob" ASC);

ALTER TABLE comp."JobLog" ADD CONSTRAINT "PK_JobLog"
	PRIMARY KEY ("IdJobLog");

CREATE INDEX "IXFK_JobLog_IdJob" ON comp."JobLog" ("IdJob" ASC);

CREATE INDEX "IXFK_JobLog_IdJobStatus" ON comp."JobLog" ("IdJobStatus" ASC);

CREATE INDEX "IXFK_JobLog_IdMessageProcessingLog" ON comp."JobLog" ("IdMessageProcessingLog" ASC);

ALTER TABLE comp."JobStatus" ADD CONSTRAINT "PK_JobStatus"
	PRIMARY KEY ("IdJobStatus");

ALTER TABLE comp."JobType" ADD CONSTRAINT "PK_JobType"
	PRIMARY KEY ("IdJobType");

ALTER TABLE mbox."Message" ADD CONSTRAINT "PK_Message"
	PRIMARY KEY ("IdMessage");

ALTER TABLE mbox."Message" 
  ADD CONSTRAINT "UQ_Message_IdMessageContent" UNIQUE ("IdMessageContent");

CREATE INDEX "IXFK_Message_IdMessageContent" ON mbox."Message" ("IdMessageContent" ASC);

CREATE INDEX "IXFK_Message_IdMessageStatus" ON mbox."Message" ("IdMessageStatus" ASC);

CREATE INDEX "IXFK_Message_IdMessageType" ON mbox."Message" ("IdMessageType" ASC);

CREATE INDEX "IXFK_Message_IdPreviousMessage" ON mbox."Message" ("IdPreviousMessage" ASC);

CREATE INDEX "IX_Message_BusinessId" ON mbox."Message" ("BusinessId" ASC);

ALTER TABLE mbox."MessageContent" ADD CONSTRAINT "PK_MessageContent"
	PRIMARY KEY ("IdMessageContent");

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "PK_MessageProcessingLog"
	PRIMARY KEY ("IdMessageProcessingLog");

CREATE INDEX "IXFK_MessageProcessingLog_IdMessageProcessingStatus" ON mbox."MessageProcessingLog" ("IdMessageProcessingStatus" ASC);

CREATE INDEX "IXFK_MessageProcessingLog_IdQueuedMessage" ON mbox."MessageProcessingLog" ("IdQueuedMessage" ASC);

ALTER TABLE mbox."MessageProcessingStatus" ADD CONSTRAINT "PK_MessageProcessingStatus"
	PRIMARY KEY ("IdMessageProcessingStatus");

ALTER TABLE mbox."MessagePublishing" ADD CONSTRAINT "PK_MessagePublishing"
	PRIMARY KEY ("IdMessagePublishing");

CREATE INDEX "IXFK_MessagePublishing_IdAdapter" ON mbox."MessagePublishing" ("IdAdapter" ASC);

CREATE INDEX "IXFK_MessagePublishing_IdJob" ON mbox."MessagePublishing" ("IdJob" ASC);

CREATE INDEX "IXFK_MessagePublishing_IdMessage" ON mbox."MessagePublishing" ("IdMessage" ASC);

CREATE INDEX "IXFK_MessagePublishing_IdStepInstance" ON mbox."MessagePublishing" ("IdStepInstance" ASC);

ALTER TABLE mbox."MessageStatus" ADD CONSTRAINT "PK_MessageStatus"
	PRIMARY KEY ("IdMessageStatus");

ALTER TABLE mbox."MessageType" ADD CONSTRAINT "PK_MessageType"
	PRIMARY KEY ("IdMessageType");

ALTER TABLE orch."Orchestration" ADD CONSTRAINT "PK_Orchestration"
	PRIMARY KEY ("IdOrchestration");

ALTER TABLE orch."OrchestrationInstance" ADD CONSTRAINT "PK_OrchestrationInstance"
	PRIMARY KEY ("IdOrchestrationInstance");

CREATE INDEX "IXFK_OrchestrationInstance_IdOrchestration" ON orch."OrchestrationInstance" ("IdOrchestration" ASC);

CREATE INDEX "IXFK_OrchestrationInstance_IdOrchestrationStatus" ON orch."OrchestrationInstance" ("IdOrchestrationStatus" ASC);

ALTER TABLE orch."OrchestrationStatus" ADD CONSTRAINT "PK_OrchestrationStatus"
	PRIMARY KEY ("IdOrchestrationStatus");

ALTER TABLE orch."OrchestrationStep" ADD CONSTRAINT "PK_OrchestrationStep"
	PRIMARY KEY ("IdOrchestrationStep");

CREATE INDEX "IXFK_OrchestrationStep_IdOrchestration" ON orch."OrchestrationStep" ("IdOrchestration" ASC);

ALTER TABLE orch."OrchestrationStepInstance" ADD CONSTRAINT "PK_OrchestrationStepInstance"
	PRIMARY KEY ("IdOrchestrationStepInstance");

CREATE INDEX "IXFK_OrchestrationStepInstance_IdOrchestration" ON orch."OrchestrationStepInstance" ("IdOrchestration" ASC);

CREATE INDEX "IXFK_OrchestrationStepInstance_IdOrchestrationStep" ON orch."OrchestrationStepInstance" ("IdOrchestrationStep" ASC);

CREATE INDEX "IXFK_OrchestrationStepInstance_IdStepStatus" ON orch."OrchestrationStepInstance" ("IdStepStatus" ASC);

ALTER TABLE orch."OrchestrationStepLog" ADD CONSTRAINT "PK_OrchestrationStepLog"
	PRIMARY KEY ("IdOrchestrationStepLog");

CREATE INDEX "IXFK_OrchestrationStepLog_IdMessageProcessingLog" ON orch."OrchestrationStepLog" ("IdMessageProcessingLog" ASC);

CREATE INDEX "IXFK_OrchestrationStepLog_IdOrchestrationStepInstance" ON orch."OrchestrationStepLog" ("IdOrchestrationStepInstance" ASC);

CREATE INDEX "IXFK_OrchestrationStepLog_IdStepStatus" ON orch."OrchestrationStepLog" ("IdStepStatus" ASC);

ALTER TABLE orch."OrchestrationStepStatus" ADD CONSTRAINT "PK_OrchestrationStepStatus"
	PRIMARY KEY ("IdOrchestrationStepStatus");

ALTER TABLE mbox."Queue" ADD CONSTRAINT "PK_Queue"
	PRIMARY KEY ("IdQueue");

CREATE INDEX "IXFK_Queue_IdAdapter" ON mbox."Queue" ("IdAdapter" ASC);

CREATE INDEX "IXFK_Queue_IdJob" ON mbox."Queue" ("IdJob" ASC);

CREATE INDEX "IXFK_Queue_IdMessageType" ON mbox."Queue" ("IdMessageType" ASC);

CREATE INDEX "IXFK_Queue_IdOrchestration" ON mbox."Queue" ("IdOrchestration" ASC);

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "PK_QueuedMessage"
	PRIMARY KEY ("IdQueuedMessage");

CREATE INDEX "IXFK_QueuedMessage_IdMessage" ON mbox."QueuedMessage" ("IdMessage" ASC);

CREATE INDEX "IXFK_QueuedMessage_IdMessageProcessingStatus" ON mbox."QueuedMessage" ("IdMessageProcessingStatus" ASC);

CREATE INDEX "IXFK_QueuedMessage_IdQueue" ON mbox."QueuedMessage" ("IdQueue" ASC);

ALTER TABLE orch."StepDirection" ADD CONSTRAINT "PK_StepDirection"
	PRIMARY KEY ("IdStepDirection");

CREATE INDEX "IXFK_StepDirection_IdFromStep" ON orch."StepDirection" ("IdFromStep" ASC);

CREATE INDEX "IXFK_StepDirection_IdToStep" ON orch."StepDirection" ("IdToStep" ASC);

ALTER TABLE comp."Adapter" ADD CONSTRAINT "FK_Adapter_IdAdapterStatus"
	FOREIGN KEY ("IdAdapterStatus") REFERENCES comp."AdapterStatus" ("IdAdapterStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterLog" ADD CONSTRAINT "FK_AdapterLog_IdAdapter"
	FOREIGN KEY ("IdAdapter") REFERENCES comp."Adapter" ("IdAdapter") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterLog" ADD CONSTRAINT "FK_AdapterLog_IdAdapterStatus"
	FOREIGN KEY ("IdAdapterStatus") REFERENCES comp."AdapterStatus" ("IdAdapterStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterLog" ADD CONSTRAINT "FK_AdapterLog_IdMessageProcessingLog"
	FOREIGN KEY ("IdMessageProcessingLog") REFERENCES mbox."MessageProcessingLog" ("IdMessageProcessingLog") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterRequest" ADD CONSTRAINT "FK_AdapterRequest_IdAdapter"
	FOREIGN KEY ("IdAdapter") REFERENCES comp."Adapter" ("IdAdapter") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterRequestPayload" ADD CONSTRAINT "FK_AdapterRequestPayload_IdAdapterRequest"
	FOREIGN KEY ("IdAdapterRequest") REFERENCES comp."AdapterRequest" ("IdAdapterRequest") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterResponse" ADD CONSTRAINT "FK_AdapterResponse_IdAdapter"
	FOREIGN KEY ("IdAdapter") REFERENCES comp."Adapter" ("IdAdapter") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterResponse" ADD CONSTRAINT "FK_AdapterResponse_IdAdapterRequest"
	FOREIGN KEY ("IdAdapterRequest") REFERENCES comp."AdapterRequest" ("IdAdapterRequest") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."AdapterResponsePayload" ADD CONSTRAINT "FK_AdapterResponsePayload_IdAdapterResponse"
	FOREIGN KEY ("IdAdapterResponse") REFERENCES comp."AdapterResponse" ("IdAdapterResponse") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."Job" ADD CONSTRAINT "FK_Job_IdJobStatus"
	FOREIGN KEY ("IdJobStatus") REFERENCES comp."JobStatus" ("IdJobStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."Job" ADD CONSTRAINT "FK_Job_IdJobType"
	FOREIGN KEY ("IdJobType") REFERENCES comp."JobType" ("IdJobType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."JobData" ADD CONSTRAINT "FK_JobData_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES comp."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."JobLog" ADD CONSTRAINT "FK_JobLog_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES comp."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."JobLog" ADD CONSTRAINT "FK_JobLog_IdJobStatus"
	FOREIGN KEY ("IdJobStatus") REFERENCES comp."JobStatus" ("IdJobStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE comp."JobLog" ADD CONSTRAINT "FK_JobLog_IdMessageProcessingLog"
	FOREIGN KEY ("IdMessageProcessingLog") REFERENCES mbox."MessageProcessingLog" ("IdMessageProcessingLog") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdMessageContent"
	FOREIGN KEY ("IdMessageContent") REFERENCES mbox."MessageContent" ("IdMessageContent") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdMessageStatus"
	FOREIGN KEY ("IdMessageStatus") REFERENCES mbox."MessageStatus" ("IdMessageStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES mbox."MessageType" ("IdMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Message" ADD CONSTRAINT "FK_Message_IdPreviousMessage"
	FOREIGN KEY ("IdPreviousMessage") REFERENCES mbox."Message" ("IdMessage") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "FK_MessageProcessingLog_IdMessageProcessingStatus"
	FOREIGN KEY ("IdMessageProcessingStatus") REFERENCES mbox."MessageProcessingStatus" ("IdMessageProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessageProcessingLog" ADD CONSTRAINT "FK_MessageProcessingLog_IdQueuedMessage"
	FOREIGN KEY ("IdQueuedMessage") REFERENCES mbox."QueuedMessage" ("IdQueuedMessage") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessagePublishing" ADD CONSTRAINT "FK_MessagePublishing_IdAdapter"
	FOREIGN KEY ("IdAdapter") REFERENCES comp."Adapter" ("IdAdapter") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessagePublishing" ADD CONSTRAINT "FK_MessagePublishing_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES comp."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessagePublishing" ADD CONSTRAINT "FK_MessagePublishing_IdMessage"
	FOREIGN KEY ("IdMessage") REFERENCES mbox."Message" ("IdMessage") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."MessagePublishing" ADD CONSTRAINT "FK_MessagePublishing_IdStepInstance"
	FOREIGN KEY ("IdStepInstance") REFERENCES orch."OrchestrationStepInstance" ("IdOrchestrationStepInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationInstance" ADD CONSTRAINT "FK_OrchestrationInstance_IdOrchestration"
	FOREIGN KEY ("IdOrchestration") REFERENCES orch."Orchestration" ("IdOrchestration") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationInstance" ADD CONSTRAINT "FK_OrchestrationInstance_IdOrchestrationStatus"
	FOREIGN KEY ("IdOrchestrationStatus") REFERENCES orch."OrchestrationStatus" ("IdOrchestrationStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStep" ADD CONSTRAINT "FK_OrchestrationStep_IdOrchestration"
	FOREIGN KEY ("IdOrchestration") REFERENCES orch."Orchestration" ("IdOrchestration") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepInstance" ADD CONSTRAINT "FK_OrchestrationStepInstance_IdOrchestration"
	FOREIGN KEY ("IdOrchestration") REFERENCES orch."OrchestrationInstance" ("IdOrchestrationInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepInstance" ADD CONSTRAINT "FK_OrchestrationStepInstance_IdOrchestrationStep"
	FOREIGN KEY ("IdOrchestrationStep") REFERENCES orch."OrchestrationStep" ("IdOrchestrationStep") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepInstance" ADD CONSTRAINT "FK_OrchestrationStepInstance_IdStepStatus"
	FOREIGN KEY ("IdStepStatus") REFERENCES orch."OrchestrationStepStatus" ("IdOrchestrationStepStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepLog" ADD CONSTRAINT "FK_OrchestrationStepLog_IdMessageProcessingLog"
	FOREIGN KEY ("IdMessageProcessingLog") REFERENCES mbox."MessageProcessingLog" ("IdMessageProcessingLog") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepLog" ADD CONSTRAINT "FK_OrchestrationStepLog_IdOrchestrationStepInstance"
	FOREIGN KEY ("IdOrchestrationStepInstance") REFERENCES orch."OrchestrationStepInstance" ("IdOrchestrationStepInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepLog" ADD CONSTRAINT "FK_OrchestrationStepLog_IdStepStatus"
	FOREIGN KEY ("IdStepStatus") REFERENCES orch."OrchestrationStepStatus" ("IdOrchestrationStepStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_IdAdapter"
	FOREIGN KEY ("IdAdapter") REFERENCES comp."Adapter" ("IdAdapter") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES comp."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_IdMessageType"
	FOREIGN KEY ("IdMessageType") REFERENCES mbox."MessageType" ("IdMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."Queue" ADD CONSTRAINT "FK_Queue_IdOrchestration"
	FOREIGN KEY ("IdOrchestration") REFERENCES orch."Orchestration" ("IdOrchestration") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "FK_QueuedMessage_IdMessage"
	FOREIGN KEY ("IdMessage") REFERENCES mbox."Message" ("IdMessage") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "FK_QueuedMessage_IdMessageProcessingStatus"
	FOREIGN KEY ("IdMessageProcessingStatus") REFERENCES mbox."MessageProcessingStatus" ("IdMessageProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE mbox."QueuedMessage" ADD CONSTRAINT "FK_QueuedMessage_IdQueue"
	FOREIGN KEY ("IdQueue") REFERENCES mbox."Queue" ("IdQueue") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."StepDirection" ADD CONSTRAINT "FK_StepDirection_IdFromStep"
	FOREIGN KEY ("IdFromStep") REFERENCES orch."OrchestrationStepInstance" ("IdOrchestrationStepInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."StepDirection" ADD CONSTRAINT "FK_StepDirection_IdToStep"
	FOREIGN KEY ("IdToStep") REFERENCES orch."OrchestrationStepInstance" ("IdOrchestrationStepInstance") ON DELETE No Action ON UPDATE No Action;
