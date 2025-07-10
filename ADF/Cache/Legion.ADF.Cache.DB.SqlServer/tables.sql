CREATE TABLE [cache].[CacheData]
(
	[KeyHash] nvarchar(32) NOT NULL,
	[ValueHash] nvarchar(32) NOT NULL,
	[Key] nvarchar(max) NOT NULL,
	[Value] nvarchar(max) NOT NULL,
	[KeyPrefix450] nvarchar(450) NOT NULL,
	[ExpiresUtc] datetime2(7) NULL,
	[SlidingTime] time(7) NULL,
	[LastAccessedUtc] datetime2(7) NOT NULL,
	[RowVersion] bigint NOT NULL
)
GO

CREATE TABLE [cache].[DistributedLock]
(
	[KeyHash] nvarchar(32) NOT NULL,
	[LockKey] nvarchar(max) NOT NULL,
	[LockId] nvarchar(32) NOT NULL,
	[Metadata] nvarchar(max) NULL,
	[ExpiresUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [cache].[ReloadableCacheKey]
(
	[IdReloadableCacheKey] uniqueidentifier NOT NULL,
	[Key] nvarchar(max) NULL,
	[Tags] nvarchar(max) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ReloadAtUtc] datetime2 NOT NULL
)
GO

ALTER TABLE [cache].[CacheData] 
 ADD CONSTRAINT [PK_CacheData]
	PRIMARY KEY CLUSTERED ([KeyHash] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_CacheData_KeyPrefix] 
 ON [cache].[CacheData] ([KeyPrefix450] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_CacheData_ExpiresUtc] 
 ON [cache].[CacheData] ([ExpiresUtc] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_CacheData_ValueHash] 
 ON [cache].[CacheData] ([ValueHash] ASC)
GO

ALTER TABLE [cache].[DistributedLock] 
 ADD CONSTRAINT [PK_DistributedLock]
	PRIMARY KEY CLUSTERED ([KeyHash] ASC)
GO

ALTER TABLE [cache].[ReloadableCacheKey] 
 ADD CONSTRAINT [PK_ReloadableCacheKey]
	PRIMARY KEY CLUSTERED ([IdReloadableCacheKey] ASC)
GO
