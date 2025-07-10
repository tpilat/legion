GRANT USAGE ON SCHEMA hosts To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema hosts To #TargetDbUsername#;
GRANT usage On All Sequences In Schema hosts To #TargetDbUsername#;

GRANT USAGE ON SCHEMA jobs To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema jobs To #TargetDbUsername#;
GRANT usage On All Sequences In Schema jobs To #TargetDbUsername#;

GRANT USAGE ON SCHEMA orch To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema orch To #TargetDbUsername#;
GRANT usage On All Sequences In Schema orch To #TargetDbUsername#;

