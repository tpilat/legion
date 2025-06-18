CREATE TABLE [jobs].[Job]
(
	[IdJob] uniqueidentifier NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Description] nvarchar(1023) NULL,
	[IdJobRunType] uniqueidentifier NOT NULL,
	[IdJobStatus] uniqueidentifier NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[Properties] nvarchar(max) NULL,
	[DelayedStartInSeconds] int NULL,
	[IdleTimeoutInSeconds] int NULL,
	[CronExpression] nvarchar(63) NULL,
	[CronExpressionIncludeSeconds] bit NOT NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[NextProcessinUtc] datetime2(7) NOT NULL,
	[TimeoutForProcessingInSeconds] int NOT NULL,
	[MaxProcessingRetryCount] int NOT NULL
)
GO

CREATE TABLE [jobs].[JobData]
(
	[IdJobData] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NOT NULL,
	[JobDataIdentifier] nvarchar(255) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[LastModifiedUtc] datetime2(7) NULL,
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

CREATE TABLE [jobs].[JobExecution]
(
	[IdJobExecution] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[StartUtc] datetime2(7) NOT NULL,
	[EndUtc] datetime2(7) NULL,
	[IdJobStatus] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [jobs].[JobLog]
(
	[IdJobLog] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NOT NULL,
	[IdLogLevel] int NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdJobStatus] uniqueidentifier NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL,
	[IdMessageProcessingLog] uniqueidentifier NULL
)
GO

CREATE TABLE [jobs].[JobMessage]
(
	[IdJobMessage] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NOT NULL,
	[IdMessage] uniqueidentifier NOT NULL,
	[IdJobMessageType] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [jobs].[JobMessageType]
(
	[IdJobMessageType] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(63) NOT NULL
)
GO

CREATE TABLE [jobs].[JobRunType]
(
	[IdJobRunType] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(63) NOT NULL
)
GO

CREATE TABLE [jobs].[JobStatistics]
(
	[IdJobStatistics] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NOT NULL,
	[StartHoutUtc] datetime2(7) NOT NULL,
	[ExecutionCount] int NOT NULL,
	[ErrorCount] int NOT NULL,
	[AverageDuration] decimal(18) NOT NULL
)
GO

CREATE TABLE [jobs].[JobStatus]
(
	[IdJobStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(63) NOT NULL
)
GO

CREATE TABLE [orch].[Orchestration]
(
	[IdOrchestration] uniqueidentifier NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Description] nvarchar(1023) NULL,
	[IsSingleton] bit NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[Version] nvarchar(31) NOT NULL,
	[Properties] nvarchar(max) NULL
)
GO

CREATE TABLE [orch].[OrchestrationInstance]
(
	[IdOrchestrationInstance] uniqueidentifier NOT NULL,
	[IdOrchestration] uniqueidentifier NOT NULL,
	[IdOrchestrationStatus] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [orch].[OrchestrationStatus]
(
	[IdOrchestrationStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [orch].[OrchestrationStep]
(
	[IdOrchestrationStep] uniqueidentifier NOT NULL,
	[IdOrchestration] uniqueidentifier NOT NULL,
	[IsMainEntry] bit NOT NULL,
	[Order] int NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Description] nvarchar(1023) NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[Properties] nvarchar(max) NULL,
	[TimeoutForMessageProcessingInSeconds] int NOT NULL,
	[MaxMessageProcessingRetryCount] int NOT NULL
)
GO

CREATE TABLE [orch].[OrchestrationStepProcessing]
(
	[IdOrchestrationStepProcessing] uniqueidentifier NOT NULL,
	[IdOrchestrationStep] uniqueidentifier NOT NULL,
	[IdOrchestrationInstance] uniqueidentifier NOT NULL,
	[IdOrchestrationStepProcessingStatus] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ProcessedUtc] datetime2(7) NULL,
	[SuspendedUtc] datetime2(7) NULL,
	[LastProcessingUtc] datetime2(7) NULL,
	[NextProcessingUtc] datetime2(7) NOT NULL,
	[RetryCount] int NOT NULL
)
GO

CREATE TABLE [orch].[OrchestrationStepProcessingDirection]
(
	[IdOrchestrationStepProcessingDirection] uniqueidentifier NOT NULL,
	[IdFromStep] uniqueidentifier NOT NULL,
	[IdToStep] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [orch].[OrchestrationStepProcessingLog]
(
	[IdOrchestrationStepProcessingLog] uniqueidentifier NOT NULL,
	[IdOrchestrationStepProcessing] uniqueidentifier NOT NULL,
	[IdLogLevel] int NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdOrchestrationStepProcessingStatus] uniqueidentifier NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL,
	[IdMessageProcessingLog] uniqueidentifier NULL
)
GO

CREATE TABLE [orch].[OrchestrationStepProcessingMessage]
(
	[IdOrchestrationStepProcessingMessage] uniqueidentifier NOT NULL,
	[IdOrchestrationStepProcessing] uniqueidentifier NOT NULL,
	[IdMessage] uniqueidentifier NOT NULL,
	[IdOrchestrationStepProcessingMessageType] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [orch].[OrchestrationStepProcessingMessageType]
(
	[IdOrchestrationStepProcessingMessageType] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(63) NOT NULL
)
GO

CREATE TABLE [orch].[OrchestrationStepProcessingStatus]
(
	[IdOrchestrationStepProcessingStatus] uniqueidentifier NOT NULL,
	[Code] nvarchar(63) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

ALTER TABLE [jobs].[Job] 
 ADD CONSTRAINT [PK_Job]
	PRIMARY KEY CLUSTERED ([IdJob] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Job_JobRunType] 
 ON [jobs].[Job] ([IdJobRunType] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Job_JobStatus] 
 ON [jobs].[Job] ([IdJobStatus] ASC)
GO

ALTER TABLE [jobs].[JobData] 
 ADD CONSTRAINT [PK_JobData]
	PRIMARY KEY CLUSTERED ([IdJobData] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobData_Job] 
 ON [jobs].[JobData] ([IdJob] ASC)
GO

ALTER TABLE [jobs].[JobExecution] 
 ADD CONSTRAINT [PK_JobExecution]
	PRIMARY KEY CLUSTERED ([IdJobExecution] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobExecution_Job] 
 ON [jobs].[JobExecution] ([IdJob] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobExecution_JobStatus] 
 ON [jobs].[JobExecution] ([IdJobStatus] ASC)
GO

ALTER TABLE [jobs].[JobLog] 
 ADD CONSTRAINT [PK_JobLog]
	PRIMARY KEY CLUSTERED ([IdJobLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobLog_Job] 
 ON [jobs].[JobLog] ([IdJob] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobLog_JobStatus] 
 ON [jobs].[JobLog] ([IdJobStatus] ASC)
GO

ALTER TABLE [jobs].[JobMessage] 
 ADD CONSTRAINT [PK_JobMessage]
	PRIMARY KEY CLUSTERED ([IdJobMessage] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobMessage_Job] 
 ON [jobs].[JobMessage] ([IdJob] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobMessage_JobMessageType] 
 ON [jobs].[JobMessage] ([IdJobMessageType] ASC)
GO

ALTER TABLE [jobs].[JobMessageType] 
 ADD CONSTRAINT [PK_JobMessageType]
	PRIMARY KEY CLUSTERED ([IdJobMessageType] ASC)
GO

ALTER TABLE [jobs].[JobRunType] 
 ADD CONSTRAINT [PK_JobRunType]
	PRIMARY KEY CLUSTERED ([IdJobRunType] ASC)
GO

ALTER TABLE [jobs].[JobStatistics] 
 ADD CONSTRAINT [PK_JobStatistics]
	PRIMARY KEY CLUSTERED ([IdJobStatistics] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobStatistics_Job] 
 ON [jobs].[JobStatistics] ([IdJob] ASC)
GO

ALTER TABLE [jobs].[JobStatus] 
 ADD CONSTRAINT [PK_JobStatus]
	PRIMARY KEY CLUSTERED ([IdJobStatus] ASC)
GO

ALTER TABLE [orch].[Orchestration] 
 ADD CONSTRAINT [PK_Orchestration]
	PRIMARY KEY CLUSTERED ([IdOrchestration] ASC)
GO

ALTER TABLE [orch].[OrchestrationInstance] 
 ADD CONSTRAINT [PK_OrchestrationInstance]
	PRIMARY KEY CLUSTERED ([IdOrchestrationInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationInstance_Orchestration] 
 ON [orch].[OrchestrationInstance] ([IdOrchestration] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationInstance_OrchestrationStatus] 
 ON [orch].[OrchestrationInstance] ([IdOrchestrationStatus] ASC)
GO

ALTER TABLE [orch].[OrchestrationStatus] 
 ADD CONSTRAINT [PK_OrchestrationStatus]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStatus] ASC)
GO

ALTER TABLE [orch].[OrchestrationStep] 
 ADD CONSTRAINT [PK_OrchestrationStep]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStep] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStep_Orchestration] 
 ON [orch].[OrchestrationStep] ([IdOrchestration] ASC)
GO

ALTER TABLE [orch].[OrchestrationStepProcessing] 
 ADD CONSTRAINT [PK_OrchestrationStepProcessing]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStepProcessing] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessing_OrchestrationInstance] 
 ON [orch].[OrchestrationStepProcessing] ([IdOrchestrationInstance] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessing_OrchestrationStep] 
 ON [orch].[OrchestrationStepProcessing] ([IdOrchestrationStep] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessing_OrchestrationStepStatus] 
 ON [orch].[OrchestrationStepProcessing] ([IdOrchestrationStepProcessingStatus] ASC)
GO

ALTER TABLE [orch].[OrchestrationStepProcessingDirection] 
 ADD CONSTRAINT [PK_OrchestrationStepProcessingDirection]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStepProcessingDirection] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessingDirection_IdFromStep] 
 ON [orch].[OrchestrationStepProcessingDirection] ([IdFromStep] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessingDirection_IdToStep] 
 ON [orch].[OrchestrationStepProcessingDirection] ([IdToStep] ASC)
GO

ALTER TABLE [orch].[OrchestrationStepProcessingLog] 
 ADD CONSTRAINT [PK_OrchestrationStepProcessingLog]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStepProcessingLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessingLog_OrchStepProcessing] 
 ON [orch].[OrchestrationStepProcessingLog] ([IdOrchestrationStepProcessing] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessingLog_OrchStepProcessingStatus] 
 ON [orch].[OrchestrationStepProcessingLog] ([IdOrchestrationStepProcessingStatus] ASC)
GO

ALTER TABLE [orch].[OrchestrationStepProcessingMessage] 
 ADD CONSTRAINT [PK_OrchestrationMessage]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStepProcessingMessage] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessingMessage_OrchStepProcessing] 
 ON [orch].[OrchestrationStepProcessingMessage] ([IdOrchestrationStepProcessing] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_OrchestrationStepProcessingMessage_OrchStepProcessingMessageType] 
 ON [orch].[OrchestrationStepProcessingMessage] ([IdOrchestrationStepProcessingMessageType] ASC)
GO

ALTER TABLE [orch].[OrchestrationStepProcessingMessageType] 
 ADD CONSTRAINT [PK_OrchestrationStepProcessingMessageType]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStepProcessingMessageType] ASC)
GO

ALTER TABLE [orch].[OrchestrationStepProcessingStatus] 
 ADD CONSTRAINT [PK_OrchestrationStepProcessingStatus]
	PRIMARY KEY CLUSTERED ([IdOrchestrationStepProcessingStatus] ASC)
GO

ALTER TABLE [jobs].[Job] ADD CONSTRAINT [FK_Job_IdJobRunType]
	FOREIGN KEY ([IdJobRunType]) REFERENCES [jobs].[JobRunType] ([IdJobRunType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[Job] ADD CONSTRAINT [FK_Job_IdJobStatus]
	FOREIGN KEY ([IdJobStatus]) REFERENCES [jobs].[JobStatus] ([IdJobStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobData] ADD CONSTRAINT [FK_JobData_IdJob]
	FOREIGN KEY ([IdJob]) REFERENCES [jobs].[Job] ([IdJob]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobExecution] ADD CONSTRAINT [FK_JobExecution_IdJob]
	FOREIGN KEY ([IdJob]) REFERENCES [jobs].[Job] ([IdJob]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobExecution] ADD CONSTRAINT [FK_JobExecution_IdJobStatus]
	FOREIGN KEY ([IdJobStatus]) REFERENCES [jobs].[JobStatus] ([IdJobStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobLog] ADD CONSTRAINT [FK_JobLog_IdJob]
	FOREIGN KEY ([IdJob]) REFERENCES [jobs].[Job] ([IdJob]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobLog] ADD CONSTRAINT [FK_JobLog_IdJobStatus]
	FOREIGN KEY ([IdJobStatus]) REFERENCES [jobs].[JobStatus] ([IdJobStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobMessage] ADD CONSTRAINT [FK_JobMessage_IdJob]
	FOREIGN KEY ([IdJob]) REFERENCES [jobs].[Job] ([IdJob]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobMessage] ADD CONSTRAINT [FK_JobMessage_IdJobMessageType]
	FOREIGN KEY ([IdJobMessageType]) REFERENCES [jobs].[JobMessageType] ([IdJobMessageType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobStatistics] ADD CONSTRAINT [FK_JobStatistics_IdJob]
	FOREIGN KEY ([IdJob]) REFERENCES [jobs].[Job] ([IdJob]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationInstance] ADD CONSTRAINT [FK_OrchestrationInstance_IdOrchestration]
	FOREIGN KEY ([IdOrchestration]) REFERENCES [orch].[Orchestration] ([IdOrchestration]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationInstance] ADD CONSTRAINT [FK_OrchestrationInstance_IdOrchestrationStatus]
	FOREIGN KEY ([IdOrchestrationStatus]) REFERENCES [orch].[OrchestrationStatus] ([IdOrchestrationStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStep] ADD CONSTRAINT [FK_OrchestrationStep_IdOrchestration]
	FOREIGN KEY ([IdOrchestration]) REFERENCES [orch].[Orchestration] ([IdOrchestration]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessing] ADD CONSTRAINT [FK_OrchestrationStepProcessing_IdOrchestrationInstance]
	FOREIGN KEY ([IdOrchestrationInstance]) REFERENCES [orch].[OrchestrationInstance] ([IdOrchestrationInstance]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessing] ADD CONSTRAINT [FK_OrchestrationStepProcessing_IdOrchestrationStep]
	FOREIGN KEY ([IdOrchestrationStep]) REFERENCES [orch].[OrchestrationStep] ([IdOrchestrationStep]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessing] ADD CONSTRAINT [FK_OrchestrationStepProcessing_IdOrchestrationStepProcessingStatus]
	FOREIGN KEY ([IdOrchestrationStepProcessingStatus]) REFERENCES [orch].[OrchestrationStepProcessingStatus] ([IdOrchestrationStepProcessingStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessingDirection] ADD CONSTRAINT [FK_OrchestrationStepProcessingDirection_IdFromStep]
	FOREIGN KEY ([IdFromStep]) REFERENCES [orch].[OrchestrationStepProcessing] ([IdOrchestrationStepProcessing]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessingDirection] ADD CONSTRAINT [FK_OrchestrationStepProcessingDirection_IdToStep]
	FOREIGN KEY ([IdToStep]) REFERENCES [orch].[OrchestrationStepProcessing] ([IdOrchestrationStepProcessing]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessingLog] ADD CONSTRAINT [FK_OrchestrationStepProcessingLog_IdOrchStepProcessing]
	FOREIGN KEY ([IdOrchestrationStepProcessing]) REFERENCES [orch].[OrchestrationStepProcessing] ([IdOrchestrationStepProcessing]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessingLog] ADD CONSTRAINT [FK_OrchestrationStepProcessingLog_IdOrchStepProcessingStatus]
	FOREIGN KEY ([IdOrchestrationStepProcessingStatus]) REFERENCES [orch].[OrchestrationStepProcessingStatus] ([IdOrchestrationStepProcessingStatus]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessingMessage] ADD CONSTRAINT [FK_OrchestrationStepProcessingMessage_IdOrchStepProcessing]
	FOREIGN KEY ([IdOrchestrationStepProcessing]) REFERENCES [orch].[OrchestrationStepProcessing] ([IdOrchestrationStepProcessing]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [orch].[OrchestrationStepProcessingMessage] ADD CONSTRAINT [FK_OrchestrationStepProcessingMessage_IdOrchStepProcessingMessageType]
	FOREIGN KEY ([IdOrchestrationStepProcessingMessageType]) REFERENCES [orch].[OrchestrationStepProcessingMessageType] ([IdOrchestrationStepProcessingMessageType]) ON DELETE No Action ON UPDATE No Action
GO
