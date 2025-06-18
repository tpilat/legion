SET client_min_messages TO WARNING; 

DROP Schema If Exists comp Cascade;
CREATE Schema comp;

DROP Schema If Exists mbox Cascade;
CREATE Schema mbox;

DROP Schema If Exists orch Cascade;
CREATE Schema orch;


CREATE OR REPLACE FUNCTION f_truncate_tables()
  RETURNS void AS
$func$
BEGIN
   EXECUTE
  (SELECT 'TRUNCATE TABLE '
       || string_agg(format('%I.%I', schemaname, tablename), ', ')
       || ' RESTART IDENTITY CASCADE'
   FROM   pg_tables
   WHERE  tableowner = 'postgres' AND schemaname in ('comp', 'mbox', 'orch'));

  EXECUTE
  (SELECT
		string_agg(format('SELECT setval(''%I.%I'', 1, FALSE)', s.sequence_schema, s.sequence_name), '; ')
	FROM information_schema.sequences s
	WHERE s.sequence_schema in ('comp', 'mbox', 'orch'));
END
$func$ LANGUAGE plpgsql;