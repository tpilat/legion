using Legion.Database.Readers;
using Npgsql;
using System.Data;

namespace Legion.Database.PostgreSQL.Readers;

public class PostgreSQLReader : ISqlReader
{
	private readonly string _connectionString;

	public PostgreSQLReader(string connectionString)
	{
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		_connectionString = connectionString;
	}

	public DataSet LoadAllData(string dbSchemaName, string dbObjectName)
	{
		Throw.IfArgumentNullOrWhiteSpace(dbSchemaName);
		Throw.IfArgumentNullOrWhiteSpace(dbObjectName);

		string query = string.IsNullOrWhiteSpace(dbSchemaName)
			? $"SELECT * FROM \"{dbObjectName}\" ORDER BY 1"
			: $"SELECT * FROM {dbSchemaName}.\"{dbObjectName}\" ORDER BY 1";

		using var connection = new NpgsqlConnection(_connectionString);
		connection.Open();

		using var adapter = new NpgsqlDataAdapter(query, connection);
		var result = new DataSet();
		adapter.Fill(result);
		return result;
	}
}
