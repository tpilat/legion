SET client_min_messages TO WARNING; 

SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = 'legion_adf_msg' AND pid <> pg_backend_pid();

COMMIT;

DROP DATABASE IF EXISTS legion_adf_msg;

CREATE DATABASE legion_adf_msg WITH OWNER = postgres TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'sk_SK.UTF-8' LC_CTYPE = 'sk_SK.UTF-8' TABLESPACE = pg_default CONNECTION LIMIT = -1;


DO
$do$
BEGIN
   IF NOT EXISTS (
      SELECT FROM pg_catalog.pg_roles
      WHERE  rolname = 'msgusr') THEN

      CREATE USER msgusr WITH PASSWORD 'msgpwd' NoCreateDB;
   END IF;
END
$do$;

DROP OWNED BY msgusr;
DROP USER IF EXISTS msgusr;
CREATE USER msgusr WITH PASSWORD 'msgpwd' NoCreateDB; 

