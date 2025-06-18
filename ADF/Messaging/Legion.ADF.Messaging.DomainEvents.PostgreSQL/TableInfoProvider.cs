using Legion.Extensions;

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public class TableInfoProvider : Legion.ADF.Messaging.DomainEvents.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _BlockedDomainEventTypeTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "\"BlockedDomainEventType\"",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType.IdBlockedDomainEventType), typeof(Guid), "\"IdBlockedDomainEventType\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetBlockedDomainEventTypeTableInfo()
		=> _BlockedDomainEventTypeTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "\"DomainEvent\"",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.IdDomainEvent), typeof(Guid), "\"IdDomainEvent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.IdContent), typeof(Guid), "\"IdContent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.IdDomainEventProcessingStatus), typeof(Guid), "\"IdDomainEventProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.NextProcessingUtc), typeof(DateTime), "\"NextProcessingUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.RetryCount), typeof(int), "\"RetryCount\"", "integer", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent.Priority), typeof(int), "\"Priority\"", "integer", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDomainEventTableInfo()
		=> _DomainEventTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventContentTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "\"DomainEventContent\"",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent.IdDomainEventContent), typeof(Guid), "\"IdDomainEventContent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent.Content), typeof(string), "\"Content\"", "jsonb", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDomainEventContentTableInfo()
		=> _DomainEventContentTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventProcessingLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "\"DomainEventProcessingLog\"",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdDomainEventProcessingLog), typeof(Guid), "\"IdDomainEventProcessingLog\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdDomainEvent), typeof(Guid), "\"IdDomainEvent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdDomainEventProcessingStatus), typeof(Guid), "\"IdDomainEventProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.TraceCorrelationId), typeof(Guid), "\"TraceCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.IdLogMessage), typeof(Guid?), "\"IdLogMessage\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.Code), typeof(string), "\"Code\"", "varchar(127)", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog.Detail), typeof(string), "\"Detail\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetDomainEventProcessingLogTableInfo()
		=> _DomainEventProcessingLogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _DomainEventProcessingStatusTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "\"DomainEventProcessingStatus\"",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus.IdDomainEventProcessingStatus), typeof(Guid), "\"IdDomainEventProcessingStatus\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus.Code), typeof(string), "\"Code\"", "varchar(63)", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus.Name), typeof(string), "\"Name\"", "varchar(127)", false),
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
