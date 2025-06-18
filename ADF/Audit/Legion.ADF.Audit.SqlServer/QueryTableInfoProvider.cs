using Legion.Extensions;

namespace Legion.ADF.Audit.SqlServer;

public class QueryTableInfoProvider : Legion.ADF.Audit.IQueryTableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwApplicationEntryTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "[VwApplicationEntry]",
				[
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.IdApplicationEntry), typeof(Guid), "[IdApplicationEntry]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.IdApplicationEntryToken), typeof(Guid), "[IdApplicationEntryToken]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.Token), typeof(string), "[Token]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.SourceFilePath), typeof(string), "[SourceFilePath]", "nvarchar(511)", false),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.MethodInfo), typeof(string), "[MethodInfo]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.AggregateName), typeof(string), "[AggregateName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.AggregateIdentifier), typeof(string), "[AggregateIdentifier]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.Description), typeof(string), "[Description]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.IdAuditOperation), typeof(Guid), "[IdAuditOperation]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.CorrelationId), typeof(Guid?), "[CorrelationId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.ExternalCorrelationId), typeof(string), "[ExternalCorrelationId]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.HttpMethod), typeof(string), "[HttpMethod]", "nvarchar(15)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.Uri), typeof(string), "[Uri]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.IdUser), typeof(Guid?), "[IdUser]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.TenantIdentifier), typeof(Guid?), "[TenantIdentifier]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.RemoteIP), typeof(string), "[RemoteIP]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.IdApplicationEntryRequest), typeof(Guid?), "[IdApplicationEntryRequest]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.IdApplicationEntryResponse), typeof(Guid?), "[IdApplicationEntryResponse]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.StatusCode), typeof(string), "[StatusCode]", "varchar(63)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.Error), typeof(string), "[Error]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Audit.Model.VwApplicationEntry.ElapsedMilliseconds), typeof(decimal?), "[ElapsedMilliseconds]", "numeric", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwApplicationEntryTableInfo()
		=> _VwApplicationEntryTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _VwAuditEntryTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "[VwAuditEntry]",
				[
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.IdAuditEntry), typeof(Guid), "[IdAuditEntry]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.IdAuditOperation), typeof(Guid), "[IdAuditOperation]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.TableName), typeof(string), "[TableName]", "nvarchar(255)", false),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.IdUser), typeof(Guid?), "[IdUser]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.PrimaryKey), typeof(string), "[PrimaryKey]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.OldValues), typeof(string), "[OldValues]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.NewValues), typeof(string), "[NewValues]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.AffectedColumns), typeof(string), "[AffectedColumns]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.AuditCorrelationId), typeof(Guid), "[AuditCorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.TraceFrame), typeof(string), "[TraceFrame]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Audit.Model.VwAuditEntry.CorrelationId), typeof(Guid?), "[CorrelationId]", "uniqueidentifier", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetVwAuditEntryTableInfo()
		=> _VwAuditEntryTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Audit.Model.VwApplicationEntry), GetVwApplicationEntryTableInfo() },
			{ typeof(Legion.ADF.Audit.Model.VwAuditEntry), GetVwAuditEntryTableInfo() },
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
