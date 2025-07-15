CREATE TABLE cache."CacheData"
(
	"KeyHash" text NOT NULL,
	"ValueHash" text NOT NULL,
	"Key" text NOT NULL,
	"Value" text NOT NULL,
	"KeyPrefix450" text NOT NULL,
	"ExpiresUtc" timestamp with time zone NULL,
	"SlidingTime" interval NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"LastAccessedUtc" timestamp with time zone NOT NULL,
	"RowVersion" uuid NOT NULL
);

CREATE TABLE cache."DistributedLock"
(
	"KeyHash" text NOT NULL,
	"LockKey" text NOT NULL,
	"LockId" text NOT NULL,
	"Metadata" text NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ExpiresUtc" timestamp with time zone NOT NULL
);

CREATE TABLE cache."ReloadableCacheKey"
(
	"IdReloadableCacheKey" uuid NOT NULL,
	"Key" text NULL,
	"Tags" text[] NULL,
	"CreatedUtc" timestamp with time zone NOT NULL,
	"ReloadAtUtc" timestamp with time zone NOT NULL
);

ALTER TABLE cache."CacheData" ADD CONSTRAINT "PK_CacheData"
	PRIMARY KEY ("KeyHash");

CREATE INDEX "IX_CacheData_KeyPrefix" ON cache."CacheData" ("KeyPrefix450" ASC);

CREATE INDEX "IX_CacheData_ExpiresUtc" ON cache."CacheData" ("ExpiresUtc" ASC);

CREATE INDEX "IX_CacheData_ValueHash" ON cache."CacheData" ("ValueHash" ASC);

ALTER TABLE cache."DistributedLock" ADD CONSTRAINT "PK_DistributedLock"
	PRIMARY KEY ("KeyHash");

CREATE INDEX "IX_DistributedLock_LockId" ON cache."DistributedLock" ("LockId" ASC);

ALTER TABLE cache."ReloadableCacheKey" ADD CONSTRAINT "PK_ReloadableCacheKey"
	PRIMARY KEY ("IdReloadableCacheKey");
