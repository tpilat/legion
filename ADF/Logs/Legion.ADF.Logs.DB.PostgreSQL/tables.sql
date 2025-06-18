CREATE TABLE log."EnvironmentInfo"
(
	"IdEnvironmentInfo" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ApplicationName" varchar(127) NULL,
	"ApplicationVersion" varchar(15) NULL,
	"RunningEnvironment" varchar(255) NULL,
	"ProcessName" varchar(255) NULL,
	"ProcessId" integer NULL,
	"FrameworkDescription" varchar(255) NULL,
	"TargetFramework" varchar(255) NULL,
	"CLRVersion" varchar(255) NULL,
	"EntryAssemblyName" varchar(255) NULL,
	"EntryAssemblyVersion" varchar(255) NULL,
	"BaseDirectory" varchar(255) NULL,
	"MachineName" varchar(255) NULL,
	"CurrentAppDomainName" varchar(255) NULL,
	"Is64BitOperatingSystem" boolean NULL,
	"Is64BitProcess" boolean NULL,
	"OperatingSystemArchitecture" varchar(255) NULL,
	"OperatingSystemPlatform" varchar(255) NULL,
	"OperatingSystemVersion" varchar(255) NULL,
	"ProcessArchitecture" varchar(255) NULL,
	"CommandLine" varchar(1023) NULL
);

CREATE TABLE log."EventCounter"
(
	"IdEventCounter" uuid NOT NULL,
	"IdEventCounterCategory" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL,
	"DisplayName" varchar(127) NOT NULL,
	"CounterType" varchar(63) NOT NULL,
	"DisplayRateTimeScale" varchar(31) NULL,
	"Metadata" jsonb NULL,
	"DisplayUnits" varchar(31) NULL
);

CREATE TABLE log."EventCounterCategory"
(
	"IdEventCounterCategory" uuid NOT NULL,
	"Source" varchar(127) NOT NULL,
	"DisplayName" varchar(127) NOT NULL
);

CREATE TABLE log."EventCounterData"
(
	"IdEventCounterData" uuid NOT NULL,
	"IdEventCounter" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"RuntimeUniqueKey" uuid NOT NULL,
	"Increment" double precision NULL,
	"Mean" double precision NULL,
	"Count" integer NULL,
	"Min" double precision NULL,
	"Max" double precision NULL
);

CREATE TABLE log."LocalRequest"
(
	"IdLocalRequest" uuid NOT NULL,
	"IdRemoteSystem" uuid NULL,
	"RemoteIp" varchar(63) NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"CorrelationId" uuid NOT NULL,
	"ExternalCorrelationId" varchar(127) NULL,
	"SourceClientIdentifier" varchar(127) NOT NULL,
	"Url" varchar(2047) NOT NULL,
	"Path" varchar(1023) NULL,
	"QueryString" varchar(1023) NULL,
	"Method" varchar(15) NULL,
	"Headers" jsonb NULL,
	"ContentType" varchar(255) NULL,
	"Metadata" jsonb NULL,
	"CustomCorrelationId" varchar(511) NULL,
	"RuntimeUniqueKey" uuid NOT NULL
);

CREATE TABLE log."LocalRequestPayload"
(
	"IdLocalRequestPayload" uuid NOT NULL,
	"IdLocalRequest" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NULL,
	"RequestContentType" varchar(127) NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"ContentHeaders" jsonb NULL,
	"DbOid" bigint NULL,
	"FileName" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL,
	"ContentEncoding" varchar(63) NULL,
	"MediaType" varchar(255) NULL,
	"MultipartFormDataContentName" varchar(511) NULL,
	"MultipartFormDataFileName" varchar(511) NULL,
	"JsonInputCSharpType" varchar(1023) NULL
);

CREATE TABLE log."LocalResponse"
(
	"IdLocalResponse" uuid NOT NULL,
	"IdLocalRequest" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"CorrelationId" uuid NOT NULL,
	"ExternalCorrelationId" varchar(127) NULL,
	"StatusCode" varchar(63) NULL,
	"Reason" varchar(511) NULL,
	"Headers" jsonb NULL,
	"ContentType" varchar(255) NULL,
	"Error" text NULL,
	"ElapsedMilliseconds" numeric NULL,
	"Metadata" jsonb NULL,
	"CustomCorrelationId" varchar(511) NULL,
	"RuntimeUniqueKey" uuid NOT NULL
);

CREATE TABLE log."LocalResponsePayload"
(
	"IdLocalResponsePayload" uuid NOT NULL,
	"IdLocalResponse" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ResponseContentType" varchar(63) NOT NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"ContentHeaders" jsonb NULL,
	"DbOid" bigint NULL,
	"FileName" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL,
	"ContentEncoding" varchar(63) NULL,
	"MediaType" varchar(255) NULL,
	"MultipartFormDataContentName" varchar(511) NULL,
	"MultipartFormDataFileName" varchar(511) NULL,
	"JsonInputCSharpType" varchar(1023) NULL
);

CREATE TABLE log."Log"
(
	"IdLog" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"InternalMessage" text NULL,
	"ClientMessage" text NULL,
	"Detail" text NULL,
	"StackTrace" text NULL,
	"Component" varchar(511) NULL,
	"OperationName" varchar(1023) NULL,
	"AggregateName" varchar(255) NULL,
	"AggregateIdentifier" varchar(511) NULL,
	"CustomCorrelationId" varchar(511) NULL,
	"IdApplicationEntry" uuid NULL,
	"CorrelationId" uuid NULL,
	"ExternalCorrelationId" varchar(511) NULL,
	"ContextProperties" jsonb NULL,
	"IdUser" uuid NULL,
	"TenantIdentifier" uuid NULL,
	"IdLogLevel" integer NOT NULL,
	"LogCode" varchar(63) NULL,
	"SourceSystemName" varchar(1023) NULL,
	"TraceCorrelationId" uuid NULL,
	"TraceFrame" text NULL,
	"SourceContext" text NULL,
	"RuntimeUniqueKey" uuid NOT NULL,
	"IsValidationError" boolean NOT NULL,
	"PropertyName" varchar(255) NULL,
	"DisplayPropertyName" varchar(255) NULL,
	"ValidationFailure" text NULL
);

CREATE TABLE log."LogLevel"
(
	"IdLogLevel" uuid NOT NULL,
	"Code" varchar(31) NOT NULL,
	"Name" varchar(31) NOT NULL,
	"ItemCode" integer NOT NULL
);

CREATE TABLE log."RemoteRequest"
(
	"IdRemoteRequest" uuid NOT NULL,
	"IdRemoteSystem" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"CorrelationId" uuid NOT NULL,
	"ExternalCorrelationId" varchar(127) NULL,
	"SourceClientIdentifier" varchar(127) NOT NULL,
	"Url" varchar(2047) NOT NULL,
	"Method" varchar(15) NULL,
	"Headers" jsonb NULL,
	"ContentType" varchar(255) NULL,
	"Metadata" jsonb NULL,
	"CustomCorrelationId" varchar(511) NULL,
	"RuntimeUniqueKey" uuid NOT NULL
);

CREATE TABLE log."RemoteRequestPayload"
(
	"IdRemoteRequestPayload" uuid NOT NULL,
	"IdRemoteRequest" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NULL,
	"RequestContentType" varchar(127) NOT NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"ContentHeaders" jsonb NULL,
	"DbOid" bigint NULL,
	"FileName" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NOT NULL,
	"EncryptionKey" text NULL,
	"ContentEncoding" varchar(63) NULL,
	"MediaType" varchar(255) NULL,
	"MultipartFormDataContentName" varchar(511) NULL,
	"MultipartFormDataFileName" varchar(511) NULL,
	"JsonInputCSharpType" varchar(1023) NULL
);

CREATE TABLE log."RemoteResponse"
(
	"IdRemoteResponse" uuid NOT NULL,
	"IdRemoteRequest" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"CorrelationId" uuid NOT NULL,
	"ExternalCorrelationId" varchar(127) NULL,
	"StatusCode" varchar(63) NULL,
	"Reason" varchar(511) NULL,
	"Headers" jsonb NULL,
	"ContentType" varchar(255) NULL,
	"Error" text NULL,
	"ElapsedMilliseconds" numeric NULL,
	"Metadata" jsonb NULL,
	"CustomCorrelationId" varchar(511) NULL,
	"RuntimeUniqueKey" uuid NOT NULL
);

CREATE TABLE log."RemoteResponsePayload"
(
	"IdRemoteResponsePayload" uuid NOT NULL,
	"IdRemoteResponse" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NULL,
	"ResponseContentType" varchar(63) NOT NULL,
	"ByteArrayContent" bytea NULL,
	"JsonContent" jsonb NULL,
	"StringContent" text NULL,
	"ContentHeaders" jsonb NULL,
	"DbOid" bigint NULL,
	"FileName" varchar(511) NULL,
	"RelativePath" varchar(1023) NULL,
	"Metadata" jsonb NULL,
	"IsCompressed" boolean NULL,
	"EncryptionKey" text NULL,
	"ContentEncoding" varchar(63) NULL,
	"MediaType" varchar(255) NULL,
	"MultipartFormDataContentName" varchar(511) NULL,
	"MultipartFormDataFileName" varchar(511) NULL,
	"JsonInputCSharpType" varchar(1023) NULL
);

CREATE TABLE log."RemoteSystem"
(
	"IdRemoteSystem" uuid NOT NULL,
	"Code" varchar(127) NOT NULL,
	"Name" varchar(127) NOT NULL
);

CREATE TABLE log."UnstructuredLog"
(
	"IdUnstructuredLog" uuid NOT NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"IdLogLevel" integer NOT NULL,
	"Message" text NULL,
	"StackTrace" text NULL,
	"SourceContext" text NULL,
	"RuntimeUniqueKey" uuid NOT NULL,
	"EventName" varchar(511) NULL,
	"EventId" integer NULL
);

ALTER TABLE log."EnvironmentInfo" ADD CONSTRAINT "PK_EnvironmentInfo"
	PRIMARY KEY ("IdEnvironmentInfo");

ALTER TABLE log."EventCounter" ADD CONSTRAINT "PK_EventCounter"
	PRIMARY KEY ("IdEventCounter");

CREATE INDEX "IXFK_EventCounter_EventCounterCategory" ON log."EventCounter" ("IdEventCounterCategory" ASC);

ALTER TABLE log."EventCounterCategory" ADD CONSTRAINT "PK_EventCounterCategory"
	PRIMARY KEY ("IdEventCounterCategory");

ALTER TABLE log."EventCounterData" ADD CONSTRAINT "PK_EventCounterData"
	PRIMARY KEY ("IdEventCounterData");

CREATE INDEX "IXFK_EventCounterData_EventCounter" ON log."EventCounterData" ("IdEventCounter" ASC);

ALTER TABLE log."LocalRequest" ADD CONSTRAINT "PK_LocalRequest"
	PRIMARY KEY ("IdLocalRequest");

CREATE INDEX "IXFK_LocalRequest_RemoteSystem" ON log."LocalRequest" ("IdRemoteSystem" ASC);

CREATE INDEX "IX_LocalRequest_CorrelationId" ON log."LocalRequest" ("CorrelationId" ASC);

ALTER TABLE log."LocalRequestPayload" ADD CONSTRAINT "PK_LocalRequestPayload"
	PRIMARY KEY ("IdLocalRequestPayload");

CREATE INDEX "IXFK_LocalRequestPayload_LocalRequest" ON log."LocalRequestPayload" ("IdLocalRequest" ASC);

ALTER TABLE log."LocalResponse" ADD CONSTRAINT "PK_LocalResponse"
	PRIMARY KEY ("IdLocalResponse");

CREATE INDEX "IXFK_LocalResponse_LocalRequest" ON log."LocalResponse" ("IdLocalRequest" ASC);

CREATE INDEX "IX_LocalResponse_CorrelationId" ON log."LocalResponse" ("CorrelationId" ASC);

ALTER TABLE log."LocalResponsePayload" ADD CONSTRAINT "PK_LocalResponsePayload"
	PRIMARY KEY ("IdLocalResponsePayload");

CREATE INDEX "IXFK_LocalResponsePayload_LocalResponse" ON log."LocalResponsePayload" ("IdLocalResponse" ASC);

ALTER TABLE log."Log" ADD CONSTRAINT "PK_Log"
	PRIMARY KEY ("IdLog");

CREATE INDEX "IX_Log_IdLogLevel" ON log."Log" ("IdLogLevel" ASC);

CREATE INDEX "IX_Log_CorrelationId" ON log."Log" ("CorrelationId" ASC);

ALTER TABLE log."LogLevel" ADD CONSTRAINT "PK_LogLevel"
	PRIMARY KEY ("IdLogLevel");

ALTER TABLE log."LogLevel" 
  ADD CONSTRAINT "UQ_LogLevel_ItemCode" UNIQUE ("ItemCode");

ALTER TABLE log."RemoteRequest" ADD CONSTRAINT "PK_RemoteRequest"
	PRIMARY KEY ("IdRemoteRequest");

CREATE INDEX "IXFK_RemoteRequest_RemoteSystem" ON log."RemoteRequest" ("IdRemoteSystem" ASC);

CREATE INDEX "IX_RemoteRequest_CorrelationId" ON log."RemoteRequest" ("CorrelationId" ASC);

ALTER TABLE log."RemoteRequestPayload" ADD CONSTRAINT "PK_RemoteRequestPayload"
	PRIMARY KEY ("IdRemoteRequestPayload");

CREATE INDEX "IXFK_RemoteRequestPayload_RemoteRequest" ON log."RemoteRequestPayload" ("IdRemoteRequest" ASC);

ALTER TABLE log."RemoteResponse" ADD CONSTRAINT "PK_RemoteResponse"
	PRIMARY KEY ("IdRemoteResponse");

CREATE INDEX "IXFK_RemoteResponse_RemoteRequest" ON log."RemoteResponse" ("IdRemoteRequest" ASC);

CREATE INDEX "IX_RemoteResponse_CorrelationId" ON log."RemoteResponse" ("CorrelationId" ASC);

ALTER TABLE log."RemoteResponsePayload" ADD CONSTRAINT "PK_RemoteResponsePayload"
	PRIMARY KEY ("IdRemoteResponsePayload");

CREATE INDEX "IXFK_RemoteResponsePayload_RemoteResponse" ON log."RemoteResponsePayload" ("IdRemoteResponse" ASC);

ALTER TABLE log."RemoteSystem" ADD CONSTRAINT "PK_RemoteSystem"
	PRIMARY KEY ("IdRemoteSystem");

ALTER TABLE log."RemoteSystem" 
  ADD CONSTRAINT "UQ_RemoteSystem_Code" UNIQUE ("Code");

ALTER TABLE log."UnstructuredLog" ADD CONSTRAINT "PK_UnstructuredLog"
	PRIMARY KEY ("IdUnstructuredLog");

CREATE INDEX "IX_UnstructuredLog_LogLevel" ON log."UnstructuredLog" ("IdLogLevel" ASC);

ALTER TABLE log."EventCounter" ADD CONSTRAINT "FK_EventCounter_IdEventCounterCategory"
	FOREIGN KEY ("IdEventCounterCategory") REFERENCES log."EventCounterCategory" ("IdEventCounterCategory") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."EventCounterData" ADD CONSTRAINT "FK_EventCounterData_IdEventCounter"
	FOREIGN KEY ("IdEventCounter") REFERENCES log."EventCounter" ("IdEventCounter") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."LocalRequest" ADD CONSTRAINT "FK_LocalRequest_IdRemoteSystem"
	FOREIGN KEY ("IdRemoteSystem") REFERENCES log."RemoteSystem" ("IdRemoteSystem") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."LocalRequestPayload" ADD CONSTRAINT "FK_LocalRequestPayload_IdLocalRequest"
	FOREIGN KEY ("IdLocalRequest") REFERENCES log."LocalRequest" ("IdLocalRequest") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."LocalResponse" ADD CONSTRAINT "FK_LocalResponse_IdLocalRequest"
	FOREIGN KEY ("IdLocalRequest") REFERENCES log."LocalRequest" ("IdLocalRequest") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."LocalResponsePayload" ADD CONSTRAINT "FK_LocalResponsePayload_IdLocalResponse"
	FOREIGN KEY ("IdLocalResponse") REFERENCES log."LocalResponse" ("IdLocalResponse") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."RemoteRequest" ADD CONSTRAINT "FK_RemoteRequest_IdRemoteSystem"
	FOREIGN KEY ("IdRemoteSystem") REFERENCES log."RemoteSystem" ("IdRemoteSystem") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."RemoteRequestPayload" ADD CONSTRAINT "FK_RemoteRequestPayload_IdRemoteRequest"
	FOREIGN KEY ("IdRemoteRequest") REFERENCES log."RemoteRequest" ("IdRemoteRequest") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."RemoteResponse" ADD CONSTRAINT "FK_RemoteResponse_IdRemoteRequest"
	FOREIGN KEY ("IdRemoteRequest") REFERENCES log."RemoteRequest" ("IdRemoteRequest") ON DELETE No Action ON UPDATE No Action;

ALTER TABLE log."RemoteResponsePayload" ADD CONSTRAINT "FK_RemoteResponsePayload_IdRemoteResponse"
	FOREIGN KEY ("IdRemoteResponse") REFERENCES log."RemoteResponse" ("IdRemoteResponse") ON DELETE No Action ON UPDATE No Action;
