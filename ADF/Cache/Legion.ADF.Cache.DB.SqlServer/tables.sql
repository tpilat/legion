CREATE TABLE [cache].[ReloadableCacheKey]
(
	[IdReloadableCacheKey] uniqueidentifier NOT NULL,
	[Key] nvarchar(max) NULL,
	[Tags] nvarchar(max) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ReloadAtUtc] datetime2 NOT NULL
)
GO

ALTER TABLE [cache].[ReloadableCacheKey] 
 ADD CONSTRAINT [PK_ReloadableCacheKey]
	PRIMARY KEY CLUSTERED ([IdReloadableCacheKey] ASC)
GO
