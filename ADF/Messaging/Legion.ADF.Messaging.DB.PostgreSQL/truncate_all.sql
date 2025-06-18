TRUNCATE TABLE devt."BlockedDomainEventType" CASCADE;
TRUNCATE TABLE devt."DomainEventProcessingLog" CASCADE;
TRUNCATE TABLE devt."DomainEvent" CASCADE;
TRUNCATE TABLE devt."DomainEventContent" CASCADE;

TRUNCATE TABLE inbox."BlockedInboxMessageType" CASCADE;
TRUNCATE TABLE inbox."InboxMessage" CASCADE;
TRUNCATE TABLE inbox."InboxMessageArchive" CASCADE;
TRUNCATE TABLE inbox."InboxMessageContent" CASCADE;
TRUNCATE TABLE inbox."InboxMessageProcessingLog" CASCADE;
TRUNCATE TABLE inbox."InboxMessageType" CASCADE;
TRUNCATE TABLE inbox."InboxQueue" CASCADE;
TRUNCATE TABLE inbox."InboxInstance" CASCADE;

TRUNCATE TABLE outbox."BlockedOutboxMessageType" CASCADE;
TRUNCATE TABLE outbox."OutboxMessage" CASCADE;
TRUNCATE TABLE outbox."OutboxMessageArchive" CASCADE;
TRUNCATE TABLE outbox."OutboxMessageContent" CASCADE;
TRUNCATE TABLE outbox."OutboxMessageProcessingLog" CASCADE;
TRUNCATE TABLE outbox."OutboxMessageType" CASCADE;
TRUNCATE TABLE outbox."OutboxQueue" CASCADE;
TRUNCATE TABLE outbox."OutboxInstance" CASCADE;

TRUNCATE TABLE mbox."BlockedMessageType" CASCADE;
TRUNCATE TABLE mbox."Message" CASCADE;
TRUNCATE TABLE mbox."MessageArchive" CASCADE;
TRUNCATE TABLE mbox."MessageBoxInstance" CASCADE;
TRUNCATE TABLE mbox."MessageBoxProcessingLog" CASCADE;
TRUNCATE TABLE mbox."MessageContent" CASCADE;
TRUNCATE TABLE mbox."MessageProcessingLog" CASCADE;
TRUNCATE TABLE mbox."MessageType" CASCADE;
TRUNCATE TABLE mbox."Queue" CASCADE;
TRUNCATE TABLE mbox."QueuedMessage" CASCADE;
TRUNCATE TABLE mbox."SubscribedMessage" CASCADE;
TRUNCATE TABLE mbox."Topic" CASCADE;
TRUNCATE TABLE mbox."TopicSubscription" CASCADE;