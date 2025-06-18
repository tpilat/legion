CREATE VIEW comp."VwJob"
AS
SELECT * from comp."Job";

CREATE VIEW orch."VwOrchestration"
AS
SELECT * FROM orch."Orchestration";

CREATE VIEW mbox."VwQueuedMessage"
AS
SELECT
	"IdQueuedMessage",
	"IdQueue",
	"IdMessage",
	"QueuedUtc",
	"IdMessageProcessingStatus",
	"LastProcessedUtc",
	"NextProcessingUtc",
	"RetryCount",
	"ProcessedUtc",
	"TerminatedUtc"
FROM mbox."QueuedMessage";
