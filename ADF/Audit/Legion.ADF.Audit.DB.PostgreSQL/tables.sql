CREATE TABLE aud."ApplicationEntry"
(
	"IdApplicationEntry" uuid NOT NULL,
	"IdApplicationEntryToken" uuid NOT NULL,
	"IdAuditOperation" uuid NOT NULL,
	"RuntimeUniqueKey" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"CorrelationId" uuid NULL,
	"ExternalCorrelationId" varchar(127) NULL,
	"AggregateIdentifier" varchar(511) NULL,
	"HttpMethod" varchar(15) NULL,
	"Uri" varchar(1023) NULL,
	"IdUser" uuid NULL,
	"TenantIdentifier" uuid NULL,
	"RemoteIP" varchar(63) NULL
);

CREATE TABLE aud."ApplicationEntryRequest"
(
	"IdApplicationEntryRequest" uuid NOT NULL,
	"IdApplicationEntry" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"Metadata" jsonb NULL,
	"MimeType" varchar(1023) NOT NULL,
	"ContentEncoding" varchar(63) NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL
);

CREATE TABLE aud."ApplicationEntryResponse"
(
	"IdApplicationEntryResponse" uuid NOT NULL,
	"IdApplicationEntry" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ElapsedMilliseconds" numeric NOT NULL,
	"StatusCode" varchar(63) NULL,
	"Metadata" jsonb NULL,
	"Error" text NULL,
	"MimeType" varchar(1023) NOT NULL,
	"ContentEncoding" varchar(63) NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"DbOid" bigint NULL,
	"Name" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL
);

CREATE TABLE aud."ApplicationEntryToken"
(
	"IdApplicationEntryToken" uuid NOT NULL,
	"Token" varchar(255) NOT NULL,
	"SourceFilePath" varchar(511) NOT NULL,
	"MethodInfo" varchar(511) NULL,
	"AggregateName" varchar(255) NULL,
	"Description" varchar(511) NULL
);

CREATE TABLE aud."AuditEntry"
(
	"IdAuditEntry" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdAuditOperation" uuid NOT NULL,
	"TableName" varchar(255) NOT NULL,
	"IdUser" uuid NULL,
	"PrimaryKey" jsonb NULL,
	"OldValues" jsonb NULL,
	"NewValues" jsonb NULL,
	"AffectedColumns" jsonb NULL,
	"AuditCorrelationId" uuid NOT NULL,
	"TraceFrame" text NULL,
	"CorrelationId" uuid NULL
);

CREATE TABLE aud."AuditOperation"
(
	"IdAuditOperation" uuid NOT NULL,
	"Code" varchar(15) NOT NULL,
	"Name" varchar(15) NOT NULL
);

ALTER TABLE aud."ApplicationEntry" ADD CONSTRAINT "PK_ApplicationEntry"
	PRIMARY KEY ("IdApplicationEntry");

CREATE INDEX "IXFK_ApplicationEntry_ApplicationEntryToken" ON aud."ApplicationEntry" ("IdApplicationEntryToken" ASC);

CREATE INDEX "IXFK_ApplicationEntry_AuditOperation" ON aud."ApplicationEntry" ("IdAuditOperation" ASC);

CREATE INDEX "IX_ApplicationEntry_CorrelationId" ON aud."ApplicationEntry" ("CorrelationId" ASC);

ALTER TABLE aud."ApplicationEntryRequest" ADD CONSTRAINT "PK_ApplicationEntryRequest"
	PRIMARY KEY ("IdApplicationEntryRequest");

CREATE INDEX "IXFK_ApplicationEntryRequest_ApplicationEntry" ON aud."ApplicationEntryRequest" ("IdApplicationEntry" ASC);

ALTER TABLE aud."ApplicationEntryResponse" ADD CONSTRAINT "PK_ApplicationEntryResponse"
	PRIMARY KEY ("IdApplicationEntryResponse");

CREATE INDEX "IXFK_ApplicationEntryResponse_ApplicationEntry" ON aud."ApplicationEntryResponse" ("IdApplicationEntry" ASC);

ALTER TABLE aud."ApplicationEntryToken" ADD CONSTRAINT "PK_ApplicationEntryToken"
	PRIMARY KEY ("IdApplicationEntryToken");

ALTER TABLE aud."ApplicationEntryToken" 
  ADD CONSTRAINT "UQ_ApplicationEntryToken_Token_SourceFilePath" UNIQUE ("SourceFilePath","Token");

ALTER TABLE aud."AuditEntry" ADD CONSTRAINT "PK_AuditEntry"
	PRIMARY KEY ("IdAuditEntry");

CREATE INDEX "IXFK_AuditEntry_AuditOperation" ON aud."AuditEntry" ("IdAuditOperation" ASC);

CREATE INDEX "IX_AuditEntry_CorrelationId" ON aud."AuditEntry" ("CorrelationId" ASC);

ALTER TABLE aud."AuditOperation" ADD CONSTRAINT "PK_AuditOperation"
	PRIMARY KEY ("IdAuditOperation");

ALTER TABLE aud."ApplicationEntry" ADD CONSTRAINT "FK_ApplicationEntry_IdApplicationEntryToken"
	FOREIGN KEY ("IdApplicationEntryToken") REFERENCES aud."ApplicationEntryToken" ("IdApplicationEntryToken") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE aud."ApplicationEntry" ADD CONSTRAINT "FK_ApplicationEntry_IdAuditOperation"
	FOREIGN KEY ("IdAuditOperation") REFERENCES aud."AuditOperation" ("IdAuditOperation") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE aud."ApplicationEntryRequest" ADD CONSTRAINT "FK_ApplicationEntryRequest_IdApplicationEntry"
	FOREIGN KEY ("IdApplicationEntry") REFERENCES aud."ApplicationEntry" ("IdApplicationEntry") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE aud."ApplicationEntryResponse" ADD CONSTRAINT "FK_ApplicationEntryResponse_IdApplicationEntry"
	FOREIGN KEY ("IdApplicationEntry") REFERENCES aud."ApplicationEntry" ("IdApplicationEntry") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE aud."AuditEntry" ADD CONSTRAINT "FK_AuditEntry_IdAuditOperation"
	FOREIGN KEY ("IdAuditOperation") REFERENCES aud."AuditOperation" ("IdAuditOperation") ON DELETE No Action ON UPDATE No Action;
