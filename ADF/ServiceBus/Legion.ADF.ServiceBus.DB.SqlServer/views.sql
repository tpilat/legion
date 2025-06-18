CREATE VIEW [jobs].[VwJob] AS 
SELECT
	j.[IdJob],
	j.[Name],
	j.[Description],
	j.[IdJobRunType],
	jrt.[Name] AS [JobRunType],
	j.[IdJobStatus],
	js.[Name] AS [JobStatus],
	j.[Namespace],
	j.[Properties],
	j.[DelayedStartInSeconds],
	j.[IdleTimeoutInSeconds],
	j.[CronExpression],
	j.[CronExpressionIncludeSeconds],
	j.[LastProcessingUtc],
	j.[NextProcessinUtc],
	j.[TimeoutForProcessingInSeconds],
	j.[MaxProcessingRetryCount]
FROM [jobs].[Job] as j
JOIN [jobs].[JobStatus] js ON j.[IdJobStatus] = js.[IdJobStatus]
JOIN [jobs].[JobRunType] jrt ON j.[IdJobRunType] = [jrt].[IdJobRunType]
GO


CREATE VIEW [orch].[VwOrchestration] AS 
SELECT *
FROM orch."Orchestration"
GO

