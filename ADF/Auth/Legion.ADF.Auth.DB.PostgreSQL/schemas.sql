SET client_min_messages TO WARNING; 

DROP Schema If Exists auth Cascade;
CREATE Schema auth;


CREATE OR REPLACE FUNCTION f_truncate_tables()
  RETURNS void AS
$func$
BEGIN
   EXECUTE
  (SELECT 'TRUNCATE TABLE '
       || string_agg(format('%I.%I', schemaname, tablename), ', ')
       || ' RESTART IDENTITY CASCADE'
   FROM   pg_tables
   WHERE  tableowner = 'postgres' AND schemaname in ('auth'));

  EXECUTE
  (SELECT
		string_agg(format('SELECT setval(''%I.%I'', 1, FALSE)', s.sequence_schema, s.sequence_name), '; ')
	FROM information_schema.sequences s
	WHERE s.sequence_schema in ('auth'));
END
$func$ LANGUAGE plpgsql;