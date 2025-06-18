using Legion.Database.Readers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Legion.Database.SqlServer.Readers;

public class SqlServerReader : ISqlReader
{
	private readonly string _connectionString;

	public SqlServerReader(string connectionString)
	{
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		_connectionString = connectionString;
	}

	public DataSet LoadAllData(string dbSchemaName, string dbObjectName)
	{
		Throw.IfArgumentNullOrWhiteSpace(dbSchemaName);
		Throw.IfArgumentNullOrWhiteSpace(dbObjectName);

		string query = string.IsNullOrWhiteSpace(dbSchemaName)
			? $"SELECT * FROM [{dbObjectName}]"
			: $"SELECT * FROM [{dbSchemaName}].[{dbObjectName}]";

		using var connection = new SqlConnection(_connectionString);
		connection.Open();

		using var adapter = new SqlDataAdapter(query, connection);
		var result = new DataSet();
		adapter.Fill(result);
		return result;
	}
}
