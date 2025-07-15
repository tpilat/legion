using Legion.Extensions;

namespace Legion.ADF.ServiceBus.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwHostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "[VwHost]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.IdHost), typeof(Guid), "[IdHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.Name), typeof(string), "[Name]", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.Description), typeof(string), "[Description]", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.IsEnabled), typeof(bool), "[IsEnabled]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.Configuration), typeof(string), "[Configuration]", "nvarchar(max)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwHost.RowVersion), typeof(Guid), "[RowVersion]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwHostTableInfo()
		=> _VwHostTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwJobTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[VwJob]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Description), typeof(string), "[Description]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdJobRunType), typeof(Guid), "[IdJobRunType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.DelayedStartInSeconds), typeof(int?), "[DelayedStartInSeconds]", "int", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdleTimeoutInSeconds), typeof(int?), "[IdleTimeoutInSeconds]", "int", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.CronExpression), typeof(string), "[CronExpression]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.CronExpressionIncludeSeconds), typeof(bool), "[CronExpressionIncludeSeconds]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.IdDefaultHost), typeof(Guid), "[IdDefaultHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.RequestedToDisable), typeof(bool), "[RequestedToDisable]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.TimeoutForProcessingInSeconds), typeof(int), "[TimeoutForProcessingInSeconds]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwJob.RowVersion), typeof(Guid), "[RowVersion]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwJobTableInfo()
		=> _VwJobTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwOrchestrationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[VwOrchestration]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.IdOrchestration), typeof(Guid), "[IdOrchestration]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Description), typeof(string), "[Description]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.IsSingleton), typeof(bool), "[IsSingleton]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Version), typeof(string), "[Version]", "nvarchar(31)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.VwOrchestration.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
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
