using Legion.Extensions;

namespace Legion.ADF.ServiceBus.SqlServer;

public class TableInfoProvider : Legion.ADF.ServiceBus.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "[Host]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.Host.IdHost), typeof(Guid), "[IdHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.Name), typeof(string), "[Name]", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.Description), typeof(string), "[Description]", "varchar(511)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.IsEnabled), typeof(bool), "[IsEnabled]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.StartedUtc), typeof(DateTime?), "[StartedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.LastActivityUtc), typeof(DateTime), "[LastActivityUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.StoppedUtc), typeof(DateTime?), "[StoppedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.Configuration), typeof(string), "[Configuration]", "nvarchar(max)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Host.IsDistributedManagerAvailable), typeof(bool), "[IsDistributedManagerAvailable]", "bit", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostTableInfo()
		=> _HostTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _HostLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"hosts", "[HostLog]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdHostLog), typeof(Guid), "[IdHostLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdHost), typeof(Guid), "[IdHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IsRunning), typeof(bool), "[IsRunning]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.HostLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetHostLogTableInfo()
		=> _HostLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[Job]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Description), typeof(string), "[Description]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdJobRunType), typeof(Guid), "[IdJobRunType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdJobStatus), typeof(Guid), "[IdJobStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.DelayedStartInSeconds), typeof(int?), "[DelayedStartInSeconds]", "int", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdleTimeoutInSeconds), typeof(int?), "[IdleTimeoutInSeconds]", "int", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.CronExpression), typeof(string), "[CronExpression]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.CronExpressionIncludeSeconds), typeof(bool), "[CronExpressionIncludeSeconds]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdDefaultHost), typeof(Guid), "[IdDefaultHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.IdCurrentHost), typeof(Guid), "[IdCurrentHost]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.AttachedToCurrentHostUtc), typeof(DateTime), "[AttachedToCurrentHostUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.LastProcessingFinishedUtc), typeof(DateTime?), "[LastProcessingFinishedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.NextProcessinUtc), typeof(DateTime), "[NextProcessinUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.TimeoutForProcessingInSeconds), typeof(int), "[TimeoutForProcessingInSeconds]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Job.MaxProcessingRetryCount), typeof(int), "[MaxProcessingRetryCount]", "int", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobTableInfo()
		=> _JobTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobDataTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobData]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.IdJobData), typeof(Guid), "[IdJobData]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.JobDataIdentifier), typeof(string), "[JobDataIdentifier]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.LastModifiedUtc), typeof(DateTime?), "[LastModifiedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.MimeType), typeof(string), "[MimeType]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.Name), typeof(string), "[Name]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.IsCompressed), typeof(bool), "[IsCompressed]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobData.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobDataTableInfo()
		=> _JobDataTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobExecutionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobExecution]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.IdJobExecution), typeof(Guid), "[IdJobExecution]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.StartUtc), typeof(DateTime), "[StartUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.EndUtc), typeof(DateTime?), "[EndUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobExecution.IdJobStatus), typeof(Guid), "[IdJobStatus]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobExecutionTableInfo()
		=> _JobExecutionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobLog]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdJobLog), typeof(Guid), "[IdJobLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdJobStatus), typeof(Guid), "[IdJobStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.JobLog.IdMessageProcessingLog), typeof(Guid?), "[IdMessageProcessingLog]", "uniqueidentifier", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobLogTableInfo()
		=> _JobLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobMessage]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdJobMessage), typeof(Guid), "[IdJobMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdMessage), typeof(Guid), "[IdMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.IdJobMessageType), typeof(Guid), "[IdJobMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessage.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobMessageTableInfo()
		=> _JobMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobMessageType]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessageType.IdJobMessageType), typeof(Guid), "[IdJobMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessageType.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobMessageType.Name), typeof(string), "[Name]", "nvarchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobMessageTypeTableInfo()
		=> _JobMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobRunTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobRunType]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobRunType.IdJobRunType), typeof(Guid), "[IdJobRunType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobRunType.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobRunType.Name), typeof(string), "[Name]", "nvarchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobRunTypeTableInfo()
		=> _JobRunTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobStatisticsTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobStatistics]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.IdJobStatistics), typeof(Guid), "[IdJobStatistics]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.IdJob), typeof(Guid), "[IdJob]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.StartHourUtc), typeof(DateTime), "[StartHourUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.ExecutionCount), typeof(int), "[ExecutionCount]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.ErrorCount), typeof(int), "[ErrorCount]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatistics.AverageDuration), typeof(decimal), "[AverageDuration]", "decimal", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobStatisticsTableInfo()
		=> _JobStatisticsTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _JobStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"jobs", "[JobStatus]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatus.IdJobStatus), typeof(Guid), "[IdJobStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatus.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.JobStatus.Name), typeof(string), "[Name]", "nvarchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetJobStatusTableInfo()
		=> _JobStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[Orchestration]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.IdOrchestration), typeof(Guid), "[IdOrchestration]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Description), typeof(string), "[Description]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.IsSingleton), typeof(bool), "[IsSingleton]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Version), typeof(string), "[Version]", "nvarchar(31)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.Orchestration.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationTableInfo()
		=> _OrchestrationTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationInstanceTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationInstance]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.IdOrchestrationInstance), typeof(Guid), "[IdOrchestrationInstance]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.IdOrchestration), typeof(Guid), "[IdOrchestration]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.IdOrchestrationStatus), typeof(Guid), "[IdOrchestrationStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationInstanceTableInfo()
		=> _OrchestrationInstanceTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStatus]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus.IdOrchestrationStatus), typeof(Guid), "[IdOrchestrationStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus.Name), typeof(string), "[Name]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStatusTableInfo()
		=> _OrchestrationStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStep]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.IdOrchestrationStep), typeof(Guid), "[IdOrchestrationStep]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.IdOrchestration), typeof(Guid), "[IdOrchestration]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.IsMainEntry), typeof(bool), "[IsMainEntry]", "bit", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Order), typeof(int), "[Order]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Name), typeof(string), "[Name]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Description), typeof(string), "[Description]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.TimeoutForMessageProcessingInSeconds), typeof(int), "[TimeoutForMessageProcessingInSeconds]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep.MaxMessageProcessingRetryCount), typeof(int), "[MaxMessageProcessingRetryCount]", "int", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepTableInfo()
		=> _OrchestrationStepTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStepProcessing]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationStepProcessing), typeof(Guid), "[IdOrchestrationStepProcessing]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationStep), typeof(Guid), "[IdOrchestrationStep]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationInstance), typeof(Guid), "[IdOrchestrationInstance]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationStepProcessingStatus), typeof(Guid), "[IdOrchestrationStepProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.RetryCount), typeof(int), "[RetryCount]", "int", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingTableInfo()
		=> _OrchestrationStepProcessingTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingDirectionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStepProcessingDirection]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.IdOrchestrationStepProcessingDirection), typeof(Guid), "[IdOrchestrationStepProcessingDirection]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.IdFromStep), typeof(Guid), "[IdFromStep]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.IdToStep), typeof(Guid), "[IdToStep]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingDirectionTableInfo()
		=> _OrchestrationStepProcessingDirectionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStepProcessingLog]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessingLog), typeof(Guid), "[IdOrchestrationStepProcessingLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessing), typeof(Guid), "[IdOrchestrationStepProcessing]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessingStatus), typeof(Guid), "[IdOrchestrationStepProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.IdMessageProcessingLog), typeof(Guid?), "[IdMessageProcessingLog]", "uniqueidentifier", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingLogTableInfo()
		=> _OrchestrationStepProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStepProcessingMessage]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessingMessage), typeof(Guid), "[IdOrchestrationStepProcessingMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessing), typeof(Guid), "[IdOrchestrationStepProcessing]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdMessage), typeof(Guid), "[IdMessage]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessingMessageType), typeof(Guid), "[IdOrchestrationStepProcessingMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingMessageTableInfo()
		=> _OrchestrationStepProcessingMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStepProcessingMessageType]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType.IdOrchestrationStepProcessingMessageType), typeof(Guid), "[IdOrchestrationStepProcessingMessageType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType.Name), typeof(string), "[Name]", "nvarchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingMessageTypeTableInfo()
		=> _OrchestrationStepProcessingMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "[OrchestrationStepProcessingStatus]",
				[
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus.IdOrchestrationStepProcessingStatus), typeof(Guid), "[IdOrchestrationStepProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus.Name), typeof(string), "[Name]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingStatusTableInfo()
		=> _OrchestrationStepProcessingStatusTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Model.Host), GetHostTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.HostLog), GetHostLogTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Model.Job), GetJobTableInfo() },
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
