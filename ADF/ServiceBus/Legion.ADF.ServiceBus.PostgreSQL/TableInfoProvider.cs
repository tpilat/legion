using Legion.Extensions;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public class TableInfoProvider : Legion.ADF.ServiceBus.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "\"Host\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.Host.IdHost), typeof(Guid), "\"IdHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.Description), typeof(string), "\"Description\"", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.IsEnabled), typeof(bool), "\"IsEnabled\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.Configuration), typeof(string), "\"Configuration\"", "jsonb", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.RowVersion), typeof(Guid), "\"RowVersion\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostTableInfo()
		=> _HostTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostActivityTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "\"HostActivity\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.HostActivity.IdHostActivity), typeof(Guid), "\"IdHostActivity\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostActivity.IdHost), typeof(Guid), "\"IdHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostActivity.StartedUtc), typeof(DateTime), "\"StartedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostActivity.LastActivityUtc), typeof(DateTime), "\"LastActivityUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostActivity.StoppedUtc), typeof(DateTime?), "\"StoppedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.HostActivity.IsDistributedManagerAvailable), typeof(bool), "\"IsDistributedManagerAvailable\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostActivity.RowVersion), typeof(Guid), "\"RowVersion\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostActivityTableInfo()
		=> _HostActivityTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "\"HostLog\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdHostLog), typeof(Guid), "\"IdHostLog\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdHost), typeof(Guid), "\"IdHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IsRunning), typeof(bool), "\"IsRunning\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.Detail), typeof(string), "\"Detail\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostLogTableInfo()
		=> _HostLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"Job\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdJobRunType), typeof(Guid), "\"IdJobRunType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.DelayedStartInSeconds), typeof(int?), "\"DelayedStartInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdleTimeoutInSeconds), typeof(int?), "\"IdleTimeoutInSeconds\"", "integer", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.CronExpression), typeof(string), "\"CronExpression\"", "varchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.CronExpressionIncludeSeconds), typeof(bool), "\"CronExpressionIncludeSeconds\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdDefaultHost), typeof(Guid), "\"IdDefaultHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.RequestedToDisable), typeof(bool), "\"RequestedToDisable\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.TimeoutForProcessingInSeconds), typeof(int), "\"TimeoutForProcessingInSeconds\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.RowVersion), typeof(Guid), "\"RowVersion\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobTableInfo()
		=> _JobTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobActivityTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobActivity\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.IdJobActivity), typeof(Guid), "\"IdJobActivity\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.IdCurrentHost), typeof(Guid), "\"IdCurrentHost\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.AttachedToCurrentHostUtc), typeof(DateTime), "\"AttachedToCurrentHostUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.LastStatusChangedUtc), typeof(DateTime), "\"LastStatusChangedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.LastProcessingStartedUtc), typeof(DateTime?), "\"LastProcessingStartedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.LastProcessingFinishedUtc), typeof(DateTime?), "\"LastProcessingFinishedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.DelayedToUtc), typeof(DateTime?), "\"DelayedToUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobActivity.RowVersion), typeof(Guid), "\"RowVersion\"", "uuid", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobActivityTableInfo()
		=> _JobActivityTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobDataTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobData\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.IdJobData), typeof(Guid), "\"IdJobData\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.JobDataIdentifier), typeof(string), "\"JobDataIdentifier\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.LastModifiedUtc), typeof(DateTime?), "\"LastModifiedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobDataTableInfo()
		=> _JobDataTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobExecutionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobExecution\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.IdJobExecution), typeof(Guid), "\"IdJobExecution\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.StartUtc), typeof(DateTime), "\"StartUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.EndUtc), typeof(DateTime?), "\"EndUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.StatisticsStartHourUtc), typeof(DateTime), "\"StatisticsStartHourUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobExecutionTableInfo()
		=> _JobExecutionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobLog\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdJobLog), typeof(Guid), "\"IdJobLog\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdMessageProcessingLog), typeof(Guid?), "\"IdMessageProcessingLog\"", "uuid", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdJobExecution), typeof(Guid?), "\"IdJobExecution\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobLogTableInfo()
		=> _JobLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobMessage\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdJobMessage), typeof(Guid), "\"IdJobMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdJobMessageType), typeof(Guid), "\"IdJobMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobMessageTableInfo()
		=> _JobMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobMessageType\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessageType.IdJobMessageType), typeof(Guid), "\"IdJobMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessageType.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessageType.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobMessageTypeTableInfo()
		=> _JobMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobRunTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobRunType\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobRunType.IdJobRunType), typeof(Guid), "\"IdJobRunType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobRunType.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobRunType.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobRunTypeTableInfo()
		=> _JobRunTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobStatisticsTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobStatistics\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.IdJobStatistics), typeof(Guid), "\"IdJobStatistics\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.IdJob), typeof(Guid), "\"IdJob\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.StartHourUtc), typeof(DateTime), "\"StartHourUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.ExecutionCount), typeof(int), "\"ExecutionCount\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.ErrorCount), typeof(int), "\"ErrorCount\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.DurationSumInSeconds), typeof(long), "\"DurationSumInSeconds\"", "bigint", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobStatisticsTableInfo()
		=> _JobStatisticsTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "\"JobStatus\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatus.IdJobStatus), typeof(Guid), "\"IdJobStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatus.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobStatusTableInfo()
		=> _JobStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"Orchestration\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.IsSingleton), typeof(bool), "\"IsSingleton\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Version), typeof(string), "\"Version\"", "varchar(31)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Properties), typeof(string), "\"Properties\"", "jsonb", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationTableInfo()
		=> _OrchestrationTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationInstanceTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationInstance\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.IdOrchestrationInstance), typeof(Guid), "\"IdOrchestrationInstance\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.IdOrchestrationStatus), typeof(Guid), "\"IdOrchestrationStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationInstanceTableInfo()
		=> _OrchestrationInstanceTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStatus\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus.IdOrchestrationStatus), typeof(Guid), "\"IdOrchestrationStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStatusTableInfo()
		=> _OrchestrationStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStep\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.IdOrchestrationStep), typeof(Guid), "\"IdOrchestrationStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.IsMainEntry), typeof(bool), "\"IsMainEntry\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Order), typeof(int), "\"Order\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.TimeoutForMessageProcessingInSeconds), typeof(int), "\"TimeoutForMessageProcessingInSeconds\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepTableInfo()
		=> _OrchestrationStepTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessing\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationStepProcessing), typeof(Guid), "\"IdOrchestrationStepProcessing\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationStep), typeof(Guid), "\"IdOrchestrationStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationInstance), typeof(Guid), "\"IdOrchestrationInstance\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationStepProcessingStatus), typeof(Guid), "\"IdOrchestrationStepProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingTableInfo()
		=> _OrchestrationStepProcessingTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingDirectionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingDirection\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.IdOrchestrationStepProcessingDirection), typeof(Guid), "\"IdOrchestrationStepProcessingDirection\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.IdFromStep), typeof(Guid), "\"IdFromStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.IdToStep), typeof(Guid), "\"IdToStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingDirectionTableInfo()
		=> _OrchestrationStepProcessingDirectionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingLog\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessingLog), typeof(Guid), "\"IdOrchestrationStepProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessing), typeof(Guid), "\"IdOrchestrationStepProcessing\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessingStatus), typeof(Guid), "\"IdOrchestrationStepProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdMessageProcessingLog), typeof(Guid?), "\"IdMessageProcessingLog\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingLogTableInfo()
		=> _OrchestrationStepProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingMessage\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessingMessage), typeof(Guid), "\"IdOrchestrationStepProcessingMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessing), typeof(Guid), "\"IdOrchestrationStepProcessing\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessingMessageType), typeof(Guid), "\"IdOrchestrationStepProcessingMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingMessageTableInfo()
		=> _OrchestrationStepProcessingMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingMessageType\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType.IdOrchestrationStepProcessingMessageType), typeof(Guid), "\"IdOrchestrationStepProcessingMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingMessageTypeTableInfo()
		=> _OrchestrationStepProcessingMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingStatus\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus.IdOrchestrationStepProcessingStatus), typeof(Guid), "\"IdOrchestrationStepProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingStatusTableInfo()
		=> _OrchestrationStepProcessingStatusTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Model.Host), GetHostTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.HostActivity), GetHostActivityTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.HostLog), GetHostLogTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.Job), GetJobTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobActivity), GetJobActivityTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobData), GetJobDataTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobExecution), GetJobExecutionTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobLog), GetJobLogTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobMessage), GetJobMessageTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobMessageType), GetJobMessageTypeTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobRunType), GetJobRunTypeTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobStatistics), GetJobStatisticsTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.JobStatus), GetJobStatusTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.Orchestration), GetOrchestrationTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationInstance), GetOrchestrationInstanceTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStatus), GetOrchestrationStatusTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStep), GetOrchestrationStepTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing), GetOrchestrationStepProcessingTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection), GetOrchestrationStepProcessingDirectionTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog), GetOrchestrationStepProcessingLogTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage), GetOrchestrationStepProcessingMessageTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType), GetOrchestrationStepProcessingMessageTypeTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus), GetOrchestrationStepProcessingStatusTableInfo() },
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
