CREATE TABLE conf."ConfigurationClass"
(
	"IdConfigurationClass" uuid NOT NULL,
	"RootPath" text NOT NULL,
	"DisplayName" varchar(255) NOT NULL,
	"Class" text NULL
);

CREATE TABLE conf."ConfigurationKeyValue"
(
	"IdConfigurationKeyValue" uuid NOT NULL,
	"Key" text NOT NULL,
	"Value" text NULL,
	"AuditCreatedUtc" timestamp with time zone NOT NULL,
	"AuditModifiedUtc" timestamp with time zone NULL,
	"IdAuditCreatedBy" uuid NULL,
	"IdAuditModifiedBy" uuid NULL,
	"ConcurrencyToken" uuid NOT NULL
);

ALTER TABLE conf."ConfigurationClass" ADD CONSTRAINT "PK_ConfigurationClass"
	PRIMARY KEY ("IdConfigurationClass");

ALTER TABLE conf."ConfigurationClass" 
  ADD CONSTRAINT "UQ_ConfigurationClass_RootPath" UNIQUE ("RootPath");

ALTER TABLE conf."ConfigurationKeyValue" ADD CONSTRAINT "PK_ConfigurationKeyValue"
	PRIMARY KEY ("IdConfigurationKeyValue");

ALTER TABLE conf."ConfigurationKeyValue" 
  ADD CONSTRAINT "UQ_ConfigurationKeyValue_Key" UNIQUE ("Key");

CREATE INDEX "IX_ConfigurationKeyValue_Key" ON conf."ConfigurationKeyValue" ("Key" ASC);
