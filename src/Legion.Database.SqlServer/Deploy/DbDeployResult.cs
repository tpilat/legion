using static Legion.Database.SqlServer.Deploy.DbDeploySettings;

namespace Legion.Database.SqlServer.Deploy;

public class DbDeployResult
{
	public bool IsSuccess { get; set; }
	public string? FilePath { get; set; }
	public string? ConnectionString { get; set; }
	public SqlFileSettings? SqlFileSettings { get; set; }
	public string? Error { get; set; }

	private DbDeployResult()
	{
	}

	public static DbDeployResult Success()
		=> new()
		{
			IsSuccess = true
		};

	public static DbDeployResult Failure(
		string? filePath,
		string? connectionString,
		SqlFileSettings sqlFileSettings,
		string error)
		=> new()
		{
			IsSuccess = false,
			FilePath = filePath,
			ConnectionString = connectionString,
			SqlFileSettings = sqlFileSettings,
			Error = error
		};
}
