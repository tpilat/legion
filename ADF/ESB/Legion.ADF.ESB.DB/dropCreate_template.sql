SET client_min_messages TO WARNING; 

SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '#DBNAME#' AND pid <> pg_backend_pid();

COMMIT;

DROP DATABASE IF EXISTS #DBNAME#;

CREATE DATABASE #DBNAME# WITH OWNER = #ADMIN# TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'sk_SK.UTF-8' LC_CTYPE = 'sk_SK.UTF-8' TABLESPACE = pg_default CONNECTION LIMIT = -1;
