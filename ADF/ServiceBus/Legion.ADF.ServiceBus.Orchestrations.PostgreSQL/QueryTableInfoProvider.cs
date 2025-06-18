using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.Orchestrations.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOrchestrationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"VwOrchestration\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Name), typeof(string), "\"Name\"", "varchar(255)", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.IsSingleton), typeof(bool?), "\"IsSingleton\"", "boolean", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Version), typeof(string), "\"Version\"", "varchar(31)", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration.Properties), typeof(string), "\"Properties\"", "jsonb", true),
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
