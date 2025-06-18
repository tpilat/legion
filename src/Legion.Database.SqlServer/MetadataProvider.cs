using Legion.Database.Metamodel;
using Legion.Enums;
using Legion.Extensions;
using Legion.MathUtils;
using Microsoft.Data.SqlClient;

namespace Legion.Database.SqlServer;

public class MetadataProvider : IDatabaseMetadataProvider
{
	private Database.Internal.DatabaseModel? _model;

	/* SQL SERVER DATA TYPES:

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
			new SqlConnectionStringBuilder(connectionString);

		return LoadMetadata(connectionString, connectionStringBuilder.InitialCatalog!);
	}

	public bool LoadMetadata(string connectionString, string databaseName)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
			throw new ArgumentNullException(nameof(connectionString));

		if (string.IsNullOrWhiteSpace(databaseName))
			throw new ArgumentNullException(nameof(databaseName));

		var connectionStringBuilder =
			new SqlConnectionStringBuilder(connectionString);

		if (string.IsNullOrWhiteSpace(connectionStringBuilder.ApplicationName))
			connectionStringBuilder.ApplicationName = $"{nameof(Legion)}.{nameof(Legion.Database)}.{nameof(Legion.Database.SqlServer)}.{nameof(MetadataProvider)}";

		using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
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

	private static Database.Internal.DatabaseModel GetDatabase(SqlConnection connection, string databaseName)
	{
		string cmd = $@"
				SELECT
					database_id as ""DatabaseId"",
					name as ""DatabaseName"",
					create_date as ""CreationDate"",
					collation_name as ""CollationName""
				FROM sys.databases
				WHERE lower(name) = '{databaseName.ToLower()}'
				ORDER BY name
				";

		using var command = new SqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var dbName = reader.GetValueOrDefault<string>("DatabaseName")!;

			if (!string.Equals(databaseName, dbName, StringComparison.InvariantCultureIgnoreCase))
				continue;

			return new Legion.Database.Internal.DatabaseModel
			{
				ProviderType = DatabaseProviderType.SqlServer,
				Id = Convert.ToInt32(reader.GetValueOrNull<int>("DatabaseId")),
				Name = dbName,
				CollationName = reader.GetValueOrDefault<string>("CollationName"),
				CreationDate = (DateTime?)reader.GetValueOrNull<DateTime>("CreationDate"),
				DefaultSchema = "public"
			};
		}

		throw new InvalidOperationException($"No database found | {nameof(databaseName)} = {databaseName}");
	}

	private void GetAllSchemas(SqlConnection connection)
	{
		string cmd = @"
				SELECT
					schema_id as ""SchemaId"",
					name as ""SchemaName""
				FROM sys.schemas
				WHERE name not in ('guest', 'INFORMATION_SCHEMA', 'sys', 'db_owner', 'db_accessadmin', 'db_securityadmin', 'db_ddladmin', 'db_backupoperator', 'db_datareader', 'db_datawriter', 'db_denydatareader', 'db_denydatawriter')
				ORDER BY name
				";

		using var command = new SqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var alias = reader.GetValueOrDefault<string>("SchemaName")!;
			var schema = new Legion.Database.Internal.Schema
			{
				Id = Convert.ToInt32(reader.GetValueOrNull<int>("SchemaId")),
				Alias = alias,
				Name = alias,
			};

			_model!.Schemas ??= [];
			_model!.Schemas.Add(schema);
		}
	}

	private void GetAllTables(SqlConnection connection)
	{
		string cmd = @"
				SELECT
					object_id as ""ObjectId"",
					name as ""ObjectName"",
					schema_id as ""SchemaId"",
					type as ""DbType""
				FROM sys.tables
				ORDER BY schema_id, name
				";

		using var command = new SqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var dbType = reader.GetValueOrDefault<string>("DbType")?.Trim();
			if (dbType == "U")
			{
				var schemaId = Convert.ToInt32(reader.GetValueOrNull<int>("SchemaId"));
				var schema = _model!.Schemas?.FirstOrDefault(s => s.Id == schemaId);
				if (schema == null)
					throw new InvalidOperationException($"{nameof(schema)} == null | {nameof(schemaId)} = {schemaId}");

				var table = new Legion.Database.Internal.Table
				{
					Id = Convert.ToInt32(reader.GetValueOrNull<int>("ObjectId")),
					Name = reader.GetValueOrDefault<string>("ObjectName")!
				};

				schema.Tables ??= [];
				schema.Tables.Add(table);
			}
		}
	}

	private void GetAllViews(SqlConnection connection)
	{
		string cmd = @"
				SELECT
					v.object_id as ""ObjectId"",
					v.name as ""ObjectName"",
					v.schema_id as ""SchemaId"",
					v.type as ""DbType"",
					m.definition
				FROM sys.views v
				INNER JOIN sys.sql_modules m ON v.object_id = m.object_id
				ORDER BY v.schema_id, v.name
				";

		using var command = new SqlCommand(cmd, connection);
		using var reader = command.ExecuteReader();

		while (reader.Read())
		{
			var dbType = reader.GetValueOrDefault<string>("DbType")?.Trim();
			if (dbType == "V")
			{
				var schemaId = Convert.ToInt32(reader.GetValueOrNull<int>("SchemaId"));
				var schema = _model!.Schemas?.FirstOrDefault(s => s.Id == schemaId);
				if (schema == null)
					throw new InvalidOperationException($"{nameof(schema)} == null | {nameof(schemaId)} = {schemaId}");

				var view = new Legion.Database.Internal.View
				{
					Id = Convert.ToInt32(reader.GetValueOrNull<int>("ObjectId")),
					Name = reader.GetValueOrDefault<string>("ObjectName")!,
					Definition = reader.GetValueOrDefault<string>("definition")!,
				};

				schema.Views ??= [];
				schema.Views.Add(view);
			}
		}
	}

	private void GetTableColumns(SqlConnection connection)
	{
		string cmd = @"
				SELECT
					s.name AS ""SchemaName"",
					t.name AS ""TableName"",
					c.name AS ""ColumnName"",
					c.column_id AS ""OrdinalPosition"",
					d.definition AS ""DefaultValue"",
					c.is_nullable AS ""IsNullable"",
					tp.name AS ""DataType"",
					CASE
						WHEN tp.name IN ('nchar', 'nvarchar', 'ntext') THEN c.max_length / 2
						ELSE c.max_length
					END AS ""CharacterMaximumLength"",
					c.precision AS ""Precision"",
					c.scale AS ""Scale"",
					c.is_identity AS ""IsIdentity"",
					IDENT_SEED(QUOTENAME(s.name) + '.' + QUOTENAME(t.name)) AS ""IdentityStart"",
					IDENT_INCR(QUOTENAME(s.name) + '.' + QUOTENAME(t.name)) AS ""IdentityIncrement"",
					IDENT_CURRENT(QUOTENAME(s.name) + '.' + QUOTENAME(t.name)) AS ""LastIdentity"",
					c.is_computed AS ""IsGenerated""
				FROM sys.columns c
				INNER JOIN sys.tables t ON c.object_id = t.object_id
				INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
				INNER JOIN sys.types tp ON c.user_type_id = tp.user_type_id
				LEFT JOIN sys.default_constraints d ON c.default_object_id = d.object_id
				ORDER BY c.column_id
				";

		using var command = new SqlCommand(cmd, connection);
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
				IsNotNull = !reader.GetValueOrDefault<bool>("IsNullable"),
				DatabaseType = storeType!,
				CsharpType = SqlServerOriginalDataTypes.StoreTypeToCsharpType(storeType!)!,
				CharacterMaximumLength = reader.GetValueOrDefault<int>("CharacterMaximumLength"),
				Precision = reader.GetValueOrDefault<byte>("Precision"),
				Scale = reader.GetValueOrDefault<byte>("Scale"),
				IsIdentity = reader.GetValueOrDefault<bool>("IsIdentity"),
				IdentityStart = reader.GetValueOrDefault<int>("IdentityStart"),
				IdentityIncrement = reader.GetValueOrDefault<int>("IdentityIncrement"),
				LastIdentity = reader.GetValueOrDefault<int>("LastIdentity"),
				ValueGenerated = reader.GetValueOrDefault<bool>("IsGenerated") ? Internal.ValueGenerated.OnAddOrUpdate : Internal.ValueGenerated.Never
			};

			table.Columns ??= [];
			table.Columns.Add(column);
		}
	}

	private void GetViewColumns(SqlConnection connection)
	{
		string cmd = @"
				SELECT
					s.name AS ""SchemaName"",
					v.name AS ""ViewName"",
					c.name AS ""ColumnName"",
					c.column_id AS ""OrdinalPosition"",
					d.definition AS ""DefaultValue"",
					c.is_nullable AS ""IsNullable"",
					tp.name AS ""DataType"",
					CASE 
						WHEN tp.name IN ('nchar', 'nvarchar', 'ntext') THEN c.max_length / 2
						ELSE c.max_length
					END AS ""CharacterMaximumLength"",
					c.precision AS ""Precision"",
					c.scale AS ""Scale"",
					c.is_identity AS ""IsIdentity"",
					IDENT_SEED(QUOTENAME(s.name) + '.' + QUOTENAME(v.name)) AS ""IdentityStart"",
					IDENT_INCR(QUOTENAME(s.name) + '.' + QUOTENAME(v.name)) AS ""IdentityIncrement"",
					IDENT_CURRENT(QUOTENAME(s.name) + '.' + QUOTENAME(v.name)) AS ""LastIdentity"",
					c.is_computed AS ""IsGenerated""
				FROM sys.columns c
				INNER JOIN sys.views v ON c.object_id = v.object_id
				INNER JOIN sys.schemas s ON v.schema_id = s.schema_id
				INNER JOIN sys.types tp ON c.user_type_id = tp.user_type_id
				LEFT JOIN sys.default_constraints d ON c.default_object_id = d.object_id
				ORDER BY c.column_id
				";

		using var command = new SqlCommand(cmd, connection);
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
				IsNotNull = !reader.GetValueOrDefault<bool>("IsNullable"),
				DatabaseType = storeType!,
				CsharpType = SqlServerOriginalDataTypes.StoreTypeToCsharpType(storeType!)!,
				CharacterMaximumLength = reader.GetValueOrDefault<int>("CharacterMaximumLength"),
				Precision = reader.GetValueOrDefault<byte>("Precision"),
				Scale = reader.GetValueOrDefault<byte>("Scale"),
				IsIdentity = reader.GetValueOrDefault<bool>("IsIdentity"),
				IdentityStart = reader.GetValueOrDefault<int>("IdentityStart"),
				IdentityIncrement = reader.GetValueOrDefault<int>("IdentityIncrement"),
				LastIdentity = reader.GetValueOrDefault<int>("LastIdentity"),
				ValueGenerated = reader.GetValueOrDefault<bool>("IsGenerated") ? Internal.ValueGenerated.OnAddOrUpdate : Internal.ValueGenerated.Never
			};

			view.Columns ??= [];
			view.Columns.Add(column);
		}
	}

	private void GetAllPrimaryKeys(SqlConnection connection)
	{
		string cmd = @"
				SELECT 
					s.name AS ""table_schema"",
					t.name AS ""table_name"",
					c.name AS ""column_name"",
					kc.name AS ""constraint_name""
				FROM sys.key_constraints kc
				INNER JOIN sys.tables t ON kc.parent_object_id = t.object_id
				INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
				INNER JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
				INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
				WHERE kc.type = 'PK'
				ORDER BY s.name, t.name, kc.name, ic.key_ordinal
				";

		using var command = new SqlCommand(cmd, connection);
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

	private void GetAllUniqueConstraints(SqlConnection connection)
	{
		string cmd = @"
				SELECT 
					s.name AS ""table_schema"",
					t.name AS ""table_name"",
					c.name AS ""column_name"",
					kc.name AS ""constraint_name""
				FROM sys.key_constraints kc
				INNER JOIN sys.tables t ON kc.parent_object_id = t.object_id
				INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
				INNER JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
				INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
				WHERE kc.type = 'UQ'
				ORDER BY s.name, t.name, kc.name, ic.key_ordinal
				";

		using var command = new SqlCommand(cmd, connection);
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

	private void GetAllForeignKeys(SqlConnection connection)
	{
		string cmd = @"
				SELECT 
					ls.name AS ""table_schema"",
					lt.name AS ""table_name"",
					lc.name AS ""column_name"",
					fs.name AS ""foreign_table_schema"",
					ft.name AS ""foreign_table_name"",
					fc.name AS ""foreign_column_name"",
					fk.name AS ""constraint_name"",
					fk.update_referential_action_desc AS ""update_rule"",
					fk.delete_referential_action_desc AS ""delete_rule""
				FROM sys.foreign_keys fk
				INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
				INNER JOIN sys.tables lt ON fkc.parent_object_id = lt.object_id
				INNER JOIN sys.schemas ls ON lt.schema_id = ls.schema_id
				INNER JOIN sys.columns lc ON fkc.parent_object_id = lc.object_id AND fkc.parent_column_id = lc.column_id
				INNER JOIN sys.tables ft ON fkc.referenced_object_id = ft.object_id
				INNER JOIN sys.schemas fs ON ft.schema_id = fs.schema_id
				INNER JOIN sys.columns fc ON fkc.referenced_object_id = fc.object_id AND fkc.referenced_column_id = fc.column_id
				ORDER BY ls.name, lt.name, fk.name, lc.column_id;
				";

		using var command = new SqlCommand(cmd, connection);
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
				OnUpdateAction = EnumHelper.ConvertStringToEnum<SqlServerForeignKeyUpdateDeleteActionsEnum>(reader.GetValueOrDefault<string>("update_rule")?.Replace(" ", "")!, true).ConvertToReferentialAction(),
				OnDeleteAction = EnumHelper.ConvertStringToEnum<SqlServerForeignKeyUpdateDeleteActionsEnum>(reader.GetValueOrDefault<string>("delete_rule")?.Replace(" ", "")!, true).ConvertToReferentialAction(),
				MatchOption = null //EnumHelper.ConvertStringToEnum<MatchOprions>(reader.GetValueOrDefault<string>("match_option")?.Replace(" ", "")!, true)
			};

			table.ForeignKeys ??= [];
			table.ForeignKeys.Add(foreignKey);
		}
	}

	private void GetAllIndexes(SqlConnection connection)
	{
		string cmd = @"
			SELECT 
				sch.name AS ""table_schema"",
				t.name AS ""table_name"",
				i.name AS ""index_name"",
				c.name AS ""column_name"",
				i.is_unique AS is_unique,
				i.is_primary_key AS is_primary
			FROM sys.indexes i
			INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
			INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
			INNER JOIN sys.tables t ON i.object_id = t.object_id
			INNER JOIN sys.schemas sch ON t.schema_id = sch.schema_id
			ORDER BY sch.name, t.name, i.name, ic.index_column_id
				";

		using var command = new SqlCommand(cmd, connection);
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

	private void GetAllSequences(SqlConnection connection)
	{
		string cmd = @"
			SELECT 
				s.name AS ""sequence_schema"",
				seq.name AS ""sequence_name"",
				t.name AS ""store_type"",
				seq.start_value AS ""start_value"",
				seq.increment AS ""increment_by"",
				seq.minimum_value AS ""minimum_value"",
				seq.maximum_value AS ""maximum_value"",
				seq.current_value AS CurrentValue,
				seq.is_cycling AS IsCycling
			FROM sys.sequences seq
			INNER JOIN sys.schemas s ON seq.schema_id = s.schema_id
			INNER JOIN sys.types t ON seq.system_type_id = t.system_type_id
			ORDER BY  s.name, seq.name;
				";

		using var command = new SqlCommand(cmd, connection);
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
				CsharpType = SqlServerOriginalDataTypes.StoreTypeToCsharpType(storeType),
				StartValue = reader.GetValueOrDefault<int>("start_value"),
				IncrementBy = reader.GetValueOrDefault<int>("increment_by"),
				MinValue = reader.GetValueOrDefault<int>("minimum_value"),
				MaxValue = reader.GetValueOrDefault<int>("maximum_value"),
				IsCyclic = reader.GetValueOrDefault<bool>("IsCycling")
			};

			schema.Sequences ??= [];
			schema.Sequences.Add(sequence);
		}
	}
}
