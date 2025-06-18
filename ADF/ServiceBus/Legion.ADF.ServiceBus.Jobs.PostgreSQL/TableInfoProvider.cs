using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public class TableInfoProvider : Legion.ADF.ServiceBus.Jobs.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"Job\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.IdJobRunType), typeof(Guid), "\"IdJobRunType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.DelayedStartInSeconds), typeof(int?), "\"DelayedStartInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.IdleTimeoutInSeconds), typeof(int?), "\"IdleTimeoutInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.CronExpression), typeof(string), "\"CronExpression\"", "varchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.CronExpressionIncludeSeconds), typeof(bool), "\"CronExpressionIncludeSeconds\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.NextProcessinUtc), typeof(DateTime), "\"NextProcessinUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.TimeoutForProcessingInSeconds), typeof(int), "\"TimeoutForProcessingInSeconds\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.Job.MaxProcessingRetryCount), typeof(int), "\"MaxProcessingRetryCount\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobTableInfo()
		=> _JobTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobDataTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobData\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.IdJobData), typeof(Guid), "\"IdJobData\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.JobDataIdentifier), typeof(string), "\"JobDataIdentifier\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.LastModifiedUtc), typeof(DateTime?), "\"LastModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobDataTableInfo()
		=> _JobDataTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobExecutionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobExecution\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution.IdJobExecution), typeof(Guid), "\"IdJobExecution\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution.StartUtc), typeof(DateTime), "\"StartUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution.EndUtc), typeof(DateTime?), "\"EndUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobExecutionTableInfo()
		=> _JobExecutionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobLog\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.IdJobLog), typeof(Guid), "\"IdJobLog\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog.IdMessageProcessingLog), typeof(Guid?), "\"IdMessageProcessingLog\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobLogTableInfo()
		=> _JobLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobMessage\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage.IdJobMessage), typeof(Guid), "\"IdJobMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage.IdJobMessageType), typeof(Guid), "\"IdJobMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobMessageTableInfo()
		=> _JobMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobMessageType\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessageType.IdJobMessageType), typeof(Guid), "\"IdJobMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessageType.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessageType.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobMessageTypeTableInfo()
		=> _JobMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobRunTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobRunType\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobRunType.IdJobRunType), typeof(Guid), "\"IdJobRunType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobRunType.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobRunType.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobRunTypeTableInfo()
		=> _JobRunTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobStatisticsTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobStatistics\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics.IdJobStatistics), typeof(Guid), "\"IdJobStatistics\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics.StartHourUtc), typeof(DateTime), "\"StartHourUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics.ExecutionCount), typeof(int), "\"ExecutionCount\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics.ErrorCount), typeof(int), "\"ErrorCount\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics.AverageDuration), typeof(decimal), "\"AverageDuration\"", "numeric", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobStatisticsTableInfo()
		=> _JobStatisticsTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobStatus\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatus.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatus.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobStatusTableInfo()
		=> _JobStatusTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.Job), GetJobTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobData), GetJobDataTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution), GetJobExecutionTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobLog), GetJobLogTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage), GetJobMessageTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobMessageType), GetJobMessageTypeTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobRunType), GetJobRunTypeTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics), GetJobStatisticsTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Jobs.Model.JobStatus), GetJobStatusTableInfo() },
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
