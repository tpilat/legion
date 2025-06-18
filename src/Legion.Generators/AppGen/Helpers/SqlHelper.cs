using Legion.Database.Metamodel;
using Legion.Database.PostgreSQL.Readers;
using Legion.Database.Readers;
using Legion.Database.SqlServer.Readers;

namespace Legion.Generators.AppGen.Helpers;

public static class SqlHelper
{
	public static ISqlReader Create(string connectionString, DatabaseProviderType databaseProviderType)
	{
		if (databaseProviderType == DatabaseProviderType.SqlServer)
		{
			return new SqlServerReader(connectionString);
		}
		else if (databaseProviderType == DatabaseProviderType.PostgreSQL)
		{
			return new PostgreSQLReader(connectionString);
		}

		Throw.NotSupportedException($"{nameof(databaseProviderType)} = {databaseProviderType}");
		return null;
	}

	public static string DBTypeToNullableType(string columnType, bool includeNullability, bool isNullable, int? maxLength)
	{
		var nullability = includeNullability
			? $" {(isNullable ? "" : "NOT ")}NULL"
			: "";

		return columnType.ToLower() switch
		{
			"bigint" => $"bigint{nullability}",
			"boolean" => $"boolean{nullability}",
			"bytea" => $"bytea{nullability}",
			"character varying" => $"varchar({maxLength}){nullability}",
			"double precision" => $"double precision{nullability}",
			"integer" => $"integer{nullability}",
			"interval" => $"interval{nullability}",
			"jsonb" => $"jsonb{nullability}",
			"numeric" => $"numeric{nullability}",
			"text" => $"text{nullability}",
			"timestamp without time zone" => $"timestamp without time zone{nullability}",
			"timestamp with time zone" => $"timestamp with time zone{nullability}",
			"uuid" => $"uuid{nullability}",
			"character" => $"char({maxLength}){nullability}",
			"smallint" => $"smallint{nullability}",
			"tsvector" => $"tsvector{nullability}",

			"_text_array" => $"text[]{nullability}",

			"bit" => $"bit{nullability}",
			"char" => $"char({(maxLength.HasValue ? maxLength.ToString() : "max")}){nullability}",
			"datetime2" => $"datetime2{nullability}", //ALWAYS USE datetime2 over datetime
			"decimal" => $"decimal{nullability}",
			"float" => $"float{nullability}",
			"int" => $"int{nullability}",
			"nchar" => $"nchar({(maxLength.HasValue ? maxLength.ToString() : "max")}){nullability}",
			"nvarchar" => $"nvarchar({(maxLength.HasValue ? maxLength.ToString() : "max")}){nullability}",
			"real" => $"real{nullability}",
			"time" => $"time{nullability}",
			"uniqueidentifier" => $"uniqueidentifier{nullability}",
			"varbinary" => $"varbinary({(maxLength.HasValue ? maxLength.ToString() : "max")}){nullability}",
			"varchar" => $"varchar({(maxLength.HasValue ? maxLength.ToString() : "max")}){nullability}",

			_ => throw new NotSupportedException(columnType),
		};
	}
}
