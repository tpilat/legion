using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.Jobs.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwJobTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[VwJob]",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Description), typeof(string), "[Description]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdJobRunType), typeof(Guid), "[IdJobRunType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.JobRunType), typeof(string), "[JobRunType]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdJobStatus), typeof(Guid), "[IdJobStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.JobStatus), typeof(string), "[JobStatus]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.DelayedStartInSeconds), typeof(int?), "[DelayedStartInSeconds]", "int", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdleTimeoutInSeconds), typeof(int?), "[IdleTimeoutInSeconds]", "int", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.CronExpression), typeof(string), "[CronExpression]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.CronExpressionIncludeSeconds), typeof(bool), "[CronExpressionIncludeSeconds]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.NextProcessinUtc), typeof(DateTime), "[NextProcessinUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.TimeoutForProcessingInSeconds), typeof(int), "[TimeoutForProcessingInSeconds]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.MaxProcessingRetryCount), typeof(int), "[MaxProcessingRetryCount]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdDefaultHost), typeof(Guid), "[IdDefaultHost]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwJobTableInfo()
		=> _VwJobTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.VwJob), GetVwJobTableInfo() },
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
