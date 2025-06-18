GRANT USAGE ON SCHEMA devt To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema devt To #TargetDbUsername#;
GRANT usage On All Sequences In Schema devt To #TargetDbUsername#;

GRANT USAGE ON SCHEMA inbox To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema inbox To #TargetDbUsername#;
GRANT usage On All Sequences In Schema inbox To #TargetDbUsername#;

GRANT USAGE ON SCHEMA mbox To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema mbox To #TargetDbUsername#;
GRANT usage On All Sequences In Schema mbox To #TargetDbUsername#;

GRANT USAGE ON SCHEMA outbox To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema outbox To #TargetDbUsername#;
GRANT usage On All Sequences In Schema outbox To #TargetDbUsername#;

