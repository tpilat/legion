CREATE TABLE [hosts].[Host]
(
	[IdHost] uniqueidentifier NOT NULL,
	[Name] varchar(255) NOT NULL,
	[Description] varchar(511) NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IsEnabled] bit NOT NULL,
	[Configuration] nvarchar(max) NOT NULL,
	[RowVersion] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [hosts].[HostActivity]
(
	[IdHostActivity] uniqueidentifier NOT NULL,
	[IdHost] uniqueidentifier NOT NULL,
	[StartedUtc] datetime2(7) NOT NULL,
	[LastActivityUtc] datetime2(7) NOT NULL,
	[StoppedUtc] datetime2(7) NULL,
	[IsDistributedManagerAvailable] bit NOT NULL,
	[RowVersion] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [hosts].[HostLog]
(
	[IdHostLog] uniqueidentifier NOT NULL,
	[IdHost] uniqueidentifier NOT NULL,
	[IdLogLevel] int NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IsRunning] bit NOT NULL,
	[TraceCorrelationId] uniqueidentifier NOT NULL,
	[IdLogMessage] uniqueidentifier NULL,
	[Code] nvarchar(127) NOT NULL,
	[Detail] nvarchar(max) NULL
)
GO

CREATE TABLE [jobs].[Job]
(
	[IdJob] uniqueidentifier NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Description] nvarchar(1023) NULL,
	[IdJobRunType] uniqueidentifier NOT NULL,
	[Namespace] nvarchar(1023) NOT NULL,
	[Properties] nvarchar(max) NULL,
	[DelayedStartInSeconds] int NULL,
	[IdleTimeoutInSeconds] int NULL,
	[CronExpression] nvarchar(63) NULL,
	[CronExpressionIncludeSeconds] bit NOT NULL,
	[IdDefaultHost] uniqueidentifier NOT NULL,
	[RequestedToDisable] bit NOT NULL,
	[TimeoutForProcessingInSeconds] int NOT NULL,
	[RowVersion] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [jobs].[JobActivity]
(
	[IdJobActivity] uniqueidentifier NOT NULL,
	[IdJob] uniqueidentifier NOT NULL,
	[IdJobStatus] uniqueidentifier NOT NULL,
	[IdCurrentHost] uniqueidentifier NOT NULL,
	[AttachedToCurrentHostUtc] datetime2(7) NOT NULL,
	[LastStatusChangedUtc] datetime2(7) NOT NULL,
	[LastProcessingStartedUtc] datetime2(7) NULL,
	[LastProcessingFinishedUtc] datetime2(7) NULL,
	[DelayedToUtc] datetime2(7) NULL,
	[RowVersion] uniqueidentifier NOT NULL
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
	[IdJobStatus] uniqueidentifier NOT NULL,
	[StatisticsStartHourUtc] datetime2(7) NOT NULL
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
	[IdMessageProcessingLog] uniqueidentifier NULL,
	[IdJobExecution] uniqueidentifier NULL
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
	[StartHourUtc] datetime2(7) NOT NULL,
	[ExecutionCount] int NOT NULL,
	[ErrorCount] int NOT NULL,
	[DurationSumInSeconds] bigint NOT NULL
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

ALTER TABLE [hosts].[Host] 
 ADD CONSTRAINT [PK_Host]
	PRIMARY KEY CLUSTERED ([IdHost] ASC)
GO

ALTER TABLE [hosts].[Host] 
 ADD CONSTRAINT [UQ_Host_Name] UNIQUE NONCLUSTERED ([Name] ASC)
GO

ALTER TABLE [hosts].[HostActivity] 
 ADD CONSTRAINT [PK_HostActivity]
	PRIMARY KEY CLUSTERED ([IdHostActivity] ASC)
GO

ALTER TABLE [hosts].[HostActivity] 
 ADD CONSTRAINT [UQ_HostActivity_IdHost] UNIQUE NONCLUSTERED ([IdHost] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_HostActivity_Host] 
 ON [hosts].[HostActivity] ([IdHost] ASC)
GO

ALTER TABLE [hosts].[HostLog] 
 ADD CONSTRAINT [PK_HostLog]
	PRIMARY KEY CLUSTERED ([IdHostLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_HostLog_Host] 
 ON [hosts].[HostLog] ([IdHost] ASC)
GO

ALTER TABLE [jobs].[Job] 
 ADD CONSTRAINT [PK_Job]
	PRIMARY KEY CLUSTERED ([IdJob] ASC)
GO

ALTER TABLE [jobs].[Job] 
 ADD CONSTRAINT [UQ_Job_Name] UNIQUE NONCLUSTERED ([Name] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Job_JobRunType] 
 ON [jobs].[Job] ([IdJobRunType] ASC)
GO

ALTER TABLE [jobs].[JobActivity] 
 ADD CONSTRAINT [PK_JobActivity]
	PRIMARY KEY CLUSTERED ([IdJobActivity] ASC)
GO

ALTER TABLE [jobs].[JobActivity] 
 ADD CONSTRAINT [UQ_JobActivity_IdJob] UNIQUE NONCLUSTERED ([IdJob] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobActivity_Job] 
 ON [jobs].[JobActivity] ([IdJob] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobActivity_JobStatus] 
 ON [jobs].[JobActivity] ([IdJobStatus] ASC)
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

CREATE NONCLUSTERED INDEX [IX_JobExecution_StatisticsStartHourUtc] 
 ON [jobs].[JobExecution] ([StatisticsStartHourUtc] ASC)
GO

ALTER TABLE [jobs].[JobLog] 
 ADD CONSTRAINT [PK_JobLog]
	PRIMARY KEY CLUSTERED ([IdJobLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobLog_Job] 
 ON [jobs].[JobLog] ([IdJob] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_JobLog_JobExecution] 
 ON [jobs].[JobLog] ([IdJobExecution] ASC)
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

ALTER TABLE [jobs].[JobStatistics] 
 ADD CONSTRAINT [UQ_JobStatistics_IdJob_StartHour] UNIQUE NONCLUSTERED ([IdJob] ASC,[StartHourUtc] ASC)
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

ALTER TABLE [hosts].[HostActivity] ADD CONSTRAINT [FK_HostActivity_IdHost]
	FOREIGN KEY ([IdHost]) REFERENCES [hosts].[Host] ([IdHost]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [hosts].[HostLog] ADD CONSTRAINT [FK_HostLog_IdHost]
	FOREIGN KEY ([IdHost]) REFERENCES [hosts].[Host] ([IdHost]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[Job] ADD CONSTRAINT [FK_Job_IdJobRunType]
	FOREIGN KEY ([IdJobRunType]) REFERENCES [jobs].[JobRunType] ([IdJobRunType]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobActivity] ADD CONSTRAINT [FK_JobActivity_IdJob]
	FOREIGN KEY ([IdJob]) REFERENCES [jobs].[Job] ([IdJob]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [jobs].[JobActivity] ADD CONSTRAINT [FK_JobActivity_IdJobStatus]
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

ALTER TABLE [jobs].[JobLog] ADD CONSTRAINT [FK_JobLog_IdJobExecution]
	FOREIGN KEY ([IdJobExecution]) REFERENCES [jobs].[JobExecution] ([IdJobExecution]) ON DELETE No Action ON UPDATE No Action
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
