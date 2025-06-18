CREATE TABLE [aud].[ApplicationEntry]
(
	[IdApplicationEntry] uniqueidentifier NOT NULL,
	[IdApplicationEntryToken] uniqueidentifier NOT NULL,
	[IdAuditOperation] uniqueidentifier NOT NULL,
	[RuntimeUniqueKey] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[CorrelationId] uniqueidentifier NULL,
	[ExternalCorrelationId] nvarchar(127) NULL,
	[AggregateIdentifier] nvarchar(511) NULL,
	[HttpMethod] nvarchar(15) NULL,
	[Uri] nvarchar(1023) NULL,
	[IdUser] uniqueidentifier NULL,
	[TenantIdentifier] uniqueidentifier NULL,
	[RemoteIP] nvarchar(63) NULL
)
GO

CREATE TABLE [aud].[ApplicationEntryRequest]
(
	[IdApplicationEntryRequest] uniqueidentifier NOT NULL,
	[IdApplicationEntry] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[Metadata] nvarchar(max) NULL,
	[MimeType] nvarchar(1023) NOT NULL,
	[ContentEncoding] nvarchar(63) NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[Name] varchar(511) NULL,
	[RelativePath] varchar(1023) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL
)
GO

CREATE TABLE [aud].[ApplicationEntryResponse]
(
	[IdApplicationEntryResponse] uniqueidentifier NOT NULL,
	[IdApplicationEntry] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ElapsedMilliseconds] numeric(18) NOT NULL,
	[StatusCode] varchar(63) NULL,
	[Metadata] nvarchar(max) NULL,
	[Error] nvarchar(max) NULL,
	[MimeType] nvarchar(1023) NULL,
	[ContentEncoding] varchar(63) NOT NULL,
	[ByteArrayContent] varbinary(max) NULL,
	[JsonContent] nvarchar(max) NULL,
	[StringContent] nvarchar(max) NULL,
	[DbOid] bigint NULL,
	[Name] varchar(511) NULL,
	[RelativePath] varchar(1023) NULL,
	[IsCompressed] bit NOT NULL,
	[EncryptionKey] nvarchar(max) NULL
)
GO

CREATE TABLE [aud].[ApplicationEntryToken]
(
	[IdApplicationEntryToken] uniqueidentifier NOT NULL,
	[Token] nvarchar(255) NOT NULL,
	[SourceFilePath] nvarchar(511) NOT NULL,
	[MethodInfo] nvarchar(511) NULL,
	[AggregateName] nvarchar(255) NULL,
	[Description] nvarchar(511) NULL
)
GO

CREATE TABLE [aud].[AuditEntry]
(
	[IdAuditEntry] uniqueidentifier NOT NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[IdAuditOperation] uniqueidentifier NOT NULL,
	[TableName] nvarchar(255) NOT NULL,
	[IdUser] uniqueidentifier NULL,
	[PrimaryKey] nvarchar(max) NULL,
	[OldValues] nvarchar(max) NULL,
	[NewValues] nvarchar(max) NULL,
	[AffectedColumns] nvarchar(max) NULL,
	[AuditCorrelationId] uniqueidentifier NOT NULL,
	[TraceFrame] nvarchar(max) NULL,
	[CorrelationId] uniqueidentifier NULL
)
GO

CREATE TABLE [aud].[AuditOperation]
(
	[IdAuditOperation] uniqueidentifier NOT NULL,
	[Code] nvarchar(15) NOT NULL,
	[Name] nvarchar(15) NOT NULL
)
GO

ALTER TABLE [aud].[ApplicationEntry] 
 ADD CONSTRAINT [PK_ApplicationEntry]
	PRIMARY KEY CLUSTERED ([IdApplicationEntry] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_ApplicationEntry_ApplicationEntryToken] 
 ON [aud].[ApplicationEntry] ([IdApplicationEntryToken] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_ApplicationEntry_AuditOperation] 
 ON [aud].[ApplicationEntry] ([IdAuditOperation] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ApplicationEntry_CorrelationId] 
 ON [aud].[ApplicationEntry] ([CorrelationId] ASC)
GO

ALTER TABLE [aud].[ApplicationEntryRequest] 
 ADD CONSTRAINT [PK_ApplicationEntryRequest]
	PRIMARY KEY CLUSTERED ([IdApplicationEntryRequest] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_ApplicationEntryRequest_ApplicationEntry] 
 ON [aud].[ApplicationEntryRequest] ([IdApplicationEntry] ASC)
GO

ALTER TABLE [aud].[ApplicationEntryResponse] 
 ADD CONSTRAINT [PK_ApplicationEntryResponse]
	PRIMARY KEY CLUSTERED ([IdApplicationEntryResponse] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_ApplicationEntryResponse_ApplicationEntry] 
 ON [aud].[ApplicationEntryResponse] ([IdApplicationEntry] ASC)
GO

ALTER TABLE [aud].[ApplicationEntryToken] 
 ADD CONSTRAINT [PK_ApplicationEntryToken]
	PRIMARY KEY CLUSTERED ([IdApplicationEntryToken] ASC)
GO

ALTER TABLE [aud].[ApplicationEntryToken] 
 ADD CONSTRAINT [UQ_ApplicationEntryToken_Token_SourceFilePath] UNIQUE NONCLUSTERED ([SourceFilePath] ASC,[Token] ASC)
GO

ALTER TABLE [aud].[AuditEntry] 
 ADD CONSTRAINT [PK_AuditEntry]
	PRIMARY KEY CLUSTERED ([IdAuditEntry] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_AuditEntry_AuditOperation] 
 ON [aud].[AuditEntry] ([IdAuditOperation] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_AuditEntry_CorrelationId] 
 ON [aud].[AuditEntry] ([CorrelationId] ASC)
GO

ALTER TABLE [aud].[AuditOperation] 
 ADD CONSTRAINT [PK_AuditOperation]
	PRIMARY KEY CLUSTERED ([IdAuditOperation] ASC)
GO

ALTER TABLE [aud].[ApplicationEntry] ADD CONSTRAINT [FK_ApplicationEntry_IdApplicationEntryToken]
	FOREIGN KEY ([IdApplicationEntryToken]) REFERENCES [aud].[ApplicationEntryToken] ([IdApplicationEntryToken]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [aud].[ApplicationEntry] ADD CONSTRAINT [FK_ApplicationEntry_IdAuditOperation]
	FOREIGN KEY ([IdAuditOperation]) REFERENCES [aud].[AuditOperation] ([IdAuditOperation]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [aud].[ApplicationEntryRequest] ADD CONSTRAINT [FK_ApplicationEntryRequest_IdApplicationEntry]
	FOREIGN KEY ([IdApplicationEntry]) REFERENCES [aud].[ApplicationEntry] ([IdApplicationEntry]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [aud].[ApplicationEntryResponse] ADD CONSTRAINT [FK_ApplicationEntryResponse_IdApplicationEntry]
	FOREIGN KEY ([IdApplicationEntry]) REFERENCES [aud].[ApplicationEntry] ([IdApplicationEntry]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [aud].[AuditEntry] ADD CONSTRAINT [FK_AuditEntry_IdAuditOperation]
	FOREIGN KEY ([IdAuditOperation]) REFERENCES [aud].[AuditOperation] ([IdAuditOperation]) ON DELETE No Action ON UPDATE No Action
GO
