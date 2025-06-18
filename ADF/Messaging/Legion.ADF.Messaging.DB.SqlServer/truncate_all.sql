-- Step 1: Recreate all foreign key constraints
DECLARE @sql2 NVARCHAR(MAX) = '';

SET @sql2 = '';

-- Generate SQL to recreate foreign key constraints for each parent-child pair
SELECT @sql2 = @sql2 + 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fkc.parent_object_id)) 
                    + '.' + QUOTENAME(OBJECT_NAME(fkc.parent_object_id)) 
                    + ' ADD CONSTRAINT ' + QUOTENAME(fk.name) + 
                    ' FOREIGN KEY(' + COL_NAME(fkc.parent_object_id, fkc.parent_column_id) + ')' +
                    ' REFERENCES ' + QUOTENAME(OBJECT_SCHEMA_NAME(fkc.referenced_object_id)) + 
                    '.' + QUOTENAME(OBJECT_NAME(fkc.referenced_object_id)) + 
                    '(' + COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) + '); '
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id;

IF OBJECT_ID('tempdb..##TempSql') IS NOT NULL DROP TABLE ##TempSql;
CREATE TABLE ##TempSql (SqlStatement NVARCHAR(MAX));

-- Store @sql2 in the global temporary table
INSERT INTO ##TempSql (SqlStatement) VALUES (@sql2);
GO


-- Step 2: Drop all foreign key constraints
DECLARE @sql NVARCHAR(MAX) = '';

-- Generate SQL to drop all foreign key constraints
SELECT @sql = @sql + 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) 
                    + '.' + QUOTENAME(OBJECT_NAME(fk.parent_object_id)) 
                    + ' DROP CONSTRAINT ' + QUOTENAME(fk.name) + '; '
FROM sys.foreign_keys fk;

-- Execute the SQL to drop the foreign key constraints
EXEC sp_executesql @sql;
GO


-- Step 3: Truncate all tables in the database
TRUNCATE TABLE [devt].[BlockedDomainEventType];
GO

TRUNCATE TABLE [devt].[DomainEventProcessingLog];
GO

TRUNCATE TABLE [devt].[DomainEvent];
GO

TRUNCATE TABLE [devt].[DomainEventContent];
GO


TRUNCATE TABLE [inbox].[BlockedInboxMessageType];
GO

TRUNCATE TABLE [inbox].[InboxMessage];
GO

TRUNCATE TABLE [inbox].[InboxMessageArchive];
GO

TRUNCATE TABLE [inbox].[InboxMessageContent];
GO

TRUNCATE TABLE [inbox].[InboxMessageProcessingLog];
GO

TRUNCATE TABLE [inbox].[InboxMessageType];
GO

TRUNCATE TABLE [inbox].[InboxQueue];
GO

TRUNCATE TABLE [inbox].[InboxInstance];
GO



TRUNCATE TABLE [outbox].[BlockedOutboxMessageType];
GO

TRUNCATE TABLE [outbox].[OutboxMessage];
GO

TRUNCATE TABLE [outbox].[OutboxMessageArchive];
GO

TRUNCATE TABLE [outbox].[OutboxMessageContent];
GO

TRUNCATE TABLE [outbox].[OutboxMessageProcessingLog];
GO

TRUNCATE TABLE [outbox].[OutboxMessageType];
GO

TRUNCATE TABLE [outbox].[OutboxQueue];
GO

TRUNCATE TABLE [outbox].[OutboxInstance];
GO



TRUNCATE TABLE [mbox].[BlockedMessageType];
GO

TRUNCATE TABLE [mbox].[Message];
GO

TRUNCATE TABLE [mbox].[MessageArchive];
GO

TRUNCATE TABLE [mbox].[MessageBoxInstance];
GO

TRUNCATE TABLE [mbox].[MessageBoxProcessingLog];
GO

TRUNCATE TABLE [mbox].[MessageType];
GO

TRUNCATE TABLE [mbox].[MessageContent];
GO

TRUNCATE TABLE [mbox].[MessageProcessingLog];
GO

TRUNCATE TABLE [mbox].[Queue];
GO

TRUNCATE TABLE [mbox].[QueuedMessage];
GO

TRUNCATE TABLE [mbox].[SubscribedMessage];
GO

TRUNCATE TABLE [mbox].[Topic];
GO

TRUNCATE TABLE [mbox].[TopicSubscription];
GO



-- Step4 Execute the SQL to recreate the foreign key constraints
DECLARE @finalSql NVARCHAR(MAX);
SELECT @finalSql = SqlStatement FROM ##TempSql;

EXEC sp_executesql @finalSql;

-- Step 5 Drop the temporary table
IF OBJECT_ID('tempdb..##TempSql') IS NOT NULL DROP TABLE ##TempSql;
GO