INSERT INTO [devt].[DomainEventProcessingStatus]
	([IdDomainEventProcessingStatus], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Created', 'Created'),
	('00000002-0000-0000-0000-000000000000', 'Processing', 'Processing'),
	('00000003-0000-0000-0000-000000000000', 'Processed', 'Processed'),
	('00000004-0000-0000-0000-000000000000', 'Failed', 'Failed'),
	('00000005-0000-0000-0000-000000000000', 'Suspended', 'Suspended'),
	('00000006-0000-0000-0000-000000000000', 'NoHandler', 'NoHandler'),
	('00000007-0000-0000-0000-000000000000', 'Blocked', 'Blocked');



INSERT INTO [outbox].[OutboxMessageStatus]
	([IdOutboxMessageStatus], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Created', 'Created'),
	('00000002-0000-0000-0000-000000000000', 'Processing', 'Processing'),
	('00000003-0000-0000-0000-000000000000', 'Processed', 'Processed'),
	('00000004-0000-0000-0000-000000000000', 'Failed', 'Failed'),
	('00000005-0000-0000-0000-000000000000', 'Suspended', 'Suspended'),
	('00000006-0000-0000-0000-000000000000', 'NoHandler', 'NoHandler'),
	('00000007-0000-0000-0000-000000000000', 'Blocked', 'Blocked'),
	('00000008-0000-0000-0000-000000000000', 'UnknownType', 'UnknownType');



INSERT INTO [outbox].[OutboxQueueProcessingMode]
	([IdOutboxQueueProcessingMode], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'NoAction', 'NoAction'),
	('00000002-0000-0000-0000-000000000000', 'Archivate', 'Archivate'),
	('00000003-0000-0000-0000-000000000000', 'Delete', 'Delete');



INSERT INTO [inbox].[InboxMessageStatus]
	([IdInboxMessageStatus], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Created', 'Created'),
	('00000002-0000-0000-0000-000000000000', 'Processing', 'Processing'),
	('00000003-0000-0000-0000-000000000000', 'Processed', 'Processed'),
	('00000004-0000-0000-0000-000000000000', 'Failed', 'Failed'),
	('00000005-0000-0000-0000-000000000000', 'Suspended', 'Suspended'),
	('00000006-0000-0000-0000-000000000000', 'NoHandler', 'NoHandler'),
	('00000007-0000-0000-0000-000000000000', 'Blocked', 'Blocked'),
	('00000008-0000-0000-0000-000000000000', 'UnknownType', 'UnknownType');



INSERT INTO [inbox].[InboxQueueProcessingMode]
	([IdInboxQueueProcessingMode], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'NoAction', 'NoAction'),
	('00000002-0000-0000-0000-000000000000', 'Archivate', 'Archivate'),
	('00000003-0000-0000-0000-000000000000', 'Delete', 'Delete');



INSERT INTO [mbox].[MessageStatus]
	([IdMessageStatus], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Created', 'Created'),
	('00000002-0000-0000-0000-000000000000', 'Processed', 'Processed'),
	('00000003-0000-0000-0000-000000000000', 'Dropped', 'Dropped  (due to valid to)');



INSERT INTO [mbox].[MessageProcessingStatus]
	([IdMessageProcessingStatus], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'Created', 'Created'),
	('00000002-0000-0000-0000-000000000000', 'Processing', 'Processing'),
	('00000003-0000-0000-0000-000000000000', 'Processed', 'Processed'),
	('00000004-0000-0000-0000-000000000000', 'Failed', 'Failed'),
	('00000005-0000-0000-0000-000000000000', 'Suspended', 'Suspended'),
	('00000006-0000-0000-0000-000000000000', 'NoHandler', 'NoHandler'),
	('00000007-0000-0000-0000-000000000000', 'Blocked', 'Blocked'),
	('00000008-0000-0000-0000-000000000000', 'UnknownType', 'UnknownType');



INSERT INTO [mbox].[QueueProcessingMode]
	([IdQueueProcessingMode], [Code], [Name])
VALUES
	('00000001-0000-0000-0000-000000000000', 'NoAction', 'NoAction'),
	('00000002-0000-0000-0000-000000000000', 'Archivate', 'Archivate'),
	('00000003-0000-0000-0000-000000000000', 'Delete', 'Delete');
