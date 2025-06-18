GRANT USAGE ON SCHEMA auth To #TargetDbUsername#;
GRANT select, insert, update, delete On All Tables In Schema auth To #TargetDbUsername#;
GRANT usage On All Sequences In Schema auth To #TargetDbUsername#;

