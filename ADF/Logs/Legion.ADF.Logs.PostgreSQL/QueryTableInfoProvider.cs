using Legion.Extensions;

namespace Legion.ADF.Logs.PostgreSQL;

public class QueryTableInfoProvider : Legion.ADF.Logs.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "\"VwLog\"",
				[
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdLog), typeof(Guid), "\"IdLog\"", "uuid", false),
					new(nameof(Legion.ADF.Logs.Model.VwLog.CreatedUtc), typeof(DateTime?), "\"CreatedUtc\"", "timestamp with time zone", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.InternalMessage), typeof(string), "\"InternalMessage\"", "text", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ClientMessage), typeof(string), "\"ClientMessage\"", "text", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.Detail), typeof(string), "\"Detail\"", "text", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.StackTrace), typeof(string), "\"StackTrace\"", "text", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.Component), typeof(string), "\"Component\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.OperationName), typeof(string), "\"OperationName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.AggregateName), typeof(string), "\"AggregateName\"", "varchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.AggregateIdentifier), typeof(string), "\"AggregateIdentifier\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.CustomCorrelationId), typeof(string), "\"CustomCorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdApplicationEntry), typeof(Guid?), "\"IdApplicationEntry\"", "uuid", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.CorrelationId), typeof(Guid?), "\"CorrelationId\"", "uuid", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ExternalCorrelationId), typeof(string), "\"ExternalCorrelationId\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ContextProperties), typeof(string), "\"ContextProperties\"", "jsonb", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdUser), typeof(Guid?), "\"IdUser\"", "uuid", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.TenantIdentifier), typeof(Guid?), "\"TenantIdentifier\"", "uuid", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdLogLevel), typeof(int?), "\"IdLogLevel\"", "integer", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.LogCode), typeof(string), "\"LogCode\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.SourceSystemName), typeof(string), "\"SourceSystemName\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.TraceCorrelationId), typeof(Guid?), "\"TraceCorrelationId\"", "uuid", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.TraceFrame), typeof(string), "\"TraceFrame\"", "text", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.SourceContext), typeof(string), "\"SourceContext\"", "text", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.RuntimeUniqueKey), typeof(Guid?), "\"RuntimeUniqueKey\"", "uuid", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IsValidationError), typeof(bool?), "\"IsValidationError\"", "boolean", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.PropertyName), typeof(string), "\"PropertyName\"", "varchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.DisplayPropertyName), typeof(string), "\"DisplayPropertyName\"", "varchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ValidationFailure), typeof(string), "\"ValidationFailure\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwLogTableInfo()
		=> _VwLogTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Logs.Model.VwLog), GetVwLogTableInfo() },
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
