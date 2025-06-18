CREATE TABLE cache."ReloadableCacheKey"
(
	"IdReloadableCacheKey" uuid NOT NULL,
	"Key" text NULL,
	"Tags" text[] NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ReloadAtUtc" timestamp with time zone NOT NULL
);

ALTER TABLE cache."ReloadableCacheKey" ADD CONSTRAINT "PK_ReloadableCacheKey"
	PRIMARY KEY ("IdReloadableCacheKey");
