using Legion.Extensions;

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.Messaging.DomainEvents.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwDomainEventTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"devt", "\"VwDomainEvent\"",
				[
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.IdDomainEvent), typeof(Guid), "\"IdDomainEvent\"", "uuid", false),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.IdContent), typeof(Guid?), "\"IdContent\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.IdDomainEventProcessingStatus), typeof(Guid?), "\"IdDomainEventProcessingStatus\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.Namespace), typeof(string), "\"Namespace\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.TraceCorrelationId), typeof(Guid?), "\"TraceCorrelationId\"", "uuid", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.Properties), typeof(string), "\"Properties\"", "jsonb", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.Publisher), typeof(string), "\"Publisher\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.PublisherId), typeof(string), "\"PublisherId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.CreatedUtc), typeof(DateTime?), "\"CreatedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.ProcessedUtc), typeof(DateTime?), "\"ProcessedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.SuspendedUtc), typeof(DateTime?), "\"SuspendedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.LastProcessingUtc), typeof(DateTime?), "\"LastProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.LastProcessingTimeoutUtc), typeof(DateTime?), "\"LastProcessingTimeoutUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.NextProcessingUtc), typeof(DateTime?), "\"NextProcessingUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.RetryCount), typeof(int?), "\"RetryCount\"", "integer", true),
					new(nameof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent.Priority), typeof(int?), "\"Priority\"", "integer", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwDomainEventTableInfo()
		=> _VwDomainEventTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent), GetVwDomainEventTableInfo() },
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
