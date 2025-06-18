using Legion.Extensions;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public class TableInfoProvider : Legion.ADF.Messaging.DomainEvents.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _BlockedDomainEventTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "[BlockedDomainEventType]",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType.IdBlockedDomainEventType), typeof(Guid), "[IdBlockedDomainEventType]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetBlockedDomainEventTypeTableInfo()
		=> _BlockedDomainEventTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "[DomainEvent]",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.IdDomainEvent), typeof(Guid), "[IdDomainEvent]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.IdContent), typeof(Guid), "[IdContent]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.IdDomainEventProcessingStatus), typeof(Guid), "[IdDomainEventProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Namespace), typeof(string), "[Namespace]", "nvarchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Properties), typeof(string), "[Properties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Publisher), typeof(string), "[Publisher]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.PublisherId), typeof(string), "[PublisherId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.ProcessedUtc), typeof(DateTime?), "[ProcessedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.SuspendedUtc), typeof(DateTime?), "[SuspendedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.LastProcessingUtc), typeof(DateTime?), "[LastProcessingUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.LastProcessingTimeoutUtc), typeof(DateTime?), "[LastProcessingTimeoutUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.NextProcessingUtc), typeof(DateTime), "[NextProcessingUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.RetryCount), typeof(int), "[RetryCount]", "int", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Priority), typeof(int), "[Priority]", "int", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDomainEventTableInfo()
		=> _DomainEventTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "[DomainEventContent]",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent.IdDomainEventContent), typeof(Guid), "[IdDomainEventContent]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent.Content), typeof(string), "[Content]", "nvarchar(max)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDomainEventContentTableInfo()
		=> _DomainEventContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "[DomainEventProcessingLog]",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdDomainEventProcessingLog), typeof(Guid), "[IdDomainEventProcessingLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdDomainEvent), typeof(Guid), "[IdDomainEvent]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdDomainEventProcessingStatus), typeof(Guid), "[IdDomainEventProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.TraceCorrelationId), typeof(Guid), "[TraceCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdLogMessage), typeof(Guid?), "[IdLogMessage]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDomainEventProcessingLogTableInfo()
		=> _DomainEventProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventProcessingStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "[DomainEventProcessingStatus]",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus.IdDomainEventProcessingStatus), typeof(Guid), "[IdDomainEventProcessingStatus]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus.Code), typeof(string), "[Code]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus.Name), typeof(string), "[Name]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDomainEventProcessingStatusTableInfo()
		=> _DomainEventProcessingStatusTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType), GetBlockedDomainEventTypeTableInfo() },
			{ typeof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent), GetDomainEventTableInfo() },
			{ typeof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent), GetDomainEventContentTableInfo() },
			{ typeof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog), GetDomainEventProcessingLogTableInfo() },
			{ typeof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus), GetDomainEventProcessingStatusTableInfo() },
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
