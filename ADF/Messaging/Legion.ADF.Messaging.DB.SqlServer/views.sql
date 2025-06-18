CREATE VIEW [inbox].[VwBlockedInboxMessageType] AS 
SELECT
	bimt."IdBlockedInboxMessageType",
	bimt."Namespace",
	bimt."CreatedUtc",
	bimt."IdInboxInstance"
FROM inbox."BlockedInboxMessageType" bimt
GO


CREATE VIEW [mbox].[VwBlockedMessageType] AS 
SELECT
	bmt."IdBlockedMessageType",
	bmt."Namespace",
	bmt."CreatedUtc",
	bmt."IdMessageBoxInstance"
FROM mbox."BlockedMessageType" bmt
GO


CREATE VIEW [outbox].[VwBlockedOutboxMessageType] AS 
SELECT
	bimt."IdBlockedOutboxMessageType",
	bimt."Namespace",
	bimt."CreatedUtc",
	bimt."IdOutboxInstance"
FROM outbox."BlockedOutboxMessageType" bimt
GO


CREATE VIEW [devt].[VwDomainEvent] AS 
SELECT
	*
FROM devt."DomainEvent"
GO


CREATE VIEW [inbox].[VwInboxMessage] AS 
SELECT
	im."IdInboxMessage",
	im."IdMessageType",
	im."IdInboxMessageStatus",
	ims."Code" AS "InboxMessageStatusCode",
	ims."Name" AS "InboxMessageStatusName",
	im."IdMessageContent",
	im."IdInboxQueue",
	im."MessageId",
	im."BusinessId",
	im."CorrelationId",
	im."SessionId",
	im."SessionMessagePartId",
	im."TraceCorrelationId",
	im."Properties",
	im."Publisher",
	im."PublisherId",
	im."CreatedUtc",
	im."ProcessedUtc",
	im."SuspendedUtc",
	im."LastProcessingUtc",
	im."LastProcessingTimeoutUtc",
	im."NextProcessingUtc",
	im."RetryCount",
	im."TargetTopic",
	im."TargetQueueName",
	im."IdInboxInstance",
	mt."Code" AS "MessageTypeCode",
	mt."Name" AS "MessageTypeName",
	mt."Namespace" AS "MessageTypeNamespace"
FROM inbox."InboxMessage" im
JOIN inbox."InboxMessageStatus" ims ON im."IdInboxMessageStatus" = ims."IdInboxMessageStatus"
JOIN inbox."InboxMessageType" mt ON im."IdMessageType" = mt."IdInboxMessageType"
GO


CREATE VIEW [inbox].[VwInboxMessageArchive] AS 
SELECT
	ima."IdInboxMessage",
	ima."IdMessageType",
	ima."IdInboxMessageStatus",
	ims."Code" AS "InboxMessageStatusCode",
	ims."Name" AS "InboxMessageStatusName",
	ima."IdMessageContent",
	ima."IdInboxQueue",
	ima."MessageId",
	ima."BusinessId",
	ima."CorrelationId",
	ima."SessionId",
	ima."SessionMessagePartId",
	ima."TraceCorrelationId",
	ima."Properties",
	ima."Publisher",
	ima."PublisherId",
	ima."CreatedUtc",
	ima."ProcessedUtc",
	ima."SuspendedUtc",
	ima."LastProcessingUtc",
	ima."LastProcessingTimeoutUtc",
	ima."NextProcessingUtc",
	ima."RetryCount",
	ima."TargetTopic",
	ima."TargetQueueName",
	ima."IdInboxInstance",
	mt."Code" AS "MessageTypeCode",
	mt."Name" AS "MessageTypeName",
	mt."Namespace" AS "MessageTypeNamespace"
FROM inbox."InboxMessageArchive" ima
JOIN inbox."InboxMessageStatus" ims ON ima."IdInboxMessageStatus" = ims."IdInboxMessageStatus"
JOIN inbox."InboxMessageType" mt ON ima."IdMessageType" = mt."IdInboxMessageType"
GO


CREATE VIEW [inbox].[VwInboxMessageContent] AS 
SELECT
	imc."IdInboxMessageContent",
	imc."MimeType",
	imc."ContentEncoding",
	imc."ByteArrayContent",
	imc."JsonContent",
	imc."StringContent",
	imc."DbOid",
	imc."Name",
	imc."RelativePath",
	imc."Metadata",
	imc."IsCompressed",
	imc."EncryptionKey"
FROM inbox."InboxMessageContent" imc
GO


CREATE VIEW [inbox].[VwInboxMessageProcessingLog] AS 
SELECT
	impl."IdInboxMessageProcessingLog",
	impl."IdInboxMessage",
	impl."IdInboxQueue",
	impl."CreatedUtc",
	impl."IdInboxMessageStatus",
	ims."Code" AS "InboxMessageStatusCode",
	ims."Name" AS "InboxMessageStatusName",
	impl."TraceCorrelationId",
	impl."IdLogMessage",
	impl."Code",
	impl."Detail",
	impl."IdInboxInstance"
FROM inbox."InboxMessageProcessingLog" impl
JOIN inbox."InboxMessageStatus" ims ON impl."IdInboxMessageStatus" = ims."IdInboxMessageStatus"
GO


CREATE VIEW [inbox].[VwInboxQueue] AS 
SELECT
	iq."IdInboxQueue",
	iq."Name",
	iq."ReceivedEventNamespace",
	iq."IdMessageType",
	iq."IsActive",
	iq."IsSequentialFIFO",
	iq."MessagesBatchCount",
	iq."MaxDegreeOfParallelism",
	iq."TimeoutForMessageProcessing",
	iq."MaxMessageProcessingRetryCount",
	iq."Properties",
	iq."IdProcessingMode",
	iq."IdInboxInstance",
	iqpmP."Code" AS "ProcessingModeCode",
	iqpmP."Name" AS "ProcessingModeName",
	iq."IdSuspendingMode",
	iqpmS."Code" AS "SuspendingModeCode",
	iqpmS."Name" AS "SuspendingModeName",
	imt."Code" AS "MessageTypeCode",
	imt."Name" AS "MessageTypeName",
	imt."Namespace" AS "MessageTypeNamespace"
FROM inbox."InboxQueue" iq
JOIN inbox."InboxQueueProcessingMode" iqpmP ON iq."IdProcessingMode" = iqpmP."IdInboxQueueProcessingMode"
JOIN inbox."InboxQueueProcessingMode" iqpmS ON iq."IdSuspendingMode" = iqpmS."IdInboxQueueProcessingMode"
LEFT JOIN inbox."InboxMessageType" imt ON iq."IdMessageType" = imt."IdInboxMessageType"
GO


CREATE VIEW [inbox].[VwInboxQueueMessages] AS 
SELECT
	iq."IdInboxQueue",
	iq."Name" AS "InboxQueueName",
	iq."IsActive",
	iq."IsSequentialFIFO",
	iq."MaxDegreeOfParallelism",
	CAST(COALESCE(created."MessageCount", 0) AS BIGINT) AS "CreatedMessageCount",
	CAST(COALESCE(processing."MessageCount", 0) AS BIGINT) AS "ProcessingMessageCount",
	CAST(COALESCE(processed."MessageCount", 0) AS BIGINT) AS "ProcessedMessageCount",
	CAST(COALESCE(failed."MessageCount", 0) AS BIGINT) AS "FailedMessageCount",
	CAST(COALESCE(suspended."MessageCount", 0) AS BIGINT) AS "SuspendedMessageCount"
FROM inbox."InboxQueue" iq
LEFT JOIN (
	SELECT
		im."IdInboxQueue",
		COUNT(*) AS "MessageCount"
	FROM inbox."InboxMessage" im
	WHERE im."IdInboxMessageStatus" = '00000001-0000-0000-0000-000000000000' --Created
	GROUP BY im."IdInboxQueue"
	) created ON iq."IdInboxQueue" = created."IdInboxQueue"
LEFT JOIN (
	SELECT
		im."IdInboxQueue",
		COUNT(*) AS "MessageCount"
	FROM inbox."InboxMessage" im
	WHERE im."IdInboxMessageStatus" = '00000002-0000-0000-0000-000000000000' --Processing
	GROUP BY im."IdInboxQueue"
	) processing ON iq."IdInboxQueue" = processing."IdInboxQueue"
LEFT JOIN (
	SELECT
		im."IdInboxQueue",
		COUNT(*) AS "MessageCount"
	FROM inbox."InboxMessage" im
	WHERE im."IdInboxMessageStatus" = '00000003-0000-0000-0000-000000000000' --Processed
	GROUP BY im."IdInboxQueue"
	) processed ON iq."IdInboxQueue" = processed."IdInboxQueue"
LEFT JOIN (
	SELECT
		im."IdInboxQueue",
		COUNT(*) AS "MessageCount"
	FROM inbox."InboxMessage" im
	WHERE im."IdInboxMessageStatus" = '00000004-0000-0000-0000-000000000000' --Failed
	GROUP BY im."IdInboxQueue"
	) failed ON iq."IdInboxQueue" = failed."IdInboxQueue"
LEFT JOIN (
	SELECT
		im."IdInboxQueue",
		COUNT(*) AS "MessageCount"
	FROM inbox."InboxMessage" im
	WHERE im."IdInboxMessageStatus" = '00000005-0000-0000-0000-000000000000' --Suspended
	GROUP BY im."IdInboxQueue"
	) suspended ON iq."IdInboxQueue" = suspended."IdInboxQueue"
GO


CREATE VIEW [mbox].[VwMessage] AS 
SELECT
	m."IdMessage",
	m."IdMessageType",
	m."IdMessageStatus",
	ms."Code" AS "MessageStatusCode",
	ms."Name" AS "MessageStatusName",
	m."IdMessageContent",
	m."IdQueue",
	m."IdTopic",
	m."CreatedUtc",
	m."MessageId",
	m."BusinessId",
	m."CorrelationId",
	m."SessionId",
	m."SessionMessagePartId",
	m."TraceCorrelationId",
	m."Properties",
	m."Publisher",
	m."PublisherId",
	m."ValidToUtc",
	m."Priority",
	mt."Code" AS "MessageTypeCode",
	mt."Name" AS "MessageTypeName",
	mt."Namespace" AS "MessageTypeNamespace"
FROM mbox."Message" m
JOIN mbox."MessageStatus" ms ON m."IdMessageStatus" = ms."IdMessageStatus"
JOIN mbox."MessageType" mt ON m."IdMessageType" = mt."IdMessageType"
GO


CREATE VIEW [mbox].[VwMessageArchive] AS 
SELECT
	ma."IdMessage",
	ma."IdMessageType",
	ma."IdMessageStatus",
	ms."Code" AS "MessageStatusCode",
	ms."Name" AS "MessageStatusName",
	ma."IdMessageContent",
	ma."IdQueue",
	ma."IdTopic",
	ma."CreatedUtc",
	ma."MessageId",
	ma."BusinessId",
	ma."CorrelationId",
	ma."SessionId",
	ma."SessionMessagePartId",
	ma."TraceCorrelationId",
	ma."Properties",
	ma."Publisher",
	ma."PublisherId",
	ma."ValidToUtc",
	ma."Priority",
	mt."Code" AS "MessageTypeCode",
	mt."Name" AS "MessageTypeName",
	mt."Namespace" AS "MessageTypeNamespace"
FROM mbox."MessageArchive" ma
JOIN mbox."MessageStatus" ms ON ma."IdMessageStatus" = ms."IdMessageStatus"
JOIN mbox."MessageType" mt ON ma."IdMessageType" = mt."IdMessageType"
GO


CREATE VIEW [mbox].[VwMessageContent] AS 
SELECT
	mc."IdMessageContent",
	mc."MimeType",
	mc."ContentEncoding",
	mc."ByteArrayContent",
	mc."JsonContent",
	mc."StringContent",
	mc."DbOid",
	mc."Name",
	mc."RelativePath",
	mc."Metadata",
	mc."IsCompressed",
	mc."EncryptionKey"
FROM mbox."MessageContent" mc
GO


CREATE VIEW [mbox].[VwMessageProcessingLog] AS 
SELECT
	mpl."IdMessageProcessingLog",
	mpl."IdMessage",
	mpl."IdQueuedMessage",
	mpl."IdSubscribedMessage",
	mpl."CreatedUtc",
	mpl."IdMessageProcessingStatus",
	mps."Code" AS "MessageProcessingStatusCode",
	mps."Name" AS "MessageProcessingStatusName",
	mpl."TraceCorrelationId",
	mpl."IdLogMessage",
	mpl."Code",
	mpl."Detail"
FROM mbox."MessageProcessingLog" mpl
JOIN mbox."MessageProcessingStatus" mps ON mpl."IdMessageProcessingStatus" = mps."IdMessageProcessingStatus"
GO


CREATE VIEW [outbox].[VwOutboxMessage] AS 
SELECT
	om."IdOutboxMessage",
	om."IdMessageType",
	om."IdOutboxMessageStatus",
	oms."Code" AS "OutboxMessageStatusCode",
	oms."Name" AS "OutboxMessageStatusName",
	om."IdMessageContent",
	om."IdOutboxQueue",
	om."MessageId",
	om."BusinessId",
	om."CorrelationId",
	om."SessionId",
	om."SessionMessagePartId",
	om."TraceCorrelationId",
	om."Properties",
	om."Publisher",
	om."PublisherId",
	om."CreatedUtc",
	om."ProcessedUtc",
	om."SuspendedUtc",
	om."LastProcessingUtc",
	om."LastProcessingTimeoutUtc",
	om."NextProcessingUtc",
	om."RetryCount",
	om."TargetTopic",
	om."TargetQueueName",
	om."IdOutboxInstance",
	mt."Code" AS "MessageTypeCode",
	mt."Name" AS "MessageTypeName",
	mt."Namespace" AS "MessageTypeNamespace"
FROM outbox."OutboxMessage" om
JOIN outbox."OutboxMessageStatus" oms ON om."IdOutboxMessageStatus" = oms."IdOutboxMessageStatus"
JOIN outbox."OutboxMessageType" mt ON om."IdMessageType" = mt."IdOutboxMessageType"
GO


CREATE VIEW [outbox].[VwOutboxMessageArchive] AS 
SELECT
	oma."IdOutboxMessage",
	oma."IdMessageType",
	oma."IdOutboxMessageStatus",
	oms."Code" AS "OutboxMessageStatusCode",
	oms."Name" AS "OutboxMessageStatusName",
	oma."IdMessageContent",
	oma."IdOutboxQueue",
	oma."MessageId",
	oma."BusinessId",
	oma."CorrelationId",
	oma."SessionId",
	oma."SessionMessagePartId",
	oma."TraceCorrelationId",
	oma."Properties",
	oma."Publisher",
	oma."PublisherId",
	oma."CreatedUtc",
	oma."ProcessedUtc",
	oma."SuspendedUtc",
	oma."LastProcessingUtc",
	oma."LastProcessingTimeoutUtc",
	oma."NextProcessingUtc",
	oma."RetryCount",
	oma."TargetTopic",
	oma."TargetQueueName",
	oma."IdOutboxInstance",
	mt."Code" AS "MessageTypeCode",
	mt."Name" AS "MessageTypeName",
	mt."Namespace" AS "MessageTypeNamespace"
FROM outbox."OutboxMessageArchive" oma
JOIN outbox."OutboxMessageStatus" oms ON oma."IdOutboxMessageStatus" = oms."IdOutboxMessageStatus"
JOIN outbox."OutboxMessageType" mt ON oma."IdMessageType" = mt."IdOutboxMessageType"
GO


CREATE VIEW [outbox].[VwOutboxMessageContent] AS 
SELECT
	omc."IdOutboxMessageContent",
	omc."MimeType",
	omc."ContentEncoding",
	omc."ByteArrayContent",
	omc."JsonContent",
	omc."StringContent",
	omc."DbOid",
	omc."Name",
	omc."RelativePath",
	omc."Metadata",
	omc."IsCompressed",
	omc."EncryptionKey"
FROM outbox."OutboxMessageContent" omc
GO


CREATE VIEW [outbox].[VwOutboxMessageProcessingLog] AS 
SELECT
	ompl."IdOutboxMessageProcessingLog",
	ompl."IdOutboxMessage",
	ompl."IdOutboxQueue",
	ompl."CreatedUtc",
	ompl."IdOutboxMessageStatus",
	oms."Code" AS "OutboxMessageStatusCode",
	oms."Name" AS "OutboxMessageStatusName",
	ompl."TraceCorrelationId",
	ompl."IdLogMessage",
	ompl."Code",
	ompl."Detail",
	ompl."IdOutboxInstance"
FROM outbox."OutboxMessageProcessingLog" ompl
JOIN outbox."OutboxMessageStatus" oms ON ompl."IdOutboxMessageStatus" = oms."IdOutboxMessageStatus"
GO


CREATE VIEW [outbox].[VwOutboxQueue] AS 
SELECT
	oq."IdOutboxQueue",
	oq."Name",
	oq."ReceivedEventNamespace",
	oq."IdMessageType",
	oq."IsActive",
	oq."IsSequentialFIFO",
	oq."MessagesBatchCount",
	oq."MaxDegreeOfParallelism",
	oq."TimeoutForMessageProcessing",
	oq."MaxMessageProcessingRetryCount",
	oq."Properties",
	oq."IdProcessingMode",
	oq."IdOutboxInstance",
	oqpmP."Code" AS "ProcessingModeCode",
	oqpmP."Name" AS "ProcessingModeName",
	oq."IdSuspendingMode",
	oqpmS."Code" AS "SuspendingModeCode",
	oqpmS."Name" AS "SuspendingModeName",
	omt."Code" AS "MessageTypeCode",
	omt."Name" AS "MessageTypeName",
	omt."Namespace" AS "MessageTypeNamespace"
FROM outbox."OutboxQueue" oq
JOIN outbox."OutboxQueueProcessingMode" oqpmP ON oq."IdProcessingMode" = oqpmP."IdOutboxQueueProcessingMode"
JOIN outbox."OutboxQueueProcessingMode" oqpmS ON oq."IdSuspendingMode" = oqpmS."IdOutboxQueueProcessingMode"
LEFT JOIN outbox."OutboxMessageType" omt ON oq."IdMessageType" = omt."IdOutboxMessageType"
GO


CREATE VIEW [outbox].[VwOutboxQueueMessages] AS 
SELECT
	oq."IdOutboxQueue",
	oq."Name" AS "OutboxQueueName",
	oq."IsActive",
	oq."IsSequentialFIFO",
	oq."MaxDegreeOfParallelism",
	CAST(COALESCE(created."MessageCount", 0) AS BIGINT) AS "CreatedMessageCount",
	CAST(COALESCE(processing."MessageCount", 0) AS BIGINT) AS "ProcessingMessageCount",
	CAST(COALESCE(processed."MessageCount", 0) AS BIGINT) AS "ProcessedMessageCount",
	CAST(COALESCE(failed."MessageCount", 0) AS BIGINT) AS "FailedMessageCount",
	CAST(COALESCE(suspended."MessageCount", 0) AS BIGINT) AS "SuspendedMessageCount"
FROM outbox."OutboxQueue" oq
LEFT JOIN (
	SELECT
		om."IdOutboxQueue",
		COUNT(*) AS "MessageCount"
	FROM outbox."OutboxMessage" om
	WHERE om."IdOutboxMessageStatus" = '00000001-0000-0000-0000-000000000000' --Created
	GROUP BY om."IdOutboxQueue"
	) created ON oq."IdOutboxQueue" = created."IdOutboxQueue"
LEFT JOIN (
	SELECT
		om."IdOutboxQueue",
		COUNT(*) AS "MessageCount"
	FROM outbox."OutboxMessage" om
	WHERE om."IdOutboxMessageStatus" = '00000002-0000-0000-0000-000000000000' --Processing
	GROUP BY om."IdOutboxQueue"
	) processing ON oq."IdOutboxQueue" = processing."IdOutboxQueue"
LEFT JOIN (
	SELECT
		om."IdOutboxQueue",
		COUNT(*) AS "MessageCount"
	FROM outbox."OutboxMessage" om
	WHERE om."IdOutboxMessageStatus" = '00000003-0000-0000-0000-000000000000' --Processed
	GROUP BY om."IdOutboxQueue"
	) processed ON oq."IdOutboxQueue" = processed."IdOutboxQueue"
LEFT JOIN (
	SELECT
		om."IdOutboxQueue",
		COUNT(*) AS "MessageCount"
	FROM outbox."OutboxMessage" om
	WHERE om."IdOutboxMessageStatus" = '00000004-0000-0000-0000-000000000000' --Failed
	GROUP BY om."IdOutboxQueue"
	) failed ON oq."IdOutboxQueue" = failed."IdOutboxQueue"
LEFT JOIN (
	SELECT
		om."IdOutboxQueue",
		COUNT(*) AS "MessageCount"
	FROM outbox."OutboxMessage" om
	WHERE om."IdOutboxMessageStatus" = '00000005-0000-0000-0000-000000000000' --Suspended
	GROUP BY om."IdOutboxQueue"
	) suspended ON oq."IdOutboxQueue" = suspended."IdOutboxQueue"
GO


CREATE VIEW [mbox].[VwQueue] AS 
SELECT
	q."IdQueue",
	q."Name",
	q."IdMessageType",
	q."IsActive",
	q."IsSequentialFIFO",
	q."MessagesBatchCount",
	q."MaxDegreeOfParallelism",
	q."TimeoutForMessageProcessing",
	q."MaxMessageProcessingRetryCount",
	q."Properties",
	q."IdProcessingMode",
	qpmP."Code" AS "ProcessingModeCode",
	qpmP."Name" AS "ProcessingModeName",
	q."IdSuspendingMode",
	qpmS."Code" AS "SuspendingModeCode",
	qpmS."Name" AS "SuspendingModeName",
	mt."Code" AS "MessageTypeCode",
	mt."Name" AS "MessageTypeName",
	mt."Namespace" AS "MessageTypeNamespace",
	q."IdJob",
	q."IdOrchestration"
FROM mbox."Queue" q
JOIN mbox."QueueProcessingMode" qpmP ON q."IdProcessingMode" = qpmP."IdQueueProcessingMode"
JOIN mbox."QueueProcessingMode" qpmS ON q."IdSuspendingMode" = qpmS."IdQueueProcessingMode"
LEFT JOIN mbox."MessageType" mt ON q."IdMessageType" = mt."IdMessageType"
GO


CREATE VIEW [mbox].[VwQueuedMessage] AS 
SELECT
	qm."IdQueuedMessage",
	qm."IdQueue",
	qm."IdMessage",
	qm."IdMessageProcessingStatus",
	mps."Code" AS "MessageProcessingStatusCode",
	mps."Name" AS "MessageProcessingStatusName",
	qm."AssignedUtc",
	qm."ProcessedUtc",
	qm."SuspendedUtc",
	qm."LastProcessingUtc",
	qm."LastProcessingTimeoutUtc",
	qm."NextProcessingUtc",
	qm."RetryCount",
	q."IdJob",
	q."IdOrchestration",
    CASE 
        WHEN ma."IdMessage" IS NOT NULL THEN 1 
        ELSE 0 
    END AS "IsArchived",
	COALESCE(m."IdMessageType", ma."IdMessageType") AS "IdMessageType",
	COALESCE(m."IdMessageStatus", ma."IdMessageStatus") AS "IdMessageStatus",
	COALESCE(ms."Code", msa."Code") AS "MessageStatusCode",
	COALESCE(ms."Name", msa."Name") AS "MessageStatusName",
	COALESCE(m."IdMessageContent", ma."IdMessageContent") AS "IdMessageContent",
	COALESCE(m."IdQueue", ma."IdQueue") AS "IdQueueMessage",
	COALESCE(m."IdTopic", ma."IdTopic") AS "IdTopicMessage",
	COALESCE(m."CreatedUtc", ma."CreatedUtc") AS "CreatedUtc",
	COALESCE(m."MessageId", ma."MessageId") AS "MessageId",
	COALESCE(m."BusinessId", ma."BusinessId") AS "BusinessId",
	COALESCE(m."CorrelationId", ma."CorrelationId") AS "CorrelationId",
	COALESCE(m."SessionId", ma."SessionId") AS "SessionId",
	COALESCE(m."SessionMessagePartId", ma."SessionMessagePartId") AS "SessionMessagePartId",
	COALESCE(m."TraceCorrelationId", ma."TraceCorrelationId") AS "TraceCorrelationId",
	COALESCE(m."Properties", ma."Properties") AS "Properties",
	COALESCE(m."Publisher", ma."Publisher") AS "Publisher",
	COALESCE(m."PublisherId", ma."PublisherId") AS "PublisherId",
	COALESCE(m."ValidToUtc", ma."ValidToUtc") AS "ValidToUtc",
	COALESCE(m."Priority", ma."Priority") AS "Priority",
	COALESCE(mt."Code", mta."Code") AS "MessageTypeCode",
	COALESCE(mt."Name", mta."Name") AS "MessageTypeName",
	COALESCE(mt."Namespace", mta."Namespace") AS "MessageTypeNamespace"
FROM mbox."QueuedMessage" qm
JOIN mbox."Queue" q ON qm."IdQueue" = q."IdQueue"
JOIN mbox."MessageProcessingStatus" mps ON qm."IdMessageProcessingStatus" = mps."IdMessageProcessingStatus"
LEFT JOIN mbox."Message" m ON qm."IdMessage" = m."IdMessage"
LEFT JOIN mbox."MessageStatus" ms ON m."IdMessageStatus" = ms."IdMessageStatus"
LEFT JOIN mbox."MessageType" mt ON m."IdMessageType" = mt."IdMessageType"
LEFT JOIN mbox."MessageArchive" ma ON qm."IdMessage" = ma."IdMessage"
LEFT JOIN mbox."MessageStatus" msa ON ma."IdMessageStatus" = msa."IdMessageStatus"
LEFT JOIN mbox."MessageType" mta ON ma."IdMessageType" = mta."IdMessageType"
GO


CREATE VIEW [mbox].[VwQueueMessages] AS 
SELECT
	q."IdQueue",
	q."Name" AS "QueueName",
	q."IsActive",
	q."IsSequentialFIFO",
	q."MaxDegreeOfParallelism",
	q."IdJob",
	q."IdOrchestration",
	CAST(COALESCE(assigned."MessageCount", 0) AS BIGINT) AS "AssignedMessageCount",
	CAST(COALESCE(processing."MessageCount", 0) AS BIGINT) AS "ProcessingMessageCount",
	CAST(COALESCE(processed."MessageCount", 0) AS BIGINT) AS "ProcessedMessageCount",
	CAST(COALESCE(failed."MessageCount", 0) AS BIGINT) AS "FailedMessageCount",
	CAST(COALESCE(suspended."MessageCount", 0) AS BIGINT) AS "SuspendedMessageCount"
FROM mbox."Queue" q
LEFT JOIN (
	SELECT
		qm."IdQueue",
		COUNT(*) AS "MessageCount"
	FROM mbox."QueuedMessage"qm
	WHERE qm."IdMessageProcessingStatus" = '00000001-0000-0000-0000-000000000000' --Assigned
	GROUP BY qm."IdQueue"
	) assigned ON q."IdQueue" = assigned."IdQueue"
LEFT JOIN (
	SELECT
		qm."IdQueue",
		COUNT(*) AS "MessageCount"
	FROM mbox."QueuedMessage"qm
	WHERE qm."IdMessageProcessingStatus" = '00000002-0000-0000-0000-000000000000' --Processing
	GROUP BY qm."IdQueue"
	) processing ON q."IdQueue" = processing."IdQueue"
LEFT JOIN (
	SELECT
		qm."IdQueue",
		COUNT(*) AS "MessageCount"
	FROM mbox."QueuedMessage"qm
	WHERE qm."IdMessageProcessingStatus" = '00000003-0000-0000-0000-000000000000' --Processed
	GROUP BY qm."IdQueue"
	) processed ON q."IdQueue" = processed."IdQueue"
LEFT JOIN (
	SELECT
		qm."IdQueue",
		COUNT(*) AS "MessageCount"
	FROM mbox."QueuedMessage"qm
	WHERE qm."IdMessageProcessingStatus" = '00000004-0000-0000-0000-000000000000' --Failed
	GROUP BY qm."IdQueue"
	) failed ON q."IdQueue" = failed."IdQueue"
LEFT JOIN (
	SELECT
		qm."IdQueue",
		COUNT(*) AS "MessageCount"
	FROM mbox."QueuedMessage"qm
	WHERE qm."IdMessageProcessingStatus" = '00000005-0000-0000-0000-000000000000' --Suspended
	GROUP BY qm."IdQueue"
	) suspended ON q."IdQueue" = suspended."IdQueue"
GO


CREATE VIEW [mbox].[VwSubscribedMessage] AS 
SELECT
	sm."IdSubscribedMessage",
	sm."IdTopicSubscription",
	ts."IdTopic",
	sm."IdMessage",
	sm."IdMessageProcessingStatus",
	mps."Code" AS "MessageProcessingStatusCode",
	mps."Name" AS "MessageProcessingStatusName",
	sm."AssignedUtc",
	sm."ProcessedUtc",
	sm."SuspendedUtc",
	sm."LastProcessingUtc",
	sm."LastProcessingTimeoutUtc",
	sm."NextProcessingUtc",
	sm."RetryCount",
	ts."IdJob",
	ts."IdOrchestration",
    CASE 
        WHEN ma."IdMessage" IS NOT NULL THEN 1 
        ELSE 0 
    END AS "IsArchived",
	COALESCE(m."IdMessageType", ma."IdMessageType") AS "IdMessageType",
	COALESCE(m."IdMessageStatus", ma."IdMessageStatus") AS "IdMessageStatus",
	COALESCE(ms."Code", msa."Code") AS "MessageStatusCode",
	COALESCE(ms."Name", msa."Name") AS "MessageStatusName",
	COALESCE(m."IdMessageContent", ma."IdMessageContent") AS "IdMessageContent",
	COALESCE(m."IdQueue", ma."IdQueue") AS "IdQueueMessage",
	COALESCE(m."IdTopic", ma."IdTopic") AS "IdTopicMessage",
	COALESCE(m."CreatedUtc", ma."CreatedUtc") AS "CreatedUtc",
	COALESCE(m."MessageId", ma."MessageId") AS "MessageId",
	COALESCE(m."BusinessId", ma."BusinessId") AS "BusinessId",
	COALESCE(m."CorrelationId", ma."CorrelationId") AS "CorrelationId",
	COALESCE(m."SessionId", ma."SessionId") AS "SessionId",
	COALESCE(m."SessionMessagePartId", ma."SessionMessagePartId") AS "SessionMessagePartId",
	COALESCE(m."TraceCorrelationId", ma."TraceCorrelationId") AS "TraceCorrelationId",
	COALESCE(m."Properties", ma."Properties") AS "Properties",
	COALESCE(m."Publisher", ma."Publisher") AS "Publisher",
	COALESCE(m."PublisherId", ma."PublisherId") AS "PublisherId",
	COALESCE(m."ValidToUtc", ma."ValidToUtc") AS "ValidToUtc",
	COALESCE(m."Priority", ma."Priority") AS "Priority",
	COALESCE(mt."Code", mta."Code") AS "MessageTypeCode",
	COALESCE(mt."Name", mta."Name") AS "MessageTypeName",
	COALESCE(mt."Namespace", mta."Namespace") AS "MessageTypeNamespace"
FROM mbox."SubscribedMessage" sm
JOIN mbox."TopicSubscription" ts ON sm."IdTopicSubscription" = ts."IdTopicSubscription"
JOIN mbox."MessageProcessingStatus" mps ON sm."IdMessageProcessingStatus" = mps."IdMessageProcessingStatus"
LEFT JOIN mbox."Message" m ON sm."IdMessage" = m."IdMessage"
LEFT JOIN mbox."MessageStatus" ms ON m."IdMessageStatus" = ms."IdMessageStatus"
LEFT JOIN mbox."MessageType" mt ON m."IdMessageType" = mt."IdMessageType"
LEFT JOIN mbox."MessageArchive" ma ON sm."IdMessage" = ma."IdMessage"
LEFT JOIN mbox."MessageStatus" msa ON ma."IdMessageStatus" = msa."IdMessageStatus"
LEFT JOIN mbox."MessageType" mta ON ma."IdMessageType" = mta."IdMessageType"
GO


CREATE VIEW [mbox].[VwTopic] AS 
SELECT
	t."IdTopic",
	t."Name",
	t."IsActive",
	t."IsSequentialFIFO",
	t."MessagesBatchCount",
	t."MaxDegreeOfParallelism",
	t."TimeoutForMessageProcessing",
	t."MaxMessageProcessingRetryCount",
	t."Properties",
	t."IdProcessingMode",
	qpmP."Code" AS "ProcessingModeCode",
	qpmP."Name" AS "ProcessingModeName",
	t."IdSuspendingMode",
	qpmS."Code" AS "SuspendingModeCode",
	qpmS."Name" AS "SuspendingModeName"
FROM mbox."Topic" t
JOIN mbox."QueueProcessingMode" qpmP ON t."IdProcessingMode" = qpmP."IdQueueProcessingMode"
JOIN mbox."QueueProcessingMode" qpmS ON t."IdSuspendingMode" = qpmS."IdQueueProcessingMode"
GO


CREATE VIEW [mbox].[VwTopicSubscription] AS 
SELECT
	ts."IdTopicSubscription",
	ts."IdTopic",
	ts."SubscriptionName",
	ts."IsActive",
	ts."IsSequentialFIFO",
	ts."MessagesBatchCount",
	ts."MaxDegreeOfParallelism",
	ts."TimeoutForMessageProcessing",
	ts."MaxMessageProcessingRetryCount",
	ts."Properties",
	ts."IdProcessingMode",
	qpmP."Code" AS "ProcessingModeCode",
	qpmP."Name" AS "ProcessingModeName",
	ts."IdSuspendingMode",
	qpmS."Code" AS "SuspendingModeCode",
	qpmS."Name" AS "SuspendingModeName",
	ts."IdJob",
	ts."IdOrchestration"
FROM mbox."TopicSubscription" ts
JOIN mbox."QueueProcessingMode" qpmP ON ts."IdProcessingMode" = qpmP."IdQueueProcessingMode"
JOIN mbox."QueueProcessingMode" qpmS ON ts."IdSuspendingMode" = qpmS."IdQueueProcessingMode"
GO


CREATE VIEW [mbox].[VwTopicSubscriptionMessages] AS 
SELECT
	ts."IdTopicSubscription",
	ts."SubscriptionName",
	ts."IsActive" AS "SubscriptionIsActive",
	ts."IsSequentialFIFO" AS "SubscriptionIsSequentialFIFO",
	ts."MessagesBatchCount" AS "SubscriptionMessagesBatchCount",
	ts."MaxDegreeOfParallelism" AS "SubscriptionMaxDegreeOfParallelism",
	ts."TimeoutForMessageProcessing" AS "SubscriptionTimeoutForMessageProcessing",
	ts."MaxMessageProcessingRetryCount" AS "SubscriptionMaxMessageProcessingRetryCount",
	ts."IdJob",
	ts."IdOrchestration",
	t."IdTopic",
	t."Name" AS "TopicName",
	t."IsActive" AS "TopisIsActive",
	t."IsSequentialFIFO" AS "TopicIsSequentialFIFO",
	t."MessagesBatchCount" AS "TopicMessagesBatchCount",
	t."MaxDegreeOfParallelism" AS "TopicMaxDegreeOfParallelism",
	t."TimeoutForMessageProcessing" AS "TopicTimeoutForMessageProcessing",
	t."MaxMessageProcessingRetryCount" AS "TopicMaxMessageProcessingRetryCount",
	CAST(COALESCE(assigned."MessageCount", 0) AS BIGINT) AS "AssignedMessageCount",
	CAST(COALESCE(processing."MessageCount", 0) AS BIGINT) AS "ProcessingMessageCount",
	CAST(COALESCE(processed."MessageCount", 0) AS BIGINT) AS "ProcessedMessageCount",
	CAST(COALESCE(failed."MessageCount", 0) AS BIGINT) AS "FailedMessageCount",
	CAST(COALESCE(suspended."MessageCount", 0) AS BIGINT) AS "SuspendedMessageCount"
FROM mbox."TopicSubscription" ts
JOIN mbox."Topic" t ON ts."IdTopic" = t."IdTopic"
LEFT JOIN (
	SELECT
		sm."IdTopicSubscription",
		COUNT(*) AS "MessageCount"
	FROM mbox."SubscribedMessage"sm
	WHERE sm."IdMessageProcessingStatus" = '00000001-0000-0000-0000-000000000000' --Assigned
	GROUP BY sm."IdTopicSubscription"
	) assigned ON ts."IdTopicSubscription" = assigned."IdTopicSubscription"
LEFT JOIN (
	SELECT
		sm."IdTopicSubscription",
		COUNT(*) AS "MessageCount"
	FROM mbox."SubscribedMessage"sm
	WHERE sm."IdMessageProcessingStatus" = '00000002-0000-0000-0000-000000000000' --Processing
	GROUP BY sm."IdTopicSubscription"
	) processing ON ts."IdTopicSubscription" = processing."IdTopicSubscription"
LEFT JOIN (
	SELECT
		sm."IdTopicSubscription",
		COUNT(*) AS "MessageCount"
	FROM mbox."SubscribedMessage"sm
	WHERE sm."IdMessageProcessingStatus" = '00000003-0000-0000-0000-000000000000' --Processed
	GROUP BY sm."IdTopicSubscription"
	) processed ON ts."IdTopicSubscription" = processed."IdTopicSubscription"
LEFT JOIN (
	SELECT
		sm."IdTopicSubscription",
		COUNT(*) AS "MessageCount"
	FROM mbox."SubscribedMessage"sm
	WHERE sm."IdMessageProcessingStatus" = '00000004-0000-0000-0000-000000000000' --Failed
	GROUP BY sm."IdTopicSubscription"
	) failed ON ts."IdTopicSubscription" = failed."IdTopicSubscription"
LEFT JOIN (
	SELECT
		sm."IdTopicSubscription",
		COUNT(*) AS "MessageCount"
	FROM mbox."SubscribedMessage"sm
	WHERE sm."IdMessageProcessingStatus" = '00000005-0000-0000-0000-000000000000' --Suspended
	GROUP BY sm."IdTopicSubscription"
	) suspended ON ts."IdTopicSubscription" = suspended."IdTopicSubscription"
GO

