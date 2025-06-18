CREATE TABLE [auth].[ExternalLogin]
(
	[IdExternalLogin] uniqueidentifier NOT NULL,
	[IdLoginProvider] uniqueidentifier NOT NULL,
	[IdUser] uniqueidentifier NOT NULL,
	[ExternalUserIdentifier] nvarchar(1024) NOT NULL,
	[Data] nvarchar(max) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ValidToUtc] datetime2(7) NOT NULL,
	[LastAccessUtc] datetime2(7) NULL,
	[RemoteIP] nvarchar(64) NULL
)
GO

CREATE TABLE [auth].[LoginProvider]
(
	[IdLoginProvider] uniqueidentifier NOT NULL,
	[Code] nvarchar(128) NOT NULL,
	[Name] nvarchar(128) NOT NULL,
	[DisabledUtc] datetime2(7) NULL
)
GO

CREATE TABLE [auth].[Permission]
(
	[IdPermission] uniqueidentifier NOT NULL,
	[Code] nvarchar(256) NOT NULL,
	[Name] nvarchar(1024) NOT NULL,
	[Description] nvarchar(max) NULL,
	[ClaimValue] nvarchar(1024) NULL,
	[IsSystemPermission] bit NOT NULL
)
GO

CREATE TABLE [auth].[Role]
(
	[IdRole] uniqueidentifier NOT NULL,
	[Name] nvarchar(256) NOT NULL,
	[NormalizedName] nvarchar(256) NOT NULL,
	[ADGroupDistinguishedName] nvarchar(max) NULL,
	[Data] nvarchar(max) NULL,
	[Description] nvarchar(max) NULL,
	[HasConstantPermissions] bit NOT NULL,
	[HasConstantUsers] bit NOT NULL,
	[IsSystemRole] bit NOT NULL,
	[AuditCreatedUtc] datetime2(7) NOT NULL,
	[AuditModifiedUtc] datetime2(7) NULL,
	[IdAuditCreatedBy] uniqueidentifier NULL,
	[IdAuditModifiedBy] uniqueidentifier NULL,
	[ConcurrencyToken] uniqueidentifier NOT NULL,
	[DeletedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [auth].[RolePermission]
(
	[IdRolePermission] uniqueidentifier NOT NULL,
	[IdRole] uniqueidentifier NOT NULL,
	[IdPermission] uniqueidentifier NOT NULL,
	[AuditCreatedUtc] datetime2(7) NOT NULL,
	[AuditModifiedUtc] datetime2(7) NULL,
	[IdAuditCreatedBy] uniqueidentifier NULL,
	[IdAuditModifiedBy] uniqueidentifier NULL,
	[ConcurrencyToken] uniqueidentifier NOT NULL,
	[DeletedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [auth].[User]
(
	[IdUser] uniqueidentifier NOT NULL,
	[Login] nvarchar(256) NULL,
	[NormalizedLogin] nvarchar(256) NULL,
	[TenantIdentifier] uniqueidentifier NULL,
	[Email] nvarchar(256) NULL,
	[NormalizedEmail] nvarchar(256) NULL,
	[EmailConfirmed] bit NOT NULL,
	[PasswordHash] nvarchar(max) NULL,
	[SecurityStamp] nvarchar(max) NULL,
	[ADDistinguishedName] nvarchar(max) NULL,
	[Data] nvarchar(max) NULL,
	[PhoneNumber] nvarchar(256) NULL,
	[PhoneNumberConfirmed] bit NOT NULL,
	[MultiFactorEnabled] bit NOT NULL,
	[LockoutEndUtc] datetime2(7) NULL,
	[LockoutEnabled] bit NOT NULL,
	[AccessFailedCount] int NOT NULL,
	[IsSystemUser] bit NOT NULL,
	[ConfirmationUrlSlug] nvarchar(max) NULL,
	[ConfirmationUrlValidToUtc] datetime2(7) NULL,
	[AuditCreatedUtc] datetime2(7) NOT NULL,
	[AuditModifiedUtc] datetime2(7) NULL,
	[IdAuditCreatedBy] uniqueidentifier NULL,
	[IdAuditModifiedBy] uniqueidentifier NULL,
	[ConcurrencyToken] uniqueidentifier NOT NULL,
	[DeletedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [auth].[UserPermission]
(
	[IdUserPermission] uniqueidentifier NOT NULL,
	[IdUser] uniqueidentifier NOT NULL,
	[IdPermission] uniqueidentifier NOT NULL,
	[TenantIdentifier] uniqueidentifier NOT NULL,
	[AuditCreatedUtc] datetime2(7) NOT NULL,
	[AuditModifiedUtc] datetime2(7) NULL,
	[IdAuditCreatedBy] uniqueidentifier NULL,
	[IdAuditModifiedBy] uniqueidentifier NULL,
	[ConcurrencyToken] uniqueidentifier NOT NULL,
	[DeletedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [auth].[UserRole]
(
	[IdUserRole] uniqueidentifier NOT NULL,
	[IdUser] uniqueidentifier NOT NULL,
	[IdRole] uniqueidentifier NOT NULL,
	[TenantIdentifier] uniqueidentifier NOT NULL,
	[AuditCreatedUtc] datetime2(7) NOT NULL,
	[AuditModifiedUtc] datetime2(7) NULL,
	[IdAuditCreatedBy] uniqueidentifier NULL,
	[IdAuditModifiedBy] uniqueidentifier NULL,
	[ConcurrencyToken] uniqueidentifier NOT NULL,
	[DeletedUtc] datetime2(7) NOT NULL
)
GO

CREATE TABLE [auth].[UserToken]
(
	[IdUserToken] uniqueidentifier NOT NULL,
	[IdLoginProvider] uniqueidentifier NOT NULL,
	[IdUser] uniqueidentifier NOT NULL,
	[Name] nvarchar(max) NOT NULL,
	[Value] nvarchar(4000) NOT NULL,
	[Data] nvarchar(max) NULL,
	[CreatedUtc] datetime2(7) NOT NULL,
	[ModifiedUtc] datetime2(7) NULL,
	[ValidToUtc] datetime2(7) NOT NULL,
	[LastAccessUtc] datetime2(7) NULL,
	[RemoteIP] nvarchar(64) NULL
)
GO

ALTER TABLE [auth].[ExternalLogin] 
 ADD CONSTRAINT [PK_ExternalLogin]
	PRIMARY KEY CLUSTERED ([IdExternalLogin] ASC)
GO

ALTER TABLE [auth].[ExternalLogin] 
 ADD CONSTRAINT [UQ_ExternalLogin_IdProvider_IdUser_Identifier] UNIQUE NONCLUSTERED ([IdLoginProvider] ASC,[IdUser] ASC,[ExternalUserIdentifier] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_ExternalLogin_LoginProvider] 
 ON [auth].[ExternalLogin] ([IdLoginProvider] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_ExternalLogin_User] 
 ON [auth].[ExternalLogin] ([IdUser] ASC)
GO

ALTER TABLE [auth].[LoginProvider] 
 ADD CONSTRAINT [PK_LoginProvider]
	PRIMARY KEY CLUSTERED ([IdLoginProvider] ASC)
GO

ALTER TABLE [auth].[LoginProvider] 
 ADD CONSTRAINT [UQ_LoginProvider_Code] UNIQUE NONCLUSTERED ([Code] ASC)
GO

ALTER TABLE [auth].[Permission] 
 ADD CONSTRAINT [PK_Permission]
	PRIMARY KEY CLUSTERED ([IdPermission] ASC)
GO

ALTER TABLE [auth].[Permission] 
 ADD CONSTRAINT [UQ_Permission_Code] UNIQUE NONCLUSTERED ([Code] ASC)
GO

ALTER TABLE [auth].[Role] 
 ADD CONSTRAINT [PK_Role]
	PRIMARY KEY CLUSTERED ([IdRole] ASC)
GO

ALTER TABLE [auth].[Role] 
 ADD CONSTRAINT [UQ_Role_Name_Deleted] UNIQUE NONCLUSTERED ([Name] ASC,[DeletedUtc] ASC)
GO

ALTER TABLE [auth].[RolePermission] 
 ADD CONSTRAINT [PK_RolePermission]
	PRIMARY KEY CLUSTERED ([IdRolePermission] ASC)
GO

ALTER TABLE [auth].[RolePermission] 
 ADD CONSTRAINT [UQ_RolePermission_Role_Permission_Deleted] UNIQUE NONCLUSTERED ([IdRole] ASC,[IdPermission] ASC,[DeletedUtc] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_RolePermission_Permission] 
 ON [auth].[RolePermission] ([IdPermission] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_RolePermission_Role] 
 ON [auth].[RolePermission] ([IdRole] ASC)
GO

ALTER TABLE [auth].[User] 
 ADD CONSTRAINT [PK_User]
	PRIMARY KEY CLUSTERED ([IdUser] ASC)
GO

ALTER TABLE [auth].[User] 
 ADD CONSTRAINT [UQ_User_Login_Deleted] UNIQUE NONCLUSTERED ([Login] ASC,[DeletedUtc] ASC)
GO

ALTER TABLE [auth].[UserPermission] 
 ADD CONSTRAINT [PK_UserPermission]
	PRIMARY KEY CLUSTERED ([IdUserPermission] ASC)
GO

ALTER TABLE [auth].[UserPermission] 
 ADD CONSTRAINT [UQ_UserPermission_User_Permission_Tenant_Deleted] UNIQUE NONCLUSTERED ([IdUser] ASC,[IdPermission] ASC,[TenantIdentifier] ASC,[DeletedUtc] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_UserPermission_Permission] 
 ON [auth].[UserPermission] ([IdPermission] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_UserPermission_User] 
 ON [auth].[UserPermission] ([IdUser] ASC)
GO

ALTER TABLE [auth].[UserRole] 
 ADD CONSTRAINT [PK_UserRole]
	PRIMARY KEY CLUSTERED ([IdUserRole] ASC)
GO

ALTER TABLE [auth].[UserRole] 
 ADD CONSTRAINT [UQ_UserRole_User_Role_Tenant_Deleted] UNIQUE NONCLUSTERED ([IdUser] ASC,[IdRole] ASC,[TenantIdentifier] ASC,[DeletedUtc] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_UserRole_Role] 
 ON [auth].[UserRole] ([IdRole] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_UserRole_User] 
 ON [auth].[UserRole] ([IdUser] ASC)
GO

ALTER TABLE [auth].[UserToken] 
 ADD CONSTRAINT [PK_UserToken]
	PRIMARY KEY CLUSTERED ([IdUserToken] ASC)
GO

ALTER TABLE [auth].[UserToken] 
 ADD CONSTRAINT [UQ_UserToken_IdProvider_IdUser_Value] UNIQUE NONCLUSTERED ([IdLoginProvider] ASC,[IdUser] ASC,[Value] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_UserToken_LoginProvider] 
 ON [auth].[UserToken] ([IdLoginProvider] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_UserToken_User] 
 ON [auth].[UserToken] ([IdUser] ASC)
GO

ALTER TABLE [auth].[ExternalLogin] ADD CONSTRAINT [FK_ExternalLogin_IdLoginProvider]
	FOREIGN KEY ([IdLoginProvider]) REFERENCES [auth].[LoginProvider] ([IdLoginProvider]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[ExternalLogin] ADD CONSTRAINT [FK_ExternalLogin_IdUser]
	FOREIGN KEY ([IdUser]) REFERENCES [auth].[User] ([IdUser]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[RolePermission] ADD CONSTRAINT [FK_RolePermission_IdPermission]
	FOREIGN KEY ([IdPermission]) REFERENCES [auth].[Permission] ([IdPermission]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[RolePermission] ADD CONSTRAINT [FK_RolePermission_IdRole]
	FOREIGN KEY ([IdRole]) REFERENCES [auth].[Role] ([IdRole]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[UserPermission] ADD CONSTRAINT [FK_UserPermission_IdPermission]
	FOREIGN KEY ([IdPermission]) REFERENCES [auth].[Permission] ([IdPermission]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[UserPermission] ADD CONSTRAINT [FK_UserPermission_IdUser]
	FOREIGN KEY ([IdUser]) REFERENCES [auth].[User] ([IdUser]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[UserRole] ADD CONSTRAINT [FK_UserRole_IdRole]
	FOREIGN KEY ([IdRole]) REFERENCES [auth].[Role] ([IdRole]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[UserRole] ADD CONSTRAINT [FK_UserRole_IdUser]
	FOREIGN KEY ([IdUser]) REFERENCES [auth].[User] ([IdUser]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[UserToken] ADD CONSTRAINT [FK_UserToken_IdLoginProvider]
	FOREIGN KEY ([IdLoginProvider]) REFERENCES [auth].[LoginProvider] ([IdLoginProvider]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [auth].[UserToken] ADD CONSTRAINT [FK_UserToken_IdUser]
	FOREIGN KEY ([IdUser]) REFERENCES [auth].[User] ([IdUser]) ON DELETE No Action ON UPDATE No Action
GO
