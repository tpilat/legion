CREATE TABLE auth."ExternalLogin"
(
	"IdExternalLogin" uuid NOT NULL,
	"IdLoginProvider" uuid NOT NULL,
	"IdUser" uuid NOT NULL,
	"ExternalUserIdentifier" text NOT NULL,
	"Data" jsonb NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ValidToUtc" timestamp with time zone NOT NULL,
	"LastAccessUtc" timestamp with time zone NULL,
	"RemoteIP" varchar(64) NULL
);

CREATE TABLE auth."LoginProvider"
(
	"IdLoginProvider" uuid NOT NULL,
	"Code" varchar(128) NOT NULL,
	"Name" varchar(128) NOT NULL,
	"DisabledUtc" timestamp with time zone NULL
);

CREATE TABLE auth."Permission"
(
	"IdPermission" uuid NOT NULL,
	"Code" varchar(256) NOT NULL,
	"Name" varchar(1024) NOT NULL,
	"Description" text NULL,
	"ClaimValue" varchar(1024) NULL,
	"IsSystemPermission" boolean NOT NULL
);

CREATE TABLE auth."Role"
(
	"IdRole" uuid NOT NULL,
	"Name" varchar(256) NOT NULL,
	"NormalizedName" varchar(256) NOT NULL,
	"ADGroupDistinguishedName" text NULL,
	"Data" jsonb NULL,
	"Description" text NULL,
	"HasConstantPermissions" boolean NOT NULL,
	"HasConstantUsers" boolean NOT NULL,
	"IsSystemRole" boolean NOT NULL,
	"AuditCreatedUtc" timestamp with time zone NOT NULL,
	"AuditModifiedUtc" timestamp with time zone NULL,
	"IdAuditCreatedBy" uuid NULL,
	"IdAuditModifiedBy" uuid NULL,
	"ConcurrencyToken" uuid NOT NULL,
	"DeletedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE auth."RolePermission"
(
	"IdRolePermission" uuid NOT NULL,
	"IdRole" uuid NOT NULL,
	"IdPermission" uuid NOT NULL,
	"AuditCreatedUtc" timestamp with time zone NOT NULL,
	"AuditModifiedUtc" timestamp with time zone NULL,
	"IdAuditCreatedBy" uuid NULL,
	"IdAuditModifiedBy" uuid NULL,
	"ConcurrencyToken" uuid NOT NULL,
	"DeletedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE auth."User"
(
	"IdUser" uuid NOT NULL,
	"Login" varchar(256) NULL,
	"NormalizedLogin" varchar(256) NULL,
	"TenantIdentifier" uuid NULL,
	"Email" varchar(256) NULL,
	"NormalizedEmail" varchar(256) NULL,
	"EmailConfirmed" boolean NOT NULL,
	"PasswordHash" text NULL,
	"SecurityStamp" text NULL,
	"ADDistinguishedName" text NULL,
	"Data" jsonb NULL,
	"PhoneNumber" varchar(256) NULL,
	"PhoneNumberConfirmed" boolean NOT NULL,
	"MultiFactorEnabled" boolean NOT NULL,
	"LockoutEndUtc" timestamp with time zone NULL,
	"LockoutEnabled" boolean NOT NULL,
	"AccessFailedCount" integer NOT NULL,
	"IsSystemUser" boolean NOT NULL,
	"ConfirmationUrlSlug" text NULL,
	"ConfirmationUrlValidToUtc" timestamp with time zone NULL,
	"AuditCreatedUtc" timestamp with time zone NOT NULL,
	"AuditModifiedUtc" timestamp with time zone NULL,
	"IdAuditCreatedBy" uuid NULL,
	"IdAuditModifiedBy" uuid NULL,
	"ConcurrencyToken" uuid NOT NULL,
	"DeletedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE auth."UserPermission"
(
	"IdUserPermission" uuid NOT NULL,
	"IdUser" uuid NOT NULL,
	"IdPermission" uuid NOT NULL,
	"TenantIdentifier" uuid NOT NULL,
	"AuditCreatedUtc" timestamp with time zone NOT NULL,
	"AuditModifiedUtc" timestamp with time zone NULL,
	"IdAuditCreatedBy" uuid NULL,
	"IdAuditModifiedBy" uuid NULL,
	"ConcurrencyToken" uuid NOT NULL,
	"DeletedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE auth."UserRole"
(
	"IdUserRole" uuid NOT NULL,
	"IdUser" uuid NOT NULL,
	"IdRole" uuid NOT NULL,
	"TenantIdentifier" uuid NOT NULL,
	"AuditCreatedUtc" timestamp with time zone NOT NULL,
	"AuditModifiedUtc" timestamp with time zone NULL,
	"IdAuditCreatedBy" uuid NULL,
	"IdAuditModifiedBy" uuid NULL,
	"ConcurrencyToken" uuid NOT NULL,
	"DeletedUtc" timestamp with time zone NOT NULL
);

CREATE TABLE auth."UserToken"
(
	"IdUserToken" uuid NOT NULL,
	"IdLoginProvider" uuid NOT NULL,
	"IdUser" uuid NOT NULL,
	"Name" text NOT NULL,
	"Value" text NOT NULL,
	"Data" jsonb NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ModifiedUtc" timestamp with time zone NULL,
	"ValidToUtc" timestamp with time zone NOT NULL,
	"LastAccessUtc" timestamp with time zone NULL,
	"RemoteIP" varchar(64) NULL
);

ALTER TABLE auth."ExternalLogin" ADD CONSTRAINT "PK_ExternalLogin"
	PRIMARY KEY ("IdExternalLogin");

ALTER TABLE auth."ExternalLogin" 
  ADD CONSTRAINT "UQ_ExternalLogin_IdProvider_IdUser_Identifier" UNIQUE ("IdLoginProvider","IdUser","ExternalUserIdentifier");

CREATE INDEX "IXFK_ExternalLogin_LoginProvider" ON auth."ExternalLogin" ("IdLoginProvider" ASC);

CREATE INDEX "IXFK_ExternalLogin_User" ON auth."ExternalLogin" ("IdUser" ASC);

ALTER TABLE auth."LoginProvider" ADD CONSTRAINT "PK_LoginProvider"
	PRIMARY KEY ("IdLoginProvider");

ALTER TABLE auth."LoginProvider" 
  ADD CONSTRAINT "UQ_LoginProvider_Code" UNIQUE ("Code");

ALTER TABLE auth."Permission" ADD CONSTRAINT "PK_Permission"
	PRIMARY KEY ("IdPermission");

ALTER TABLE auth."Permission" 
  ADD CONSTRAINT "UQ_Permission_Code" UNIQUE ("Code");

ALTER TABLE auth."Role" ADD CONSTRAINT "PK_Role"
	PRIMARY KEY ("IdRole");

ALTER TABLE auth."Role" 
  ADD CONSTRAINT "UQ_Role_Name_Deleted" UNIQUE ("Name","DeletedUtc");

ALTER TABLE auth."RolePermission" ADD CONSTRAINT "PK_RolePermission"
	PRIMARY KEY ("IdRolePermission");

ALTER TABLE auth."RolePermission" 
  ADD CONSTRAINT "UQ_RolePermission_Role_Permission_Deleted" UNIQUE ("IdRole","IdPermission","DeletedUtc");

CREATE INDEX "IXFK_RolePermission_Permission" ON auth."RolePermission" ("IdPermission" ASC);

CREATE INDEX "IXFK_RolePermission_Role" ON auth."RolePermission" ("IdRole" ASC);

ALTER TABLE auth."User" ADD CONSTRAINT "PK_User"
	PRIMARY KEY ("IdUser");

ALTER TABLE auth."User" 
  ADD CONSTRAINT "UQ_User_Login_Deleted" UNIQUE ("Login","DeletedUtc");

ALTER TABLE auth."UserPermission" ADD CONSTRAINT "PK_UserPermission"
	PRIMARY KEY ("IdUserPermission");

ALTER TABLE auth."UserPermission" 
  ADD CONSTRAINT "UQ_UserPermission_User_Permission_Tenant_Deleted" UNIQUE ("IdUser","IdPermission","TenantIdentifier","DeletedUtc");

CREATE INDEX "IXFK_UserPermission_Permission" ON auth."UserPermission" ("IdPermission" ASC);

CREATE INDEX "IXFK_UserPermission_User" ON auth."UserPermission" ("IdUser" ASC);

ALTER TABLE auth."UserRole" ADD CONSTRAINT "PK_UserRole"
	PRIMARY KEY ("IdUserRole");

ALTER TABLE auth."UserRole" 
  ADD CONSTRAINT "UQ_UserRole_User_Role_Tenant_Deleted" UNIQUE ("IdUser","IdRole","TenantIdentifier","DeletedUtc");

CREATE INDEX "IXFK_UserRole_Role" ON auth."UserRole" ("IdRole" ASC);

CREATE INDEX "IXFK_UserRole_User" ON auth."UserRole" ("IdUser" ASC);

ALTER TABLE auth."UserToken" ADD CONSTRAINT "PK_UserToken"
	PRIMARY KEY ("IdUserToken");

ALTER TABLE auth."UserToken" 
  ADD CONSTRAINT "UQ_UserToken_IdProvider_IdUser_Value" UNIQUE ("IdLoginProvider","IdUser","Value");

CREATE INDEX "IXFK_UserToken_LoginProvider" ON auth."UserToken" ("IdLoginProvider" ASC);

CREATE INDEX "IXFK_UserToken_User" ON auth."UserToken" ("IdUser" ASC);

ALTER TABLE auth."ExternalLogin" ADD CONSTRAINT "FK_ExternalLogin_IdLoginProvider"
	FOREIGN KEY ("IdLoginProvider") REFERENCES auth."LoginProvider" ("IdLoginProvider") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."ExternalLogin" ADD CONSTRAINT "FK_ExternalLogin_IdUser"
	FOREIGN KEY ("IdUser") REFERENCES auth."User" ("IdUser") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."RolePermission" ADD CONSTRAINT "FK_RolePermission_IdPermission"
	FOREIGN KEY ("IdPermission") REFERENCES auth."Permission" ("IdPermission") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."RolePermission" ADD CONSTRAINT "FK_RolePermission_IdRole"
	FOREIGN KEY ("IdRole") REFERENCES auth."Role" ("IdRole") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."UserPermission" ADD CONSTRAINT "FK_UserPermission_IdPermission"
	FOREIGN KEY ("IdPermission") REFERENCES auth."Permission" ("IdPermission") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."UserPermission" ADD CONSTRAINT "FK_UserPermission_IdUser"
	FOREIGN KEY ("IdUser") REFERENCES auth."User" ("IdUser") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."UserRole" ADD CONSTRAINT "FK_UserRole_IdRole"
	FOREIGN KEY ("IdRole") REFERENCES auth."Role" ("IdRole") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."UserRole" ADD CONSTRAINT "FK_UserRole_IdUser"
	FOREIGN KEY ("IdUser") REFERENCES auth."User" ("IdUser") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."UserToken" ADD CONSTRAINT "FK_UserToken_IdLoginProvider"
	FOREIGN KEY ("IdLoginProvider") REFERENCES auth."LoginProvider" ("IdLoginProvider") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE auth."UserToken" ADD CONSTRAINT "FK_UserToken_IdUser"
	FOREIGN KEY ("IdUser") REFERENCES auth."User" ("IdUser") ON DELETE No Action ON UPDATE No Action;
