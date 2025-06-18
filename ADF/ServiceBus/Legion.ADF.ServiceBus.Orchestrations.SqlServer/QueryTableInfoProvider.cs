using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Orchestrations.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.Orchestrations.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOrchestrationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[VwOrchestration]",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.IdOrchestration), typeof(Guid), "[IdOrchestration]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Description), typeof(string), "[Description]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.IsSingleton), typeof(bool), "[IsSingleton]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Version), typeof(string), "[Version]", "nvarchar(31)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOrchestrationTableInfo()
		=> _VwOrchestrationTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration), GetVwOrchestrationTableInfo() },
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
