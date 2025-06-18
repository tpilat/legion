CREATE TABLE [conf].[ConfigurationClass]
(
	[IdConfigurationClass] uniqueidentifier NOT NULL,
	[RootPath] nvarchar(4000) NOT NULL,
	[DisplayName] nvarchar(255) NOT NULL,
	[Class] nvarchar(max) NULL
)
GO

CREATE TABLE [conf].[ConfigurationKeyValue]
(
	[IdConfigurationKeyValue] uniqueidentifier NOT NULL,
	[Key] nvarchar(4000) NOT NULL,
	[Value] nvarchar(max) NULL,
	[AuditCreatedUtc] datetime2(7) NOT NULL,
	[AuditModifiedUtc] datetime2(7) NULL,
	[IdAuditCreatedBy] uniqueidentifier NULL,
	[IdAuditModifiedBy] uniqueidentifier NULL,
	[ConcurrencyToken] uniqueidentifier NOT NULL
)
GO

ALTER TABLE [conf].[ConfigurationClass] 
 ADD CONSTRAINT [PK_ConfigurationClass]
	PRIMARY KEY CLUSTERED ([IdConfigurationClass] ASC)
GO

ALTER TABLE [conf].[ConfigurationClass] 
 ADD CONSTRAINT [UQ_ConfigurationClass_RootPath] UNIQUE NONCLUSTERED ([RootPath] ASC)
GO

ALTER TABLE [conf].[ConfigurationKeyValue] 
 ADD CONSTRAINT [PK_ConfigurationKeyValue]
	PRIMARY KEY CLUSTERED ([IdConfigurationKeyValue] ASC)
GO

ALTER TABLE [conf].[ConfigurationKeyValue] 
 ADD CONSTRAINT [UQ_ConfigurationKeyValue_Key] UNIQUE NONCLUSTERED ([Key] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ConfigurationKeyValue_Key] 
 ON [conf].[ConfigurationKeyValue] ([Key] ASC)
GO
