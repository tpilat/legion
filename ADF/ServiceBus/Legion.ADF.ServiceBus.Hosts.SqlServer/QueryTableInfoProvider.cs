using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Hosts.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.Hosts.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwHostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "[VwHost]",
				[
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.IdHost), typeof(Guid), "[IdHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.Name), typeof(string), "[Name]", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.Description), typeof(string), "[Description]", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.IsEnabled), typeof(bool), "[IsEnabled]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.StartedUtc), typeof(DateTime?), "[StartedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.LastActivityUtc), typeof(DateTime), "[LastActivityUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.StoppedUtc), typeof(DateTime?), "[StoppedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Hosts.Model.VwHost.Configuration), typeof(string), "[Configuration]", "nvarchar(max)", false),
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
