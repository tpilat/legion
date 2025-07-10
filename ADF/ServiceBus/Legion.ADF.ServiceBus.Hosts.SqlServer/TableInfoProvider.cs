using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Hosts.SqlServer;

public class TableInfoProvider : Legion.ADF.ServiceBus.Hosts.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "[Host]",
				[
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.IdHost), typeof(Guid), "[IdHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.Name), typeof(string), "[Name]", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.Description), typeof(string), "[Description]", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.IsEnabled), typeof(bool), "[IsEnabled]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.StartedUtc), typeof(DateTime?), "[StartedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.LastActivityUtc), typeof(DateTime), "[LastActivityUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.StoppedUtc), typeof(DateTime?), "[StoppedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.Configuration), typeof(string), "[Configuration]", "nvarchar(max)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostTableInfo()
		=> _HostTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "[HostLog]",
				[
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdHostLog), typeof(Guid), "[IdHostLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdHost), typeof(Guid), "[IdHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IsRunning), typeof(bool), "[IsRunning]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostLogTableInfo()
		=> _HostLogTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Hosts.Model.Host), GetHostTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Hosts.Model.HostLog), GetHostLogTableInfo() },
		});

	public IReadOnlyDictionary<Type, Legion.Database.Metamodel.Info.TableInfo> TableInfoDictionary => _tableInfoDictionary.Value;

	public Legion.Database.Metamodel.Info.TableInfo GetTableInfo<T>()
		=> GetTableInfo(typeof(T));

	public Legion.Database.Metamodel.Info.TableInfo GetTableInfo(Type type)
	{
		if (TableInfoDictionary.TryGetValue(type, out var tableInfo))
			return tableInfo;

		Legion.Throw.InvalidOperationException($"Invalid entity type = {type.ToFriendlyFullName()}");
		return null;
	}
}
