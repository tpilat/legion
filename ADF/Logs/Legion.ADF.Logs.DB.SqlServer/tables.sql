CREATE TABLE [log].[EnvironmentInfo]
(
	[IdEnvironmentInfo] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ApplicationName] nvarchar(127) NULL,
	[ApplicationVersion] nvarchar(15) NULL,
	[RunningEnvironment] nvarchar(255) NULL,
	[ProcessName] nvarchar(255) NULL,
	[ProcessId] int NULL,
	[FrameworkDescription] nvarchar(255) NULL,
	[TargetFramework] nvarchar(255) NULL,
	[CLRVersion] nvarchar(255) NULL,
	[EntryAssemblyName] nvarchar(255) NULL,
	[EntryAssemblyVersion] nvarchar(255) NULL,
	[BaseDirectory] nvarchar(255) NULL,
	[MachineName] nvarchar(255) NULL,
	[CurrentAppDomainName] nvarchar(255) NULL,
	[Is64BitOperatingSystem] bit NULL,
	[Is64BitProcess] bit NULL,
	[OperatingSystemArchitecture] nvarchar(255) NULL,
	[OperatingSystemPlatform] nvarchar(255) NULL,
	[OperatingSystemVersion] nvarchar(255) NULL,
	[ProcessArchitecture] nvarchar(255) NULL,
	[CommandLine] nvarchar(1023) NULL
)
GO

CREATE TABLE [log].[EventCounter]
(
	[IdEventCounter] uniqueidentifier NOT NULL,
	[IdEventCounterCategory] uniqueidentifier NOT NULL,
	[Code] nvarchar(127) NOT NULL,
	[Name] nvarchar(127) NOT NULL,
	[DisplayName] nvarchar(127) NOT NULL,
	[CounterType] nvarchar(63) NOT NULL,
	[DisplayRateTimeScale] nvarchar(31) NULL,
	[Metadata] nvarchar(max) NULL,
	[DisplayUnits] nvarchar(31) NULL
)
GO

CREATE TABLE [log].[EventCounterCategory]
(
	[IdEventCounterCategory] uniqueidentifier NOT NULL,
	[Source] nvarchar(127) NOT NULL,
	[DisplayName] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [log].[EventCounterData]
(
	[IdEventCounterData] uniqueidentifier NOT NULL,
	[IdEventCounter] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL,
	[Increment] float NULL,
	[Mean] float NULL,
	[Count] int NULL,
	[Min] float NULL,
	[Max] float NULL
)
GO

CREATE TABLE [log].[LocalRequest]
(
	[IdLocalRequest] uniqueidentifier NOT NULL,
	[IdRemoteSystem] uniqueidentifier NULL,
	[RemoteIp] nvarchar(63) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[CorrelationId] uniqueidentifier NOT NULL,
	[ExternalCorrelationId] nvarchar(127) NULL,
	[SourceClientIdentifier] nvarchar(127) NOT NULL,
	[Url] nvarchar(2047) NOT NULL,
	[Path] nvarchar(1023) NULL,
	[QueryString] nvarchar(1023) NULL,
	[Method] nvarchar(15) NULL,
	[Headers] nvarchar(max) NULL,
	[ContentType] nvarchar(255) NULL,
	[Metadata] nvarchar(max) NULL,
	[CustomCorrelationId] nvarchar(511) NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [log].[LocalRequestPayload]
(
	[IdLocalRequestPayload] uniqueidentifier NOT NULL,
	[IdLocalRequest] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NULL,
	[RequestContentType] nvarchar(127) NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[ContentHeaders] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[FileName] nvarchar(511) NULL,
	[RelativePath] nvarchar(1023) NULL,
	[Metadata] nvarchar(max) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[MediaType] nvarchar(255) NULL,
	[MultipartFormDataContentName] nvarchar(511) NULL,
	[MultipartFormDataFileName] nvarchar(511) NULL,
	[JsonInputCSharpType] nvarchar(1023) NULL
)
GO

CREATE TABLE [log].[LocalResponse]
(
	[IdLocalResponse] uniqueidentifier NOT NULL,
	[IdLocalRequest] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[CorrelationId] uniqueidentifier NOT NULL,
	[ExternalCorrelationId] nvarchar(127) NULL,
	[StatusCode] nvarchar(63) NULL,
	[Reason] nvarchar(511) NULL,
	[Headers] nvarchar(max) NULL,
	[ContentType] nvarchar(255) NULL,
	[Error] nvarchar(max) NULL,
	[ElapsedMilliseconds] numeric NULL,
	[Metadata] nvarchar(max) NULL,
	[CustomCorrelationId] nvarchar(511) NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [log].[LocalResponsePayload]
(
	[IdLocalResponsePayload] uniqueidentifier NOT NULL,
	[IdLocalResponse] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ResponseContentType] nvarchar(63) NOT NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[ContentHeaders] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[FileName] nvarchar(511) NULL,
	[RelativePath] nvarchar(1023) NULL,
	[Metadata] nvarchar(max) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[MediaType] nvarchar(255) NULL,
	[MultipartFormDataContentName] nvarchar(511) NULL,
	[MultipartFormDataFileName] nvarchar(511) NULL,
	[JsonInputCSharpType] nvarchar(1023) NULL
)
GO

CREATE TABLE [log].[Log]
(
	[IdLog] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[InternalMessage] nvarchar(max) NULL,
	[ClientMessage] nvarchar(max) NULL,
	[Detail] nvarchar(max) NULL,
	[StackTrace] nvarchar(max) NULL,
	[Component] nvarchar(511) NULL,
	[OperationName] nvarchar(1023) NULL,
	[AggregateName] nvarchar(255) NULL,
	[AggregateIdentifier] nvarchar(511) NULL,
	[CustomCorrelationId] nvarchar(511) NULL,
	[IdApplicationEntry] uniqueidentifier NULL,
	[CorrelationId] uniqueidentifier NULL,
	[ExternalCorrelationId] nvarchar(511) NULL,
	[ContextProperties] nvarchar(max) NULL,
	[IdUser] uniqueidentifier NULL,
	[TenantIdentifier] uniqueidentifier NULL,
	[IdLogLevel] int NOT NULL,
	[LogCode] nvarchar(63) NULL,
	[SourceSystemName] nvarchar(1023) NULL,
	[TraceCorrelationId] uniqueidentifier NULL,
	[TraceFrame] nvarchar(max) NULL,
	[SourceContext] nvarchar(max) NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL,
	[IsValidationError] bit NOT NULL,
	[PropertyName] nvarchar(255) NULL,
	[DisplayPropertyName] nvarchar(255) NULL,
	[ValidationFailure] nvarchar(max) NULL
)
GO

CREATE TABLE [log].[LogLevel]
(
	[IdLogLevel] uniqueidentifier NOT NULL,
	[Code] nvarchar(31) NOT NULL,
	[Name] nvarchar(31) NOT NULL,
	[ItemCode] int NOT NULL
)
GO

CREATE TABLE [log].[RemoteRequest]
(
	[IdRemoteRequest] uniqueidentifier NOT NULL,
	[IdRemoteSystem] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[CorrelationId] uniqueidentifier NOT NULL,
	[ExternalCorrelationId] nvarchar(127) NULL,
	[SourceClientIdentifier] nvarchar(127) NOT NULL,
	[Url] nvarchar(2047) NOT NULL,
	[Method] nvarchar(15) NULL,
	[Headers] nvarchar(max) NULL,
	[ContentType] nvarchar(255) NULL,
	[Metadata] nvarchar(max) NULL,
	[CustomCorrelationId] nvarchar(511) NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [log].[RemoteRequestPayload]
(
	[IdRemoteRequestPayload] uniqueidentifier NOT NULL,
	[IdRemoteRequest] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NULL,
	[RequestContentType] nvarchar(127) NOT NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[ContentHeaders] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[FileName] nvarchar(511) NULL,
	[RelativePath] nvarchar(1023) NULL,
	[Metadata] nvarchar(max) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[MediaType] nvarchar(255) NULL,
	[MultipartFormDataContentName] nvarchar(511) NULL,
	[MultipartFormDataFileName] nvarchar(511) NULL,
	[JsonInputCSharpType] nvarchar(1023) NULL
)
GO

CREATE TABLE [log].[RemoteResponse]
(
	[IdRemoteResponse] uniqueidentifier NOT NULL,
	[IdRemoteRequest] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[CorrelationId] uniqueidentifier NOT NULL,
	[ExternalCorrelationId] nvarchar(127) NULL,
	[StatusCode] nvarchar(63) NULL,
	[Reason] nvarchar(511) NULL,
	[Headers] nvarchar(max) NULL,
	[ContentType] nvarchar(255) NULL,
	[Error] nvarchar(max) NULL,
	[ElapsedMilliseconds] numeric NULL,
	[Metadata] nvarchar(max) NULL,
	[CustomCorrelationId] nvarchar(511) NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [log].[RemoteResponsePayload]
(
	[IdRemoteResponsePayload] uniqueidentifier NOT NULL,
	[IdRemoteResponse] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NULL,
	[ResponseContentType] nvarchar(63) NOT NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[ContentHeaders] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[FileName] nvarchar(511) NULL,
	[RelativePath] nvarchar(1023) NULL,
	[Metadata] nvarchar(max) NULL,
	[IsCompressed] bit NULL,
	[EncryptionKey] nvarchar(max) NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[MediaType] nvarchar(255) NULL,
	[MultipartFormDataContentName] nvarchar(511) NULL,
	[MultipartFormDataFileName] nvarchar(511) NULL,
	[JsonInputCSharpType] nvarchar(1023) NULL
)
GO

CREATE TABLE [log].[RemoteSystem]
(
	[IdRemoteSystem] uniqueidentifier NOT NULL,
	[Code] nvarchar(127) NOT NULL,
	[Name] nvarchar(127) NOT NULL
)
GO

CREATE TABLE [log].[UnstructuredLog]
(
	[IdUnstructuredLog] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdLogLevel] int NOT NULL,
	[Message] nvarchar(max) NULL,
	[StackTrace] nvarchar(max) NULL,
	[SourceContext] nvarchar(max) NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL,
	[EventName] nvarchar(511) NULL,
	[EventId] int NULL
)
GO

ALTER TABLE [log].[EnvironmentInfo] 
 ADD CONSTRAINT [PK_EnvironmentInfo]
	PRIMARY KEY CLUSTERED ([IdEnvironmentInfo] ASC)
GO

ALTER TABLE [log].[EventCounter] 
 ADD CONSTRAINT [PK_EventCounter]
	PRIMARY KEY CLUSTERED ([IdEventCounter] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_EventCounter_EventCounterCategory] 
 ON [log].[EventCounter] ([IdEventCounterCategory] ASC)
GO

ALTER TABLE [log].[EventCounterCategory] 
 ADD CONSTRAINT [PK_EventCounterCategory]
	PRIMARY KEY CLUSTERED ([IdEventCounterCategory] ASC)
GO

ALTER TABLE [log].[EventCounterData] 
 ADD CONSTRAINT [PK_EventCounterData]
	PRIMARY KEY CLUSTERED ([IdEventCounterData] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_EventCounterData_EventCounter] 
 ON [log].[EventCounterData] ([IdEventCounter] ASC)
GO

ALTER TABLE [log].[LocalRequest] 
 ADD CONSTRAINT [PK_LocalRequest]
	PRIMARY KEY CLUSTERED ([IdLocalRequest] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_LocalRequest_RemoteSystem] 
 ON [log].[LocalRequest] ([IdRemoteSystem] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_LocalRequest_CorrelationId] 
 ON [log].[LocalRequest] ([CorrelationId] ASC)
GO

ALTER TABLE [log].[LocalRequestPayload] 
 ADD CONSTRAINT [PK_LocalRequestPayload]
	PRIMARY KEY CLUSTERED ([IdLocalRequestPayload] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_LocalRequestPayload_LocalRequest] 
 ON [log].[LocalRequestPayload] ([IdLocalRequest] ASC)
GO

ALTER TABLE [log].[LocalResponse] 
 ADD CONSTRAINT [PK_LocalResponse]
	PRIMARY KEY CLUSTERED ([IdLocalResponse] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_LocalResponse_LocalRequest] 
 ON [log].[LocalResponse] ([IdLocalRequest] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_LocalResponse_CorrelationId] 
 ON [log].[LocalResponse] ([CorrelationId] ASC)
GO

ALTER TABLE [log].[LocalResponsePayload] 
 ADD CONSTRAINT [PK_LocalResponsePayload]
	PRIMARY KEY CLUSTERED ([IdLocalResponsePayload] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_LocalResponsePayload_LocalResponse] 
 ON [log].[LocalResponsePayload] ([IdLocalResponse] ASC)
GO

ALTER TABLE [log].[Log] 
 ADD CONSTRAINT [PK_Log]
	PRIMARY KEY CLUSTERED ([IdLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Log_IdLogLevel] 
 ON [log].[Log] ([IdLogLevel] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Log_CorrelationId] 
 ON [log].[Log] ([CorrelationId] ASC)
GO

ALTER TABLE [log].[LogLevel] 
 ADD CONSTRAINT [PK_LogLevel]
	PRIMARY KEY CLUSTERED ([IdLogLevel] ASC)
GO

ALTER TABLE [log].[LogLevel] 
 ADD CONSTRAINT [UQ_LogLevel_ItemCode] UNIQUE NONCLUSTERED ([ItemCode] ASC)
GO

ALTER TABLE [log].[RemoteRequest] 
 ADD CONSTRAINT [PK_RemoteRequest]
	PRIMARY KEY CLUSTERED ([IdRemoteRequest] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_RemoteRequest_RemoteSystem] 
 ON [log].[RemoteRequest] ([IdRemoteSystem] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_RemoteRequest_CorrelationId] 
 ON [log].[RemoteRequest] ([CorrelationId] ASC)
GO

ALTER TABLE [log].[RemoteRequestPayload] 
 ADD CONSTRAINT [PK_RemoteRequestPayload]
	PRIMARY KEY CLUSTERED ([IdRemoteRequestPayload] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_RemoteRequestPayload_RemoteRequest] 
 ON [log].[RemoteRequestPayload] ([IdRemoteRequest] ASC)
GO

ALTER TABLE [log].[RemoteResponse] 
 ADD CONSTRAINT [PK_RemoteResponse]
	PRIMARY KEY CLUSTERED ([IdRemoteResponse] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_RemoteResponse_RemoteRequest] 
 ON [log].[RemoteResponse] ([IdRemoteRequest] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_RemoteResponse_CorrelationId] 
 ON [log].[RemoteResponse] ([CorrelationId] ASC)
GO

ALTER TABLE [log].[RemoteResponsePayload] 
 ADD CONSTRAINT [PK_RemoteResponsePayload]
	PRIMARY KEY CLUSTERED ([IdRemoteResponsePayload] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_RemoteResponsePayload_RemoteResponse] 
 ON [log].[RemoteResponsePayload] ([IdRemoteResponse] ASC)
GO

ALTER TABLE [log].[RemoteSystem] 
 ADD CONSTRAINT [PK_RemoteSystem]
	PRIMARY KEY CLUSTERED ([IdRemoteSystem] ASC)
GO

ALTER TABLE [log].[RemoteSystem] 
 ADD CONSTRAINT [UQ_RemoteSystem_Code] UNIQUE NONCLUSTERED ([Code] ASC)
GO

ALTER TABLE [log].[UnstructuredLog] 
 ADD CONSTRAINT [PK_UnstructuredLog]
	PRIMARY KEY CLUSTERED ([IdUnstructuredLog] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_UnstructuredLog_LogLevel] 
 ON [log].[UnstructuredLog] ([IdLogLevel] ASC)
GO

ALTER TABLE [log].[EventCounter] ADD CONSTRAINT [FK_EventCounter_IdEventCounterCategory]
	FOREIGN KEY ([IdEventCounterCategory]) REFERENCES [log].[EventCounterCategory] ([IdEventCounterCategory]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[EventCounterData] ADD CONSTRAINT [FK_EventCounterData_IdEventCounter]
	FOREIGN KEY ([IdEventCounter]) REFERENCES [log].[EventCounter] ([IdEventCounter]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[LocalRequest] ADD CONSTRAINT [FK_LocalRequest_IdRemoteSystem]
	FOREIGN KEY ([IdRemoteSystem]) REFERENCES [log].[RemoteSystem] ([IdRemoteSystem]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[LocalRequestPayload] ADD CONSTRAINT [FK_LocalRequestPayload_IdLocalRequest]
	FOREIGN KEY ([IdLocalRequest]) REFERENCES [log].[LocalRequest] ([IdLocalRequest]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[LocalResponse] ADD CONSTRAINT [FK_LocalResponse_IdLocalRequest]
	FOREIGN KEY ([IdLocalRequest]) REFERENCES [log].[LocalRequest] ([IdLocalRequest]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[LocalResponsePayload] ADD CONSTRAINT [FK_LocalResponsePayload_IdLocalResponse]
	FOREIGN KEY ([IdLocalResponse]) REFERENCES [log].[LocalResponse] ([IdLocalResponse]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[RemoteRequest] ADD CONSTRAINT [FK_RemoteRequest_IdRemoteSystem]
	FOREIGN KEY ([IdRemoteSystem]) REFERENCES [log].[RemoteSystem] ([IdRemoteSystem]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[RemoteRequestPayload] ADD CONSTRAINT [FK_RemoteRequestPayload_IdRemoteRequest]
	FOREIGN KEY ([IdRemoteRequest]) REFERENCES [log].[RemoteRequest] ([IdRemoteRequest]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[RemoteResponse] ADD CONSTRAINT [FK_RemoteResponse_IdRemoteRequest]
	FOREIGN KEY ([IdRemoteRequest]) REFERENCES [log].[RemoteRequest] ([IdRemoteRequest]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [log].[RemoteResponsePayload] ADD CONSTRAINT [FK_RemoteResponsePayload_IdRemoteResponse]
	FOREIGN KEY ([IdRemoteResponse]) REFERENCES [log].[RemoteResponse] ([IdRemoteResponse]) ON DELETE No Action ON UPDATE No Action
GO
