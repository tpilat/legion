SET client_min_messages TO WARNING; 

SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '#TargetDatabase#' AND pid <> pg_backend_pid();

COMMIT;

DROP DATABASE IF EXISTS #TargetDatabase#;

CREATE DATABASE #TargetDatabase# WITH OWNER = #AdminUser# TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'sk_SK.UTF-8' LC_CTYPE = 'sk_SK.UTF-8' TABLESPACE = pg_default CONNECTION LIMIT = -1;


DO
$do$
BEGIN
   IF NOT EXISTS (
      SELECT FROM pg_catalog.pg_roles
      WHERE  rolname = '#TargetDbUsername#') THEN

      CREATE USER #TargetDbUsername# WITH PASSWORD '#TargetDbPassword#' NoCreateDB;
   END IF;
END
$do$;

DROP OWNED BY #TargetDbUsername#;
DROP USER IF EXISTS #TargetDbUsername#;
CREATE USER #TargetDbUsername# WITH PASSWORD '#TargetDbPassword#' NoCreateDB; 

