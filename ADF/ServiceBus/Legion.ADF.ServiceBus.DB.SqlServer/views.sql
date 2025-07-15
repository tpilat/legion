CREATE VIEW [hosts].[VwHost] AS 
SELECT *
FROM hosts.[Host]
GO


CREATE VIEW [jobs].[VwJob] AS 
SELECT *
FROM [jobs].[Job]
GO


CREATE VIEW [orch].[VwOrchestration] AS 
SELECT *
FROM orch."Orchestration"
GO

