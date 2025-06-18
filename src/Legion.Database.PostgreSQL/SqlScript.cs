using Npgsql;

namespace Legion.Database.PostgreSQL;

public static class SqlScript
{
	private static NpgsqlConnection ConnectToDB(string connectionString)
	{
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		if (connectionString.Contains("{0}"))
			connectionString = string.Format(connectionString, System.Environment.GetEnvironmentVariable("PGPASSWORD"));

		var _connection = new NpgsqlConnection(connectionString);
		_connection.Open();
		return _connection;
	}

	public static string? Execute(string connectionString, string script, bool createTransaction)
		=> Execute(ConnectToDB(connectionString), script, createTransaction);


	public static string? Execute(NpgsqlConnection npgsqlConnection, string script, bool createTransaction)
	{
		Throw.IfArgumentNull(npgsqlConnection);

		try
		{
			if (createTransaction)
			{
				using var tran = npgsqlConnection!.BeginTransaction();
				try
				{
					using var cmd = new NpgsqlCommand(script, npgsqlConnection, tran);
					cmd.ExecuteNonQuery();
					tran.Commit();
				}
				catch
				{
					tran.Rollback();
					throw;
				}
			}
			else
			{
				using var cmd = new NpgsqlCommand(script, npgsqlConnection);
				cmd.ExecuteNonQuery();
			}
		}
		catch (Exception ex)
		{
			return ex.ToString();
		}

		return null;
	}
}
