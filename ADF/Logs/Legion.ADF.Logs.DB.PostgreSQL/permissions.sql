GRANT USAGE ON SCHEMA log To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema log To #TargetDbUsername#;
GRANT usage On All Sequences In Schema log To #TargetDbUsername#;

