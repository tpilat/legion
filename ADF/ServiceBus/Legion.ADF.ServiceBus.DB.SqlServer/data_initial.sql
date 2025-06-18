INSERT INTO [jobs].[JobStatus]
	([IdJobStatus], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Disabled', 'Disabled'),
	('00000002-0000-0000-0000-000000000000', 'Started', 'Started'),
	('00000003-0000-0000-0000-000000000000', 'Idle', 'Idle'),
	('00000004-0000-0000-0000-000000000000', 'Running', 'Running'),
	('00000005-0000-0000-0000-000000000000', 'Error', 'Error'),
	('00000006-0000-0000-0000-000000000000', 'Suspended', 'Suspended'),
	('00000007-0000-0000-0000-000000000000', 'Stopped', 'Stopped');



INSERT INTO [jobs].[JobRunType]
	([IdJobRunType], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'SequentialTimer', 'SequentialTimer'),
	('00000002-0000-0000-0000-000000000000', 'PeriodicTimer', 'PeriodicTimer'),
	('00000003-0000-0000-0000-000000000000', 'Cron', 'Cron');



INSERT INTO [jobs].[JobMessageType]
	([IdJobMessageType], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Published', 'Published'),
	('00000002-0000-0000-0000-000000000000', 'SubscribedFromQueue', 'SubscribedFromQueue'),
	('00000003-0000-0000-0000-000000000000', 'SubscribedFromTopic', 'SubscribedFromTopic');