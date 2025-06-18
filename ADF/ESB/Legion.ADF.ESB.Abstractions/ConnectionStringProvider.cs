using Legion.ADF.ESB.Exceptions.Internal;
using Legion.ADF.ESB.Settings;
using Legion.Database;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ESB;

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

	public string GetConncetionString(IScopeContext scopeContext)
		=> GetConncetionString(scopeContext.TargetStoreId!);
}
