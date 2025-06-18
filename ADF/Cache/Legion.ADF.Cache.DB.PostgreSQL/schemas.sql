SET client_min_messages TO WARNING; 

DROP Schema If Exists cache Cascade;
CREATE Schema cache;


CREATE OR REPLACE FUNCTION f_truncate_tables()
  RETURNS void AS
$func$
BEGIN
   EXECUTE
  (SELECT 'TRUNCATE TABLE '
       || string_agg(format('%I.%I', schemaname, tablename), ', ')
       || ' RESTART IDENTITY CASCADE'
   FROM   pg_tables
   WHERE  tableowner = 'postgres' AND schemaname in ('cache'));

  EXECUTE
  (SELECT
		string_agg(format('SELECT setval(''%I.%I'', 1, FALSE)', s.sequence_schema, s.sequence_name), '; ')
	FROM information_schema.sequences s
	WHERE s.sequence_schema in ('cache'));
END
$func$ LANGUAGE plpgsql;