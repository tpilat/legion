CREATE TABLE hosts."Host"
(
	"IdHost" uuid NOT NULL,
	"Name" varchar(255) NOT NULL,
	"Description" varchar(511) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IsEnabled" boolean NOT NULL,
	"Configuration" jsonb NOT NULL,
	"RowVersion" uuid NOT NULL
);

CREATE TABLE hosts."HostActivity"
(
	"IdHostActivity" uuid NOT NULL,
	"IdHost" uuid NOT NULL,
	"StartedUtc" timestamp with time zone NOT NULL,
	"LastActivityUtc" timestamp with time zone NOT NULL,
	"StoppedUtc" timestamp with time zone NULL,
	"IsDistributedManagerAvailable" boolean NOT NULL,
	"RowVersion" uuid NOT NULL
);

CREATE TABLE hosts."HostLog"
(
	"IdHostLog" uuid NOT NULL,
	"IdHost" uuid NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IsRunning" boolean NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL
);

CREATE TABLE jobs."Job"
(
	"IdJob" uuid NOT NULL,
	"Name" varchar(255) NOT NULL,
	"Description" varchar(1023) NULL,
	"IdJobRunType" uuid NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"Properties" jsonb NULL,
	"DelayedStartInSeconds" integer NULL,
	"IdleTimeoutInSeconds" integer NULL,
	"CronExpression" varchar(63) NULL,
	"CronExpressionIncludeSeconds" boolean NOT NULL,
	"IdDefaultHost" uuid NOT NULL,
	"RequestedToDisable" boolean NOT NULL,
	"TimeoutForProcessingInSeconds" integer NOT NULL,
	"RowVersion" uuid NOT NULL
);

CREATE TABLE jobs."JobActivity"
(
	"IdJobActivity" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"IdJobStatus" uuid NOT NULL,
	"IdCurrentHost" uuid NOT NULL,
	"AttachedToCurrentHostUtc" timestamp with time zone NOT NULL,
	"LastStatusChangedUtc" timestamp with time zone NOT NULL,
	"LastProcessingStartedUtc" timestamp with time zone NULL,
	"LastProcessingFinishedUtc" timestamp with time zone NULL,
	"DelayedToUtc" timestamp with time zone NULL,
	"RowVersion" uuid NOT NULL
);

CREATE TABLE jobs."JobData"
(
	"IdJobData" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"JobDataIdentifier" varchar(255) NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"LastModifiedUtc" timestamp with time zone NULL,
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

CREATE TABLE jobs."JobExecution"
(
	"IdJobExecution" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"StartUtc" timestamp with time zone NOT NULL,
	"EndUtc" timestamp with time zone NULL,
	"IdJobStatus" uuid NOT NULL,
	"StatisticsStartHourUtc" timestamp with time zone NOT NULL
);

CREATE TABLE jobs."JobLog"
(
	"IdJobLog" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdJobStatus" uuid NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL,
	"IdMessageProcessingLog" uuid NULL,
	"IdJobExecution" uuid NULL
);

CREATE TABLE jobs."JobMessage"
(
	"IdJobMessage" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"IdMessage" uuid NOT NULL,
	"IdJobMessageType" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE jobs."JobMessageType"
(
	"IdJobMessageType" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(63) NOT NULL
);

CREATE TABLE jobs."JobRunType"
(
	"IdJobRunType" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(63) NOT NULL
);

CREATE TABLE jobs."JobStatistics"
(
	"IdJobStatistics" uuid NOT NULL,
	"IdJob" uuid NOT NULL,
	"StartHourUtc" timestamp with time zone NOT NULL,
	"ExecutionCount" integer NOT NULL,
	"ErrorCount" integer NOT NULL,
	"DurationSumInSeconds" bigint NOT NULL
);

CREATE TABLE jobs."JobStatus"
(
	"IdJobStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(63) NOT NULL
);

CREATE TABLE orch."Orchestration"
(
	"IdOrchestration" uuid NOT NULL,
	"Name" varchar(255) NOT NULL,
	"Description" varchar(1023) NULL,
	"IsSingleton" boolean NOT NULL,
	"Namespace" varchar(1023) NOT NULL,
	"Version" varchar(31) NOT NULL,
	"Properties" jsonb NULL
);

CREATE TABLE orch."OrchestrationInstance"
(
	"IdOrchestrationInstance" uuid NOT NULL,
	"IdOrchestration" uuid NOT NULL,
	"IdOrchestrationStatus" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL
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
	"Order" integer NOT NULL,
	"Name" varchar(255) NOT NULL,
	"Description" varchar(1023) NULL,
	"Namespace" varchar(1023) NOT NULL,
	"Properties" jsonb NULL,
	"TimeoutForMessageProcessingInSeconds" integer NOT NULL,
	"MaxMessageProcessingRetryCount" integer NOT NULL
);

CREATE TABLE orch."OrchestrationStepProcessing"
(
	"IdOrchestrationStepProcessing" uuid NOT NULL,
	"IdOrchestrationStep" uuid NOT NULL,
	"IdOrchestrationInstance" uuid NOT NULL,
	"IdOrchestrationStepProcessingStatus" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ProcessedUtc" timestamp with time zone NULL,
	"SuspendedUtc" timestamp with time zone NULL,
	"LastProcessingUtc" timestamp with time zone NULL,
	"NextProcessingUtc" timestamp with time zone NOT NULL,
	"RetryCount" integer NOT NULL
);

CREATE TABLE orch."OrchestrationStepProcessingDirection"
(
	"IdOrchestrationStepProcessingDirection" uuid NOT NULL,
	"IdFromStep" uuid NOT NULL,
	"IdToStep" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE orch."OrchestrationStepProcessingLog"
(
	"IdOrchestrationStepProcessingLog" uuid NOT NULL,
	"IdOrchestrationStepProcessing" uuid NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdOrchestrationStepProcessingStatus" uuid NOT NULL,
	"TraceCorrelationId" uuid NOT NULL,
	"IdLogMessage" uuid NULL,
	"Code" varchar(127) NOT NULL,
	"Detail" text NULL,
	"IdMessageProcessingLog" uuid NULL
);

CREATE TABLE orch."OrchestrationStepProcessingMessage"
(
	"IdOrchestrationStepProcessingMessage" uuid NOT NULL,
	"IdOrchestrationStepProcessing" uuid NOT NULL,
	"IdMessage" uuid NOT NULL,
	"IdOrchestrationStepProcessingMessageType" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE orch."OrchestrationStepProcessingMessageType"
(
	"IdOrchestrationStepProcessingMessageType" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(63) NOT NULL
);

CREATE TABLE orch."OrchestrationStepProcessingStatus"
(
	"IdOrchestrationStepProcessingStatus" uuid NOT NULL,
	"Code" varchar(63) NOT NULL,
	"Name" varchar(127) NOT NULL
);

ALTER TABLE hosts."Host" ADD CONSTRAINT "PK_Host"
	PRIMARY KEY ("IdHost");

ALTER TABLE hosts."Host" 
  ADD CONSTRAINT "UQ_Host_Name" UNIQUE ("Name");

ALTER TABLE hosts."HostActivity" ADD CONSTRAINT "PK_HostActivity"
	PRIMARY KEY ("IdHostActivity");

ALTER TABLE hosts."HostActivity" 
  ADD CONSTRAINT "UQ_HostActivity_IdHost" UNIQUE ("IdHost");

CREATE INDEX "IXFK_HostActivity_Host" ON hosts."HostActivity" ("IdHost" ASC);

ALTER TABLE hosts."HostLog" ADD CONSTRAINT "PK_HostLog"
	PRIMARY KEY ("IdHostLog");

CREATE INDEX "IXFK_HostLog_Host" ON hosts."HostLog" ("IdHost" ASC);

ALTER TABLE jobs."Job" ADD CONSTRAINT "PK_Job"
	PRIMARY KEY ("IdJob");

ALTER TABLE jobs."Job" 
  ADD CONSTRAINT "UQ_Job_Name" UNIQUE ("Name");

CREATE INDEX "IXFK_Job_JobRunType" ON jobs."Job" ("IdJobRunType" ASC);

ALTER TABLE jobs."JobActivity" ADD CONSTRAINT "PK_JobActivity"
	PRIMARY KEY ("IdJobActivity");

ALTER TABLE jobs."JobActivity" 
  ADD CONSTRAINT "UQ_JobActivity_IdJob" UNIQUE ("IdJob");

CREATE INDEX "IXFK_JobActivity_Job" ON jobs."JobActivity" ("IdJob" ASC);

CREATE INDEX "IXFK_JobActivity_JobStatus" ON jobs."JobActivity" ("IdJobStatus" ASC);

ALTER TABLE jobs."JobData" ADD CONSTRAINT "PK_JobData"
	PRIMARY KEY ("IdJobData");

CREATE INDEX "IXFK_JobData_Job" ON jobs."JobData" ("IdJob" ASC);

ALTER TABLE jobs."JobExecution" ADD CONSTRAINT "PK_JobExecution"
	PRIMARY KEY ("IdJobExecution");

CREATE INDEX "IXFK_JobExecution_Job" ON jobs."JobExecution" ("IdJob" ASC);

CREATE INDEX "IXFK_JobExecution_JobStatus" ON jobs."JobExecution" ("IdJobStatus" ASC);

CREATE INDEX "IX_JobExecution_StatisticsStartHourUtc" ON jobs."JobExecution" ("StatisticsStartHourUtc" ASC);

ALTER TABLE jobs."JobLog" ADD CONSTRAINT "PK_JobLog"
	PRIMARY KEY ("IdJobLog");

CREATE INDEX "IXFK_JobLog_Job" ON jobs."JobLog" ("IdJob" ASC);

CREATE INDEX "IXFK_JobLog_JobExecution" ON jobs."JobLog" ("IdJobExecution" ASC);

CREATE INDEX "IXFK_JobLog_JobStatus" ON jobs."JobLog" ("IdJobStatus" ASC);

ALTER TABLE jobs."JobMessage" ADD CONSTRAINT "PK_JobMessage"
	PRIMARY KEY ("IdJobMessage");

CREATE INDEX "IXFK_JobMessage_Job" ON jobs."JobMessage" ("IdJob" ASC);

CREATE INDEX "IXFK_JobMessage_JobMessageType" ON jobs."JobMessage" ("IdJobMessageType" ASC);

ALTER TABLE jobs."JobMessageType" ADD CONSTRAINT "PK_JobMessageType"
	PRIMARY KEY ("IdJobMessageType");

ALTER TABLE jobs."JobRunType" ADD CONSTRAINT "PK_JobRunType"
	PRIMARY KEY ("IdJobRunType");

ALTER TABLE jobs."JobStatistics" ADD CONSTRAINT "PK_JobStatistics"
	PRIMARY KEY ("IdJobStatistics");

ALTER TABLE jobs."JobStatistics" 
  ADD CONSTRAINT "UQ_JobStatistics_IdJob_StartHour" UNIQUE ("IdJob","StartHourUtc");

CREATE INDEX "IXFK_JobStatistics_Job" ON jobs."JobStatistics" ("IdJob" ASC);

ALTER TABLE jobs."JobStatus" ADD CONSTRAINT "PK_JobStatus"
	PRIMARY KEY ("IdJobStatus");

ALTER TABLE orch."Orchestration" ADD CONSTRAINT "PK_Orchestration"
	PRIMARY KEY ("IdOrchestration");

ALTER TABLE orch."OrchestrationInstance" ADD CONSTRAINT "PK_OrchestrationInstance"
	PRIMARY KEY ("IdOrchestrationInstance");

CREATE INDEX "IXFK_OrchestrationInstance_Orchestration" ON orch."OrchestrationInstance" ("IdOrchestration" ASC);

CREATE INDEX "IXFK_OrchestrationInstance_OrchestrationStatus" ON orch."OrchestrationInstance" ("IdOrchestrationStatus" ASC);

ALTER TABLE orch."OrchestrationStatus" ADD CONSTRAINT "PK_OrchestrationStatus"
	PRIMARY KEY ("IdOrchestrationStatus");

ALTER TABLE orch."OrchestrationStep" ADD CONSTRAINT "PK_OrchestrationStep"
	PRIMARY KEY ("IdOrchestrationStep");

CREATE INDEX "IXFK_OrchestrationStep_Orchestration" ON orch."OrchestrationStep" ("IdOrchestration" ASC);

ALTER TABLE orch."OrchestrationStepProcessing" ADD CONSTRAINT "PK_OrchestrationStepProcessing"
	PRIMARY KEY ("IdOrchestrationStepProcessing");

CREATE INDEX "IXFK_OrchestrationStepProcessing_OrchestrationInstance" ON orch."OrchestrationStepProcessing" ("IdOrchestrationInstance" ASC);

CREATE INDEX "IXFK_OrchestrationStepProcessing_OrchestrationStep" ON orch."OrchestrationStepProcessing" ("IdOrchestrationStep" ASC);

CREATE INDEX "IXFK_OrchestrationStepProcessing_OrchestrationStepStatus" ON orch."OrchestrationStepProcessing" ("IdOrchestrationStepProcessingStatus" ASC);

ALTER TABLE orch."OrchestrationStepProcessingDirection" ADD CONSTRAINT "PK_OrchestrationStepProcessingDirection"
	PRIMARY KEY ("IdOrchestrationStepProcessingDirection");

CREATE INDEX "IXFK_OrchestrationStepProcessingDirection_IdFromStep" ON orch."OrchestrationStepProcessingDirection" ("IdFromStep" ASC);

CREATE INDEX "IXFK_OrchestrationStepProcessingDirection_IdToStep" ON orch."OrchestrationStepProcessingDirection" ("IdToStep" ASC);

ALTER TABLE orch."OrchestrationStepProcessingLog" ADD CONSTRAINT "PK_OrchestrationStepProcessingLog"
	PRIMARY KEY ("IdOrchestrationStepProcessingLog");

CREATE INDEX "IXFK_OrchestrationStepProcessingLog_OrchStepProcessing" ON orch."OrchestrationStepProcessingLog" ("IdOrchestrationStepProcessing" ASC);

CREATE INDEX "IXFK_OrchestrationStepProcessingLog_OrchStepProcessingStatus" ON orch."OrchestrationStepProcessingLog" ("IdOrchestrationStepProcessingStatus" ASC);

ALTER TABLE orch."OrchestrationStepProcessingMessage" ADD CONSTRAINT "PK_OrchestrationMessage"
	PRIMARY KEY ("IdOrchestrationStepProcessingMessage");

CREATE INDEX "IXFK_OrchestrationStepProcessingMessage_OrchStepProcessing" ON orch."OrchestrationStepProcessingMessage" ("IdOrchestrationStepProcessing" ASC);

CREATE INDEX "IXFK_OrchestrationStepProcessingMessage_OrchStepProcessingMessageType" ON orch."OrchestrationStepProcessingMessage" ("IdOrchestrationStepProcessingMessageType" ASC);

ALTER TABLE orch."OrchestrationStepProcessingMessageType" ADD CONSTRAINT "PK_OrchestrationStepProcessingMessageType"
	PRIMARY KEY ("IdOrchestrationStepProcessingMessageType");

ALTER TABLE orch."OrchestrationStepProcessingStatus" ADD CONSTRAINT "PK_OrchestrationStepProcessingStatus"
	PRIMARY KEY ("IdOrchestrationStepProcessingStatus");

ALTER TABLE hosts."HostActivity" ADD CONSTRAINT "FK_HostActivity_IdHost"
	FOREIGN KEY ("IdHost") REFERENCES hosts."Host" ("IdHost") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE hosts."HostLog" ADD CONSTRAINT "FK_HostLog_IdHost"
	FOREIGN KEY ("IdHost") REFERENCES hosts."Host" ("IdHost") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."Job" ADD CONSTRAINT "FK_Job_IdJobRunType"
	FOREIGN KEY ("IdJobRunType") REFERENCES jobs."JobRunType" ("IdJobRunType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobActivity" ADD CONSTRAINT "FK_JobActivity_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES jobs."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobActivity" ADD CONSTRAINT "FK_JobActivity_IdJobStatus"
	FOREIGN KEY ("IdJobStatus") REFERENCES jobs."JobStatus" ("IdJobStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobData" ADD CONSTRAINT "FK_JobData_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES jobs."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobExecution" ADD CONSTRAINT "FK_JobExecution_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES jobs."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobExecution" ADD CONSTRAINT "FK_JobExecution_IdJobStatus"
	FOREIGN KEY ("IdJobStatus") REFERENCES jobs."JobStatus" ("IdJobStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobLog" ADD CONSTRAINT "FK_JobLog_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES jobs."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobLog" ADD CONSTRAINT "FK_JobLog_IdJobExecution"
	FOREIGN KEY ("IdJobExecution") REFERENCES jobs."JobExecution" ("IdJobExecution") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobLog" ADD CONSTRAINT "FK_JobLog_IdJobStatus"
	FOREIGN KEY ("IdJobStatus") REFERENCES jobs."JobStatus" ("IdJobStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobMessage" ADD CONSTRAINT "FK_JobMessage_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES jobs."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobMessage" ADD CONSTRAINT "FK_JobMessage_IdJobMessageType"
	FOREIGN KEY ("IdJobMessageType") REFERENCES jobs."JobMessageType" ("IdJobMessageType") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE jobs."JobStatistics" ADD CONSTRAINT "FK_JobStatistics_IdJob"
	FOREIGN KEY ("IdJob") REFERENCES jobs."Job" ("IdJob") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationInstance" ADD CONSTRAINT "FK_OrchestrationInstance_IdOrchestration"
	FOREIGN KEY ("IdOrchestration") REFERENCES orch."Orchestration" ("IdOrchestration") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationInstance" ADD CONSTRAINT "FK_OrchestrationInstance_IdOrchestrationStatus"
	FOREIGN KEY ("IdOrchestrationStatus") REFERENCES orch."OrchestrationStatus" ("IdOrchestrationStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStep" ADD CONSTRAINT "FK_OrchestrationStep_IdOrchestration"
	FOREIGN KEY ("IdOrchestration") REFERENCES orch."Orchestration" ("IdOrchestration") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessing" ADD CONSTRAINT "FK_OrchestrationStepProcessing_IdOrchestrationInstance"
	FOREIGN KEY ("IdOrchestrationInstance") REFERENCES orch."OrchestrationInstance" ("IdOrchestrationInstance") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessing" ADD CONSTRAINT "FK_OrchestrationStepProcessing_IdOrchestrationStep"
	FOREIGN KEY ("IdOrchestrationStep") REFERENCES orch."OrchestrationStep" ("IdOrchestrationStep") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessing" ADD CONSTRAINT "FK_OrchestrationStepProcessing_IdOrchestrationStepProcessingStatus"
	FOREIGN KEY ("IdOrchestrationStepProcessingStatus") REFERENCES orch."OrchestrationStepProcessingStatus" ("IdOrchestrationStepProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessingDirection" ADD CONSTRAINT "FK_OrchestrationStepProcessingDirection_IdFromStep"
	FOREIGN KEY ("IdFromStep") REFERENCES orch."OrchestrationStepProcessing" ("IdOrchestrationStepProcessing") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessingDirection" ADD CONSTRAINT "FK_OrchestrationStepProcessingDirection_IdToStep"
	FOREIGN KEY ("IdToStep") REFERENCES orch."OrchestrationStepProcessing" ("IdOrchestrationStepProcessing") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessingLog" ADD CONSTRAINT "FK_OrchestrationStepProcessingLog_IdOrchStepProcessing"
	FOREIGN KEY ("IdOrchestrationStepProcessing") REFERENCES orch."OrchestrationStepProcessing" ("IdOrchestrationStepProcessing") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessingLog" ADD CONSTRAINT "FK_OrchestrationStepProcessingLog_IdOrchStepProcessingStatus"
	FOREIGN KEY ("IdOrchestrationStepProcessingStatus") REFERENCES orch."OrchestrationStepProcessingStatus" ("IdOrchestrationStepProcessingStatus") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessingMessage" ADD CONSTRAINT "FK_OrchestrationStepProcessingMessage_IdOrchStepProcessing"
	FOREIGN KEY ("IdOrchestrationStepProcessing") REFERENCES orch."OrchestrationStepProcessing" ("IdOrchestrationStepProcessing") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE orch."OrchestrationStepProcessingMessage" ADD CONSTRAINT "FK_OrchestrationStepProcessingMessage_IdOrchStepProcessingMessageType"
	FOREIGN KEY ("IdOrchestrationStepProcessingMessageType") REFERENCES orch."OrchestrationStepProcessingMessageType" ("IdOrchestrationStepProcessingMessageType") ON DELETE No Action ON UPDATE No Action;
