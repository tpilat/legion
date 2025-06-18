using Legion.Extensions;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public class TableInfoProvider : Legion.ADF.ServiceBus.Orchestrations.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"Orchestration\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration.IsSingleton), typeof(bool), "\"IsSingleton\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration.Version), typeof(string), "\"Version\"", "varchar(31)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration.Properties), typeof(string), "\"Properties\"", "jsonb", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationTableInfo()
		=> _OrchestrationTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationInstanceTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationInstance\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance.IdOrchestrationInstance), typeof(Guid), "\"IdOrchestrationInstance\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance.IdOrchestrationStatus), typeof(Guid), "\"IdOrchestrationStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationInstanceTableInfo()
		=> _OrchestrationInstanceTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStatus\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus.IdOrchestrationStatus), typeof(Guid), "\"IdOrchestrationStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStatusTableInfo()
		=> _OrchestrationStatusTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStep\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.IdOrchestrationStep), typeof(Guid), "\"IdOrchestrationStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.IdOrchestration), typeof(Guid), "\"IdOrchestration\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.IsMainEntry), typeof(bool), "\"IsMainEntry\"", "boolean", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.Order), typeof(int), "\"Order\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.Name), typeof(string), "\"Name\"", "varchar(255)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.Description), typeof(string), "\"Description\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.TimeoutForMessageProcessingInSeconds), typeof(int), "\"TimeoutForMessageProcessingInSeconds\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.MaxMessageProcessingRetryCount), typeof(int), "\"MaxMessageProcessingRetryCount\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepTableInfo()
		=> _OrchestrationStepTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessing\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.IdOrchestrationStepProcessing), typeof(Guid), "\"IdOrchestrationStepProcessing\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.IdOrchestrationStep), typeof(Guid), "\"IdOrchestrationStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.IdOrchestrationInstance), typeof(Guid), "\"IdOrchestrationInstance\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.IdOrchestrationStepProcessingStatus), typeof(Guid), "\"IdOrchestrationStepProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingTableInfo()
		=> _OrchestrationStepProcessingTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingDirectionTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingDirection\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection.IdOrchestrationStepProcessingDirection), typeof(Guid), "\"IdOrchestrationStepProcessingDirection\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection.IdFromStep), typeof(Guid), "\"IdFromStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection.IdToStep), typeof(Guid), "\"IdToStep\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingDirectionTableInfo()
		=> _OrchestrationStepProcessingDirectionTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingLog\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessingLog), typeof(Guid), "\"IdOrchestrationStepProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessing), typeof(Guid), "\"IdOrchestrationStepProcessing\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.IdLogLevel), typeof(int), "\"IdLogLevel\"", "integer", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessingStatus), typeof(Guid), "\"IdOrchestrationStepProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.IdMessageProcessingLog), typeof(Guid?), "\"IdMessageProcessingLog\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingLogTableInfo()
		=> _OrchestrationStepProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingMessageTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingMessage\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessingMessage), typeof(Guid), "\"IdOrchestrationStepProcessingMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessing), typeof(Guid), "\"IdOrchestrationStepProcessing\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage.IdMessage), typeof(Guid), "\"IdMessage\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessingMessageType), typeof(Guid), "\"IdOrchestrationStepProcessingMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingMessageTableInfo()
		=> _OrchestrationStepProcessingMessageTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingMessageTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingMessageType\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType.IdOrchestrationStepProcessingMessageType), typeof(Guid), "\"IdOrchestrationStepProcessingMessageType\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType.Name), typeof(string), "\"Name\"", "varchar(63)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingMessageTypeTableInfo()
		=> _OrchestrationStepProcessingMessageTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _OrchestrationStepProcessingStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"orch", "\"OrchestrationStepProcessingStatus\"",
				[
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus.IdOrchestrationStepProcessingStatus), typeof(Guid), "\"IdOrchestrationStepProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetOrchestrationStepProcessingStatusTableInfo()
		=> _OrchestrationStepProcessingStatusTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration), GetOrchestrationTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance), GetOrchestrationInstanceTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus), GetOrchestrationStatusTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep), GetOrchestrationStepTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing), GetOrchestrationStepProcessingTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection), GetOrchestrationStepProcessingDirectionTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog), GetOrchestrationStepProcessingLogTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage), GetOrchestrationStepProcessingMessageTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType), GetOrchestrationStepProcessingMessageTypeTableInfo() },
			{ typeof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus), GetOrchestrationStepProcessingStatusTableInfo() },
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
