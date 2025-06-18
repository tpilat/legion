using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Legion.Database.SqlServer;

public static class SqlScript
{
	private static SqlConnection ConnectToDB(string connectionString)
	{
		Throw.IfArgumentNullOrWhiteSpace(connectionString);

		if (connectionString.Contains("{0}"))
			connectionString = string.Format(connectionString, System.Environment.GetEnvironmentVariable("MSSQLPASSWORD"));

		var _connection = new SqlConnection(connectionString);
		_connection.Open();
		return _connection;
	}

	public static string? Execute(string connectionString, string script, bool createTransaction)
		=> Execute(ConnectToDB(connectionString), script, createTransaction);


	public static string? Execute(SqlConnection sqlServerConnection, string sqlScript, bool createTransaction)
	{
		Throw.IfArgumentNull(sqlServerConnection);

		try
		{
			if (createTransaction)
			{
				using var tran = sqlServerConnection!.BeginTransaction();
				try
				{
					var scripts = SplitSqlStatements(sqlScript);
					foreach (var script in scripts)
					{
						using var cmd = new SqlCommand(script, sqlServerConnection, tran);
						cmd.ExecuteNonQuery();
					}

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
				var scripts = SplitSqlStatements(sqlScript);
				foreach (var script in scripts)
				{
					using var cmd = new SqlCommand(script, sqlServerConnection);
					cmd.ExecuteNonQuery();
				}
			}
		}
		catch (Exception ex)
		{
			return ex.ToString();
		}

		return null;
	}

	public static IEnumerable<string> SplitSqlStatements(string sqlScript)
	{
		sqlScript = Regex.Replace(sqlScript, @"(\r\n|\n\r|\n|\r)", "\n");

		var statements = Regex.Split(
				sqlScript,
				@"^\s*GO\s*\d*\s*(?:--.*)?$",
				RegexOptions.Multiline |
				RegexOptions.IgnorePatternWhitespace |
				RegexOptions.IgnoreCase);

		return statements
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => x.Trim(' ', '\n'));
	}
}
