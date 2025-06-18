using Legion.Database.Metamodel;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Npgsql;
using System.Data.Common;

namespace Legion.Generators.AppGen.Model.Config;

[Serializable]
public class DatabaseConnection
{
	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public DatabaseProviderType Provider { get; set; }

	[JsonProperty]
	public string ServerName { get; set; }

	[JsonProperty]
	public int? Port { get; set; }

	[JsonProperty]
	public bool UseWindowsIntegrated { get; set; }

	[JsonProperty]
	public string UserName { get; set; }

	[JsonProperty]
	public string Password { get; set; }

	[JsonProperty]
	public bool LoadPassword { get; set; }

	[JsonProperty]
	public string Database { get; set; }

	public DatabaseConnection()
	{
		Provider = DatabaseProviderType.PostgreSQL;
		LoadPassword = true;
	}

	public DbConnectionStringBuilder GetConnectionStringBuilder()
	{
		if (Provider == DatabaseProviderType.SqlServer)
		{
			var connectionBuilder = new SqlConnectionStringBuilder();

			connectionBuilder.TrustServerCertificate = true;

			if (string.IsNullOrWhiteSpace(Database))
				connectionBuilder.InitialCatalog = "master";
			else
				connectionBuilder.InitialCatalog = Database;


			if (string.IsNullOrWhiteSpace(ServerName))
				return null;

			connectionBuilder.DataSource = ServerName;

			if (0 < Port)
				connectionBuilder.DataSource = $"{connectionBuilder.DataSource},{Port}";

			if (UseWindowsIntegrated)
			{
				connectionBuilder.IntegratedSecurity = true;
			}
			else
			{
				connectionBuilder.IntegratedSecurity = false;
				if (!string.IsNullOrWhiteSpace(UserName))
					connectionBuilder.UserID = UserName;

				if (LoadPassword)
				{
					string pwd = System.Environment.GetEnvironmentVariable("MSSQLPASSWORD");
					if (!string.IsNullOrWhiteSpace(pwd))
						connectionBuilder.Password = pwd;
				}
				else
					if (!string.IsNullOrWhiteSpace(Password))
						connectionBuilder.Password = Password;
			}
			return connectionBuilder;
		}
		if (Provider == DatabaseProviderType.PostgreSQL)
		{
			var connectionBuilder = new NpgsqlConnectionStringBuilder();

			if (string.IsNullOrWhiteSpace(Database))
				connectionBuilder.Database = "postgres";
			else
				connectionBuilder.Database = Database;

			if (string.IsNullOrWhiteSpace(ServerName))
				return null;

			connectionBuilder.Host = ServerName;

			if (0 < Port)
				connectionBuilder.Port = Port ?? 5432;

			if (UseWindowsIntegrated)
			{
				Throw.NotSupportedException($"{nameof(UseWindowsIntegrated)} is not allowed for PostgreSQL");
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(UserName))
					connectionBuilder.Username = UserName;

				if (LoadPassword)
				{
					string pwd = System.Environment.GetEnvironmentVariable("PGPASSWORD");
					if (!string.IsNullOrWhiteSpace(pwd))
						connectionBuilder.Password = pwd;
				}
				else
					if (!string.IsNullOrWhiteSpace(Password))
					connectionBuilder.Password = Password;
			}
			return connectionBuilder;
		}

		return null;
	}

	public string GetConnectionString()
	{
		return GetConnectionStringBuilder()?.ConnectionString;
	}

	public string GetEnvironmentVariablePasswordGetter()
	{
		if (Provider == DatabaseProviderType.SqlServer)
		{
			return "System.Environment.GetEnvironmentVariable(\"MSSQLPASSWORD\")";
		}
		if (Provider == DatabaseProviderType.PostgreSQL)
		{
			return "System.Environment.GetEnvironmentVariable(\"PGPASSWORD\")";
		}
		return null;
	}

	public string GetConnectionStringAsContextProvider()
	{
		string connectionString = null;

		if (LoadPassword && !UseWindowsIntegrated)
		{
			string currPassword = Password;
			try
			{
				LoadPassword = false;
				Password = "xxx";
				connectionString = GetConnectionStringBuilder()?.ConnectionString;
			}
			finally
			{
				LoadPassword = true;
				Password = currPassword;
			}

			if (!string.IsNullOrWhiteSpace(connectionString))
			{
				string pwd = null;
				if (Provider == DatabaseProviderType.SqlServer)
				{
					pwd = "Password={System.Environment.GetEnvironmentVariable(\"MSSQLPASSWORD\")}";
				}
				if (Provider == DatabaseProviderType.PostgreSQL)
				{
					pwd = "Password={System.Environment.GetEnvironmentVariable(\"PGPASSWORD\")}";
				}
				connectionString = connectionString.Replace("Password=xxx", $"{pwd}");
			}
		}
		else
		{
			connectionString = GetConnectionStringBuilder()?.ConnectionString;
		}

		return connectionString;
	}
}
