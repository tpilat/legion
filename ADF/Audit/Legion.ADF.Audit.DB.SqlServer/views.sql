CREATE VIEW [aud].[VwApplicationEntry] AS 
SELECT
	ae.[IdApplicationEntry],
	ae.[IdApplicationEntryToken],
	aet.[Token],
	aet.[SourceFilePath],
	aet.[MethodInfo],
	aet.[AggregateName],
	ae.[AggregateIdentifier],
	aet.[Description],
	ae.[IdAuditOperation],
	ae.[RuntimeUniqueKey],
	ae.[CreatedUtc],
	ae.[CorrelationId],
	ae.[ExternalCorrelationId],
	ae.[HttpMethod],
	ae.[Uri],
	ae.[IdUser],
	ae.[TenantIdentifier],
	ae.[RemoteIP],
	aereq.[IdApplicationEntryRequest],
	aeres.[IdApplicationEntryResponse],
	aeres.[StatusCode],
	aeres.[Error],
	aeres.[ElapsedMilliseconds]
FROM aud.[ApplicationEntry] ae
JOIN aud.[ApplicationEntryToken] aet ON ae.[IdApplicationEntryToken] = aet.[IdApplicationEntryToken]
LEFT JOIN aud.[ApplicationEntryRequest] aereq ON ae.[IdApplicationEntry] = aereq.[IdApplicationEntry]
LEFT JOIN aud.[ApplicationEntryResponse] aeres ON ae.[IdApplicationEntry] = aeres.[IdApplicationEntry]
GO


CREATE VIEW [aud].[VwAuditEntry] AS 
SELECT
	*
FROM aud.[AuditEntry]
GO

