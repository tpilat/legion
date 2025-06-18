SET client_min_messages TO WARNING; 

SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = 'legion_adf_logs' AND pid <> pg_backend_pid();

COMMIT;

DROP DATABASE IF EXISTS legion_adf_logs;

CREATE DATABASE legion_adf_logs WITH OWNER = postgres TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'sk_SK.UTF-8' LC_CTYPE = 'sk_SK.UTF-8' TABLESPACE = pg_default CONNECTION LIMIT = -1;


DO
$do$
BEGIN
   IF NOT EXISTS (
      SELECT FROM pg_catalog.pg_roles
      WHERE  rolname = 'logsusr') THEN

      CREATE USER logsusr WITH PASSWORD 'logspwd' NoCreateDB;
   END IF;
END
$do$;

DROP OWNED BY logsusr;
DROP USER IF EXISTS logsusr;
CREATE USER logsusr WITH PASSWORD 'logspwd' NoCreateDB; 

