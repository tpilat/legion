using Legion.Extensions;

namespace Legion.ADF.Logs.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.Logs.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[VwLog]",
				[
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdLog), typeof(Guid), "[IdLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.VwLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.VwLog.InternalMessage), typeof(string), "[InternalMessage]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ClientMessage), typeof(string), "[ClientMessage]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.StackTrace), typeof(string), "[StackTrace]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.Component), typeof(string), "[Component]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.OperationName), typeof(string), "[OperationName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.AggregateName), typeof(string), "[AggregateName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.AggregateIdentifier), typeof(string), "[AggregateIdentifier]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.CustomCorrelationId), typeof(string), "[CustomCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdApplicationEntry), typeof(Guid?), "[IdApplicationEntry]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.CorrelationId), typeof(Guid?), "[CorrelationId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ExternalCorrelationId), typeof(string), "[ExternalCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ContextProperties), typeof(string), "[ContextProperties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdUser), typeof(Guid?), "[IdUser]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.TenantIdentifier), typeof(Guid?), "[TenantIdentifier]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.Logs.Model.VwLog.LogCode), typeof(string), "[LogCode]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.SourceSystemName), typeof(string), "[SourceSystemName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.TraceCorrelationId), typeof(Guid?), "[TraceCorrelationId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.TraceFrame), typeof(string), "[TraceFrame]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.SourceContext), typeof(string), "[SourceContext]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.VwLog.IsValidationError), typeof(bool), "[IsValidationError]", "bit", false),
					new(nameof(Legion.ADF.Logs.Model.VwLog.PropertyName), typeof(string), "[PropertyName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.DisplayPropertyName), typeof(string), "[DisplayPropertyName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.VwLog.ValidationFailure), typeof(string), "[ValidationFailure]", "nvarchar(max)", true),
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
