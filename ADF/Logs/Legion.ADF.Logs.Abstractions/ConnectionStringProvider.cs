using Legion.ADF.Logs.Exceptions.Internal;
using Legion.ADF.Logs.Settings;
using Legion.Database;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Logs;

public class ConnectionStringProvider : IConnectionStringProvider
{
	private readonly DBSettings _dBSettings;

	public ConnectionStringProvider(IOptions<DBSettings> dBSettings)
	{
		Throw.IfArgumentNull(dBSettings);
		Throw.IfArgumentNull(dBSettings.Value);
		Throw.IfNullOrEmpty(dBSettings.Value.DbConnectionSettings);

		_dBSettings = dBSettings.Value;
	}

	public string GetDefaultConncetionString()
		=> _dBSettings.DbConnectionSettings.FirstOrDefault().Value?.ConnectionString!;

	public string GetConncetionString(string storeId)
	{
		if (string.IsNullOrWhiteSpace(storeId))
			return GetDefaultConncetionString();

		if (_dBSettings.DbConnectionSettings.TryGetValue(storeId, out var dbConnectionSettings)
			&& !string.IsNullOrWhiteSpace(dbConnectionSettings.ConnectionString))
			return dbConnectionSettings.ConnectionString;

		Throw.OutOfRangeException(storeId, ErrorCodes.ConnectionStringProviderException.InvalidStoreId(storeId));
		return null!;
	}

	public string GetConncetionString(IInvocationContext invocationContext)
		=> GetConncetionString(invocationContext.TargetStoreId!);
}
