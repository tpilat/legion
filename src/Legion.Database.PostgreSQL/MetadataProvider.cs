using Legion.Database.Metamodel;
using Legion.Enums;
using Legion.Extensions;
using Legion.MathUtils;
using Npgsql;

namespace Legion.Database.PostgreSQL;

public class MetadataProvider : IDatabaseMetadataProvider
{
	private Database.Internal.DatabaseModel? _model;

	/* POSTGRE SQL DATA TYPES:

	SELECT pg_catalog.format_type(t.oid, NULL) AS "Name"
	FROM pg_catalog.pg_type t
	WHERE t.typrelid = 0
	  --AND NOT EXISTS(SELECT 1 FROM pg_catalog.pg_type el WHERE el.oid = t.typelem AND el.typarray = t.oid)
	  AND pg_catalog.pg_type_is_visible(t.oid)
	ORDER BY 1

		 */

	public IDatabaseModel? Model => _model;

	public bool LoadMetadata(string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
			throw new ArgumentNullException(nameof(connectionString));

		var connectionStringBuilder =
			new NpgsqlConnectionStringBuilder(connectionString);

		return LoadMetadata(connectionString, connectionStringBuilder.Database!);
	}

	public bool LoadMetadata(string connectionString, string databaseName)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
			throw new ArgumentNullException(nameof(connectionString));

		if (string.IsNullOrWhiteSpace(databaseName))
			throw new ArgumentNullException(nameof(databaseName));

		var connectionStringBuilder =
			new NpgsqlConnectionStringBuilder(connectionString);

		if (string.IsNullOrWhiteSpace(connectionStringBuilder.ApplicationName))
			connectionStringBuilder.ApplicationName = $"{nameof(Legion)}.{nameof(Legion.Database)}.{nameof(Legion.Database.PostgreSQL)}.{nameof(MetadataProvider)}";

		using var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
		connection.Open();

		_model = GetDatabase(connection, databaseName);
		GetAllSchemas(connection);
		GetAllTables(connection);
		GetAllViews(connection);
		GetTableColumns(connection);
		GetViewColumns(connection);
		GetAllPrimaryKeys(connection);
		GetAllUniqueConstraints(connection);
		GetAllForeignKeys(connection);
		GetAllIndexes(connection);
		GetAllSequences(connection);

		return _model.Build();
	}

	private static Database.Internal.DatabaseModel GetDatabase(NpgsqlConnection connection, string databaseName)
	{
		string cmd = $@"
				SELECT d.oid as ""DatabaseId"",
					d.datname as ""DatabaseName"",
					(pg_stat_file('base/' || d.oid || '/PG_VERSION')).creation as ""CreationDate"",
					d.datcollate as ""CollationName""
				FROM pg_database as d
				WHERE d.datistemplate = false AND lower(d.datname) = '{databaseName.ToLower()}'
				ORDER BY d.datname
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var dbName = reader.GetValueOrDefault<string>("DatabaseName")!;

			if (!string.Equals(databaseName, dbName, StringComparison.InvariantCultureIgnoreCase))
				continue;

			return new Legion.Database.Internal.DatabaseModel
			{
				ProviderType = DatabaseProviderType.PostgreSQL,
				Id = Convert.ToInt32(reader.GetValueOrNull<uint>("DatabaseId")),
				Name = dbName,
				CollationName = reader.GetValueOrDefault<string>("CollationName"),
				CreationDate = (DateTime?)reader.GetValueOrNull<DateTime>("CreationDate"),
				DefaultSchema = "public"
			};
		}

		throw new InvalidOperationException($"No database found | {nameof(databaseName)} = {databaseName}");
	}

	private void GetAllSchemas(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT current_database() as ""DatabaseName"",
					oid as ""SchemaId"",
					nspname as ""SchemaName""
				FROM pg_namespace
				WHERE nspname NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema')
				ORDER BY nspname
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var alias = reader.GetValueOrDefault<string>("SchemaName")!;
			var schema = new Legion.Database.Internal.Schema
			{
				Id = Convert.ToInt32(reader.GetValueOrNull<uint>("SchemaId")),
				Alias = alias,
				Name = alias,
			};

			_model!.Schemas ??= [];
			_model!.Schemas.Add(schema);
		}
	}

	private void GetAllTables(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT cls.oid as ""ObjectId"",
					relname as ""ObjectName"",
					ns.oid as ""SchemaId"",
					relkind as ""DbType""
				FROM pg_class as cls
				INNER JOIN pg_namespace as ns ON cls.relnamespace = ns.oid
				WHERE relkind = 'r' AND (ns.nspname NOT IN('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema'))
				ORDER BY ns.oid, relname
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var dbType = reader.GetValueOrDefault<char>("DbType");
			if (dbType == 'r')
			{
				var schemaId = Convert.ToInt32(reader.GetValueOrNull<uint>("SchemaId"));
				var schema = _model!.Schemas?.FirstOrDefault(s => s.Id == schemaId);
				if (schema == null)
					throw new InvalidOperationException($"{nameof(schema)} == null | {nameof(schemaId)} = {schemaId}");

				var table = new Legion.Database.Internal.Table
				{
					Id = Convert.ToInt32(reader.GetValueOrNull<uint>("ObjectId")),
					Name = reader.GetValueOrDefault<string>("ObjectName")!
				};

				schema.Tables ??= [];
				schema.Tables.Add(table);
			}
		}
	}

	private void GetAllViews(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT cls.oid as ""ObjectId"",
					relname as ""ObjectName"",
					ns.oid as ""SchemaId"",
					relkind as ""DbType"",
					vw.definition
				FROM pg_class as cls
				INNER JOIN pg_namespace as ns ON cls.relnamespace = ns.oid
				JOIN pg_views vw ON relname = vw.viewname and ns.nspname = vw.schemaname 
				WHERE relkind = 'v' AND (ns.nspname NOT IN('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema'))
				ORDER BY ns.oid, relname
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var dbType = reader.GetValueOrDefault<char>("DbType");
			if (dbType == 'v')
			{
				var schemaId = Convert.ToInt32(reader.GetValueOrNull<uint>("SchemaId"));
				var schema = _model!.Schemas?.FirstOrDefault(s => s.Id == schemaId);
				if (schema == null)
					throw new InvalidOperationException($"{nameof(schema)} == null | {nameof(schemaId)} = {schemaId}");

				var view = new Legion.Database.Internal.View
				{
					Id = Convert.ToInt32(reader.GetValueOrNull<uint>("ObjectId")),
					Name = reader.GetValueOrDefault<string>("ObjectName")!,
					Definition = reader.GetValueOrDefault<string>("definition")!,
				};

				schema.Views ??= [];
				schema.Views.Add(view);
			}
		}
	}

	private void GetTableColumns(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT
					table_catalog as ""DatabaseName"",
					table_schema as ""SchemaName"",
					table_name as ""TableName"",
					column_name as ""ColumnName"",
					ordinal_position as ""OrdinalPosition"",
					column_default as ""DefaultValue"",
					is_nullable as ""IsNullable"",
					CASE 
						WHEN data_type = 'ARRAY' THEN CONCAT(udt_name, '_', data_type)
						ELSE data_type
					END AS ""DataType"",
					character_maximum_length as ""CharacterMaximumLength"",
					COALESCE(numeric_precision, datetime_precision) as ""Precision"",
					numeric_scale as ""Scale"",
					is_identity as ""IsIdentity"",
					identity_start as ""IdentityStart"",
					identity_increment as ""IdentityIncrement"",
					identity_maximum as ""LastIdentity"",
					is_generated as ""IsGenerated""
				FROM information_schema.""columns""
				JOIN pg_class as cls ON cls.relname = table_name and cls.relkind = 'r'
				JOIN pg_namespace as ns ON cls.relnamespace = ns.oid
				WHERE table_schema NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema')
				ORDER BY ordinal_position
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var storeType = reader.GetValueOrDefault<string>("DataType");

			var schemaAlias = reader.GetValueOrDefault<string>("SchemaName");
			var tableName = reader.GetValueOrDefault<string>("TableName");
			var schema = _model!.Schemas?.FirstOrDefault(x => x.Alias == schemaAlias);
			var table = schema?.Tables?.FirstOrDefault(t => t.Name == tableName);
			if (table == null)
				throw new InvalidOperationException($"{nameof(table)} == null | {nameof(schemaAlias)} = {schemaAlias} | {nameof(tableName)} = {tableName}");

			var column = new Legion.Database.Internal.Column
			{
				Name = reader.GetValueOrDefault<string>("ColumnName")!,
				OrdinalPosition = reader.GetValueOrDefault<int>("OrdinalPosition"),
				DefaultValue = reader.GetValueOrDefault<string>("DefaultValue"),
				IsNotNull = !(reader.GetValueOrDefault<string>("IsNullable")?.Equals("YES", StringComparison.OrdinalIgnoreCase) ?? false),
				DatabaseType = storeType!,
				CsharpType = PostgreSQLOriginalDataTypes.StoreTypeToCsharpType(storeType!)!,
				CharacterMaximumLength = reader.GetValueOrDefault<int>("CharacterMaximumLength"),
				Precision = reader.GetValueOrDefault<int>("Precision"),
				Scale = reader.GetValueOrDefault<int>("Scale"),
				IsIdentity = reader.GetValueOrDefault<string>("IsIdentity")?.Equals("YES", StringComparison.OrdinalIgnoreCase) ?? false,
				IdentityStart = MathHelper.LongParseSafe(reader.GetValueOrDefault<string>("IdentityStart")) ?? 0,
				IdentityIncrement = MathHelper.LongParseSafe(reader.GetValueOrDefault<string>("IdentityIncrement")) ?? 0,
				LastIdentity = MathHelper.LongParseSafe(reader.GetValueOrDefault<string>("LastIdentity")) ?? 0,
				ValueGenerated = EnumHelper.ConvertStringToEnum<Internal.ValueGenerated>(reader.GetValueOrDefault<string>("IsGenerated") ?? Internal.ValueGenerated.Never.ToString(), true)
			};

			table.Columns ??= [];
			table.Columns.Add(column);
		}
	}

	private void GetViewColumns(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT
					table_catalog as ""DatabaseName"",
					table_schema as ""SchemaName"",
					table_name as ""ViewName"",
					column_name as ""ColumnName"",
					ordinal_position as ""OrdinalPosition"",
					column_default as ""DefaultValue"",
					is_nullable as ""IsNullable"",
					CASE 
						WHEN data_type = 'ARRAY' THEN CONCAT(udt_name, '_', data_type)
						ELSE data_type
					END AS ""DataType"",
					character_maximum_length as ""CharacterMaximumLength"",
					COALESCE(numeric_precision, datetime_precision) as ""Precision"",
					numeric_scale as ""Scale"",
					is_identity as ""IsIdentity"",
					identity_start as ""IdentityStart"",
					identity_increment as ""IdentityIncrement"",
					identity_maximum as ""LastIdentity"",
					is_generated as ""IsGenerated""
				FROM information_schema.""columns""
				JOIN pg_class as cls ON cls.relname = table_name and cls.relkind = 'v'
				JOIN pg_namespace as ns ON cls.relnamespace = ns.oid
				WHERE table_schema NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema')
				ORDER BY ordinal_position
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var storeType = reader.GetValueOrDefault<string>("DataType");

			var schemaAlias = reader.GetValueOrDefault<string>("SchemaName");
			var viewName = reader.GetValueOrDefault<string>("ViewName");
			var schema = _model!.Schemas?.FirstOrDefault(x => x.Alias == schemaAlias);
			var view = schema?.Views?.FirstOrDefault(t => t.Name == viewName);
			if (view == null)
				throw new InvalidOperationException($"{nameof(view)} == null | {nameof(schemaAlias)} = {schemaAlias} | {nameof(viewName)} = {viewName}");

			var column = new Legion.Database.Internal.Column
			{
				Name = reader.GetValueOrDefault<string>("ColumnName")!,
				OrdinalPosition = reader.GetValueOrDefault<int>("OrdinalPosition"),
				DefaultValue = reader.GetValueOrDefault<string>("DefaultValue"),
				IsNotNull = !(reader.GetValueOrDefault<string>("IsNullable")?.Equals("YES", StringComparison.OrdinalIgnoreCase) ?? false),
				DatabaseType = storeType!,
				CsharpType = PostgreSQLOriginalDataTypes.StoreTypeToCsharpType(storeType!)!,
				CharacterMaximumLength = reader.GetValueOrDefault<int>("CharacterMaximumLength"),
				Precision = reader.GetValueOrDefault<int>("Precision"),
				Scale = reader.GetValueOrDefault<int>("Scale"),
				IsIdentity = reader.GetValueOrDefault<string>("IsIdentity")?.Equals("YES", StringComparison.OrdinalIgnoreCase) ?? false,
				IdentityStart = MathHelper.LongParseSafe(reader.GetValueOrDefault<string>("IdentityStart")) ?? 0,
				IdentityIncrement = MathHelper.LongParseSafe(reader.GetValueOrDefault<string>("IdentityIncrement")) ?? 0,
				LastIdentity = MathHelper.LongParseSafe(reader.GetValueOrDefault<string>("LastIdentity")) ?? 0,
				ValueGenerated = EnumHelper.ConvertStringToEnum<Internal.ValueGenerated>(reader.GetValueOrDefault<string>("IsGenerated") ?? Internal.ValueGenerated.Never.ToString(), true)
			};

			view.Columns ??= [];
			view.Columns.Add(column);
		}
	}

	private void GetAllPrimaryKeys(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT kc.*
				FROM information_schema.table_constraints tc
				JOIN information_schema.key_column_usage kc ON kc.table_name = tc.table_name AND kc.table_schema = tc.table_schema AND kc.constraint_name = tc.constraint_name
				WHERE tc.constraint_type = 'PRIMARY KEY' AND kc.constraint_schema NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema')
				ORDER BY tc.table_schema,
					tc.table_name,
					tc.constraint_name,
					kc.ordinal_position,
					kc.position_in_unique_constraint
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var schemaAlias = reader.GetValueOrDefault<string>("table_schema");
			var tableName = reader.GetValueOrDefault<string>("table_name");
			var schema = _model!.Schemas?.FirstOrDefault(x => x.Alias == schemaAlias);
			var table = schema?.Tables?.FirstOrDefault(t => t.Name == tableName);
			if (table == null)
				throw new InvalidOperationException($"{nameof(table)} == null | {nameof(schemaAlias)} = {schemaAlias} | {nameof(tableName)} = {tableName}");

			var primaryKeyName = reader.GetValueOrDefault<string>("constraint_name")!;

			var primaryKey = table.PrimaryKey;

			if (primaryKey == null)
			{
				primaryKey = new Legion.Database.Internal.PrimaryKey
				{
					Name = primaryKeyName
				};

				table.PrimaryKey = primaryKey;
			}

			primaryKey.Columns ??= [];
			primaryKey.Columns.Add(reader.GetValueOrDefault<string>("column_name")!);
		}
	}

	private void GetAllUniqueConstraints(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT kc.*
				FROM information_schema.table_constraints tc
				JOIN information_schema.key_column_usage kc ON kc.table_name = tc.table_name AND kc.table_schema = tc.table_schema AND kc.constraint_name = tc.constraint_name
				WHERE tc.constraint_type = 'UNIQUE' AND kc.constraint_schema NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema')
				ORDER BY tc.table_schema,
					tc.table_name,
					tc.constraint_name,
					kc.ordinal_position,
					kc.position_in_unique_constraint
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var schemaAlias = reader.GetValueOrDefault<string>("table_schema");
			var tableName = reader.GetValueOrDefault<string>("table_name");
			var schema = _model!.Schemas?.FirstOrDefault(x => x.Alias == schemaAlias);
			var table = schema?.Tables?.FirstOrDefault(t => t.Name == tableName);
			if (table == null)
				throw new InvalidOperationException($"{nameof(table)} == null | {nameof(schemaAlias)} = {schemaAlias} | {nameof(tableName)} = {tableName}");

			var uniqueConstraintName = reader.GetValueOrDefault<string>("constraint_name")!;
			var uniqueConstraint = table.UniqueConstraints?.FirstOrDefault(uq => uq.Name == uniqueConstraintName);

			if (uniqueConstraint == null)
			{
				uniqueConstraint = new Legion.Database.Internal.UniqueConstraint
				{
					Name = uniqueConstraintName
				};

				table.UniqueConstraints ??= [];
				table.UniqueConstraints.Add(uniqueConstraint);
			}

			uniqueConstraint.Columns ??= [];
			uniqueConstraint.Columns.Add(reader.GetValueOrDefault<string>("column_name")!);
		}
	}

	private void GetAllForeignKeys(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT kc.*,
					cc.table_schema AS foreign_table_schema,
					cc.table_name AS foreign_table_name,
					cc.column_name AS foreign_column_name,
					rc.match_option,
					rc.update_rule,
					rc.delete_rule
				FROM information_schema.table_constraints tc
				JOIN information_schema.key_column_usage kc ON kc.table_name = tc.table_name AND kc.table_schema = tc.table_schema AND kc.constraint_name = tc.constraint_name
				JOIN information_schema.constraint_column_usage cc ON cc.constraint_schema = tc.constraint_schema AND cc.constraint_name = tc.constraint_name
				JOIN information_schema.referential_constraints rc ON rc.constraint_schema = tc.constraint_schema AND rc.constraint_name = tc.constraint_name
				WHERE tc.constraint_type = 'FOREIGN KEY' AND kc.constraint_schema NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema')
				ORDER BY tc.table_schema,
					tc.table_name,
					tc.constraint_name,
					kc.ordinal_position,
					kc.position_in_unique_constraint
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var schemaAlias = reader.GetValueOrDefault<string>("table_schema");
			var tableName = reader.GetValueOrDefault<string>("table_name");
			var schema = _model!.Schemas?.FirstOrDefault(x => x.Alias == schemaAlias);
			var table = schema?.Tables?.FirstOrDefault(t => t.Name == tableName);
			if (table == null)
				throw new InvalidOperationException($"{nameof(table)} == null | {nameof(schemaAlias)} = {schemaAlias} | {nameof(tableName)} = {tableName}");

			var foreignSchemaAlias = reader.GetValueOrDefault<string>("foreign_table_schema");
			var foreignTableName = reader.GetValueOrDefault<string>("foreign_table_name");
			var foreignSchema = _model!.Schemas?.FirstOrDefault(x => x.Alias == foreignSchemaAlias);
			var foreignTable = foreignSchema?.Tables?.FirstOrDefault(t => t.Name == foreignTableName);
			if (foreignTable == null)
				throw new InvalidOperationException($"{nameof(foreignTable)} == null | {nameof(foreignSchemaAlias)} = {foreignSchemaAlias} | {nameof(foreignTableName)} = {foreignTableName}");

			var foreignKey = new Legion.Database.Internal.ForeignKey
			{
				Name = reader.GetValueOrDefault<string>("constraint_name")!,
				Column = reader.GetValueOrDefault<string>("column_name")!,
				ForeignSchemaAlias = foreignSchema!.Alias,
				ForeignTableName = foreignTableName!,
				ForeignColumnName = reader.GetValueOrDefault<string>("foreign_column_name")!,
				OnUpdateAction = EnumHelper.ConvertStringToEnum<ReferentialAction>(reader.GetValueOrDefault<string>("update_rule")?.Replace(" ", "")!, true),
				OnDeleteAction = EnumHelper.ConvertStringToEnum<ReferentialAction>(reader.GetValueOrDefault<string>("delete_rule")?.Replace(" ", "")!, true),
				MatchOption = EnumHelper.ConvertStringToEnum<MatchOprions>(reader.GetValueOrDefault<string>("match_option")?.Replace(" ", "")!, true)
			};

			table.ForeignKeys ??= [];
			table.ForeignKeys.Add(foreignKey);
		}
	}

	private void GetAllIndexes(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT
					U.usename AS user_name,
					tnsp.nspname AS table_schema,
					trel.relname AS table_name,
					irel.relname AS index_name,
					a.attname AS column_name,  
					i.indisunique AS is_unique,
					i.indisprimary AS is_primary,  
					(i.indexprs IS NOT NULL) OR(i.indkey::int[] @> array[0]) AS is_functional,
					i.indpred IS NOT NULL AS is_partial
				FROM pg_index AS i
					JOIN pg_class AS trel ON trel.oid = i.indrelid
					JOIN pg_namespace AS tnsp ON trel.relnamespace = tnsp.oid
					JOIN pg_class AS irel ON irel.oid = i.indexrelid
					CROSS JOIN LATERAL unnest(i.indkey) WITH ORDINALITY AS c(colnum, ordinality)
					LEFT JOIN LATERAL unnest(i.indoption) WITH ORDINALITY AS o(option, ordinality) ON c.ordinality = o.ordinality
					JOIN pg_attribute AS a ON trel.oid = a.attrelid AND a.attnum = c.colnum
					JOIN pg_user AS U ON trel.relowner = U.usesysid
				WHERE --i.indisunique <> TRUE AND i.indisprimary <> TRUE AND
					NOT nspname LIKE 'pg%' --Excluding system tables
				ORDER BY U.usename,
					tnsp.nspname,
					trel.relname,
					irel.relname,
					a.attname
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var schemaAlias = reader.GetValueOrDefault<string>("table_schema");
			var tableName = reader.GetValueOrDefault<string>("table_name");
			var schema = _model!.Schemas?.FirstOrDefault(x => x.Alias == schemaAlias);
			var table = schema?.Tables?.FirstOrDefault(t => t.Name == tableName);
			if (table == null)
				throw new InvalidOperationException($"{nameof(table)} == null | {nameof(schemaAlias)} = {schemaAlias} | {nameof(tableName)} = {tableName}");

			var indexName = reader.GetValueOrDefault<string>("index_name")!;
			var index = table.Indexes?.FirstOrDefault(idx => idx.Name == indexName);

			if (index == null)
			{
				index = new Legion.Database.Internal.Index
				{
					Name = indexName,
					IsUnique = reader.GetValueOrDefault<bool>("is_unique"),
					IsPrimary = reader.GetValueOrDefault<bool>("is_primary")
				};

				table.Indexes ??= [];
				table.Indexes.Add(index);
			}

			index.Columns ??= [];
			index.Columns.Add(reader.GetValueOrDefault<string>("column_name")!);
		}
	}

	private void GetAllSequences(NpgsqlConnection connection)
	{
		string cmd = @"
				SELECT
					sequence_schema,
					sequence_name,
					data_type as store_type,
					start_value,
					increment as increment_by,
					minimum_value,
					maximum_value,
					cycle_option
				FROM information_schema.sequences
				WHERE sequence_schema NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1', 'pg_catalog', 'information_schema')
				ORDER BY sequence_schema, sequence_name
				";

		using var command = new NpgsqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var schema = _model!.Schemas?.FirstOrDefault(s => s.Name == reader.GetValueOrDefault<string>("sequence_schema"));

			if (schema == null)
				throw new Exception($"{nameof(schema)} == null");

			var storeType = reader.GetValueOrDefault<string>("store_type");

			var sequence = new Internal.Sequence
			{
				Schema = schema,
				Name = reader.GetValueOrDefault<string>("sequence_name"),
				StoreType = storeType,
				CsharpType = PostgreSQLOriginalDataTypes.StoreTypeToCsharpType(storeType),
				StartValue = long.Parse(reader.GetValueOrDefault<string>("start_value")),
				IncrementBy = int.Parse(reader.GetValueOrDefault<string>("increment_by")),
				MinValue = long.Parse(reader.GetValueOrDefault<string>("minimum_value")),
				MaxValue = long.Parse(reader.GetValueOrDefault<string>("maximum_value")),
				IsCyclic = reader.GetValueOrDefault<string>("cycle_option")?.Equals("YES", StringComparison.OrdinalIgnoreCase) ?? false
			};

			schema.Sequences ??= [];
			schema.Sequences.Add(sequence);
		}
	}
}
