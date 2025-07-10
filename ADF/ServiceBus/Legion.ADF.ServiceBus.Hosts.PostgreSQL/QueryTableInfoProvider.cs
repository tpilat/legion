using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Hosts.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.Hosts.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwHostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "\"VwHost\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.IdHost), typeof(Guid), "\"IdHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.Description), typeof(string), "\"Description\"", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.IsEnabled), typeof(bool), "\"IsEnabled\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.StartedUtc), typeof(DateTime?), "\"StartedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.LastActivityUtc), typeof(DateTime), "\"LastActivityUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.StoppedUtc), typeof(DateTime?), "\"StoppedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.Configuration), typeof(string), "\"Configuration\"", "jsonb", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwHostTableInfo()
		=> _VwHostTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Hosts.Model.VwHost), GetVwHostTableInfo() },
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
