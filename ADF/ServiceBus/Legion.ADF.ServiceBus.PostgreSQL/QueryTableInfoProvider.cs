using Legion.Extensions;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwHostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "\"VwHost\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.IdHost), typeof(Guid), "\"IdHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.Description), typeof(string), "\"Description\"", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.IsEnabled), typeof(bool), "\"IsEnabled\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.Configuration), typeof(string), "\"Configuration\"", "jsonb", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.RowVersion), typeof(Guid?), "\"RowVersion\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwHostTableInfo()
		=> _VwHostTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwJobTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"VwJob\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdJobRunType), typeof(Guid), "\"IdJobRunType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.DelayedStartInSeconds), typeof(int?), "\"DelayedStartInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdleTimeoutInSeconds), typeof(int?), "\"IdleTimeoutInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.CronExpression), typeof(string), "\"CronExpression\"", "varchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.CronExpressionIncludeSeconds), typeof(bool), "\"CronExpressionIncludeSeconds\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdDefaultHost), typeof(Guid), "\"IdDefaultHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.RequestedToDisable), typeof(bool), "\"RequestedToDisable\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.TimeoutForProcessingInSeconds), typeof(int), "\"TimeoutForProcessingInSeconds\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.RowVersion), typeof(Guid), "\"RowVersion\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwJobTableInfo()
		=> _VwJobTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOrchestrationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"VwOrchestration\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Name), typeof(string), "\"Name\"", "varchar(255)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.IsSingleton), typeof(bool?), "\"IsSingleton\"", "boolean", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Version), typeof(string), "\"Version\"", "varchar(31)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Properties), typeof(string), "\"Properties\"", "jsonb", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwOrchestrationTableInfo()
		=> _VwOrchestrationTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Model.VwHost), GetVwHostTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.VwJob), GetVwJobTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.VwOrchestration), GetVwOrchestrationTableInfo() },
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
