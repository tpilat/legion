using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Hosts.PostgreSQL;

public class TableInfoProvider : Legion.ADF.ServiceBus.Hosts.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "\"Host\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.IdHost), typeof(Guid), "\"IdHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.Description), typeof(string), "\"Description\"", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.IsEnabled), typeof(bool), "\"IsEnabled\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.StartedUtc), typeof(DateTime?), "\"StartedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.LastActivityUtc), typeof(DateTime), "\"LastActivityUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.StoppedUtc), typeof(DateTime?), "\"StoppedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.Host.Configuration), typeof(string), "\"Configuration\"", "jsonb", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostTableInfo()
		=> _HostTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "\"HostLog\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdHostLog), typeof(Guid), "\"IdHostLog\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdHost), typeof(Guid), "\"IdHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IsRunning), typeof(bool), "\"IsRunning\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog.Detail), typeof(string), "\"Detail\"", "text", true),
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
