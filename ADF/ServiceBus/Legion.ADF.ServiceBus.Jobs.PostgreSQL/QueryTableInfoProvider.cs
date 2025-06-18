using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.ServiceBus.Jobs.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwJobTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"VwJob\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdJobRunType), typeof(Guid), "\"IdJobRunType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.JobRunType), typeof(string), "\"JobRunType\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.JobStatus), typeof(string), "\"JobStatus\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.DelayedStartInSeconds), typeof(int?), "\"DelayedStartInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.IdleTimeoutInSeconds), typeof(int?), "\"IdleTimeoutInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.CronExpression), typeof(string), "\"CronExpression\"", "varchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.CronExpressionIncludeSeconds), typeof(bool), "\"CronExpressionIncludeSeconds\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.NextProcessinUtc), typeof(DateTime), "\"NextProcessinUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.TimeoutForProcessingInSeconds), typeof(int), "\"TimeoutForProcessingInSeconds\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.VwJob.MaxProcessingRetryCount), typeof(int), "\"MaxProcessingRetryCount\"", "integer", false),
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
