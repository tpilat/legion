using Legion.Extensions;

namespace Legion.ADF.Audit.PostgreSQL;

public class TableInfoProvider : Legion.ADF.Audit.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ApplicationEntryTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "\"ApplicationEntry\"",
				[
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.IdApplicationEntry), typeof(Guid), "\"IdApplicationEntry\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.IdApplicationEntryToken), typeof(Guid), "\"IdApplicationEntryToken\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.IdAuditOperation), typeof(Guid), "\"IdAuditOperation\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.RuntimeUniqueKey), typeof(Guid), "\"RuntimeUniqueKey\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.CorrelationId), typeof(Guid?), "\"CorrelationId\"", "uuid", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.ExternalCorrelationId), typeof(string), "\"ExternalCorrelationId\"", "varchar(127)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.AggregateIdentifier), typeof(string), "\"AggregateIdentifier\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.HttpMethod), typeof(string), "\"HttpMethod\"", "varchar(15)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.Uri), typeof(string), "\"Uri\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.IdUser), typeof(Guid?), "\"IdUser\"", "uuid", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.TenantIdentifier), typeof(Guid?), "\"TenantIdentifier\"", "uuid", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntry.RemoteIP), typeof(string), "\"RemoteIP\"", "varchar(63)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetApplicationEntryTableInfo()
		=> _ApplicationEntryTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ApplicationEntryRequestTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "\"ApplicationEntryRequest\"",
				[
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.IdApplicationEntryRequest), typeof(Guid), "\"IdApplicationEntryRequest\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.IdApplicationEntry), typeof(Guid), "\"IdApplicationEntry\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetApplicationEntryRequestTableInfo()
		=> _ApplicationEntryRequestTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ApplicationEntryResponseTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "\"ApplicationEntryResponse\"",
				[
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.IdApplicationEntryResponse), typeof(Guid), "\"IdApplicationEntryResponse\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.IdApplicationEntry), typeof(Guid), "\"IdApplicationEntry\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.ElapsedMilliseconds), typeof(decimal), "\"ElapsedMilliseconds\"", "numeric", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.StatusCode), typeof(string), "\"StatusCode\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.Metadata), typeof(string), "\"Metadata\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.Error), typeof(string), "\"Error\"", "text", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.MimeType), typeof(string), "\"MimeType\"", "varchar(1023)", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.ContentEncoding), typeof(string), "\"ContentEncoding\"", "varchar(63)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.ByteArrayContent), typeof(byte[]), "\"ByteArrayContent\"", "bytea", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.JsonContent), typeof(string), "\"JsonContent\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.StringContent), typeof(string), "\"StringContent\"", "text", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.DbOid), typeof(long?), "\"DbOid\"", "bigint", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.Name), typeof(string), "\"Name\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.RelativePath), typeof(string), "\"RelativePath\"", "varchar(1023)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.IsCompressed), typeof(bool), "\"IsCompressed\"", "boolean", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse.EncryptionKey), typeof(string), "\"EncryptionKey\"", "text", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetApplicationEntryResponseTableInfo()
		=> _ApplicationEntryResponseTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _ApplicationEntryTokenTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "\"ApplicationEntryToken\"",
				[
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryToken.IdApplicationEntryToken), typeof(Guid), "\"IdApplicationEntryToken\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryToken.Token), typeof(string), "\"Token\"", "varchar(255)", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryToken.SourceFilePath), typeof(string), "\"SourceFilePath\"", "varchar(511)", false),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryToken.MethodInfo), typeof(string), "\"MethodInfo\"", "varchar(511)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryToken.AggregateName), typeof(string), "\"AggregateName\"", "varchar(255)", true),
					new(nameof(Legion.ADF.Audit.Model.ApplicationEntryToken.Description), typeof(string), "\"Description\"", "varchar(511)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetApplicationEntryTokenTableInfo()
		=> _ApplicationEntryTokenTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _AuditEntryTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "\"AuditEntry\"",
				[
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.IdAuditEntry), typeof(Guid), "\"IdAuditEntry\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.CreatedUtc), typeof(DateTime), "\"CreatedUtc\"", "timestamp with time zone", false),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.IdAuditOperation), typeof(Guid), "\"IdAuditOperation\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.TableName), typeof(string), "\"TableName\"", "varchar(255)", false),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.IdUser), typeof(Guid?), "\"IdUser\"", "uuid", true),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.PrimaryKey), typeof(string), "\"PrimaryKey\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.OldValues), typeof(string), "\"OldValues\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.NewValues), typeof(string), "\"NewValues\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.AffectedColumns), typeof(string), "\"AffectedColumns\"", "jsonb", true),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.AuditCorrelationId), typeof(Guid), "\"AuditCorrelationId\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.TraceFrame), typeof(string), "\"TraceFrame\"", "text", true),
					new(nameof(Legion.ADF.Audit.Model.AuditEntry.CorrelationId), typeof(Guid?), "\"CorrelationId\"", "uuid", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetAuditEntryTableInfo()
		=> _AuditEntryTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _AuditOperationTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"aud", "\"AuditOperation\"",
				[
					new(nameof(Legion.ADF.Audit.Model.AuditOperation.IdAuditOperation), typeof(Guid), "\"IdAuditOperation\"", "uuid", false),
					new(nameof(Legion.ADF.Audit.Model.AuditOperation.Code), typeof(string), "\"Code\"", "varchar(15)", false),
					new(nameof(Legion.ADF.Audit.Model.AuditOperation.Name), typeof(string), "\"Name\"", "varchar(15)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetAuditOperationTableInfo()
		=> _AuditOperationTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Audit.Model.ApplicationEntry), GetApplicationEntryTableInfo() },
			{ typeof(Legion.ADF.Audit.Model.ApplicationEntryRequest), GetApplicationEntryRequestTableInfo() },
			{ typeof(Legion.ADF.Audit.Model.ApplicationEntryResponse), GetApplicationEntryResponseTableInfo() },
			{ typeof(Legion.ADF.Audit.Model.ApplicationEntryToken), GetApplicationEntryTokenTableInfo() },
			{ typeof(Legion.ADF.Audit.Model.AuditEntry), GetAuditEntryTableInfo() },
			{ typeof(Legion.ADF.Audit.Model.AuditOperation), GetAuditOperationTableInfo() },
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
