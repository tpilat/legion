using Legion.Extensions;

namespace Legion.ADF.Logs.SqlServer;

public class TableInfoProvider : Legion.ADF.Logs.ITableInfoProvider
{
	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _EnvironmentInfoTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[EnvironmentInfo]",
				[
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.IdEnvironmentInfo), typeof(Guid), "[IdEnvironmentInfo]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.ApplicationName), typeof(string), "[ApplicationName]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.ApplicationVersion), typeof(string), "[ApplicationVersion]", "nvarchar(15)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.RunningEnvironment), typeof(string), "[RunningEnvironment]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.ProcessName), typeof(string), "[ProcessName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.ProcessId), typeof(int?), "[ProcessId]", "int", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.FrameworkDescription), typeof(string), "[FrameworkDescription]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.TargetFramework), typeof(string), "[TargetFramework]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.CLRVersion), typeof(string), "[CLRVersion]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.EntryAssemblyName), typeof(string), "[EntryAssemblyName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.EntryAssemblyVersion), typeof(string), "[EntryAssemblyVersion]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.BaseDirectory), typeof(string), "[BaseDirectory]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.MachineName), typeof(string), "[MachineName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.CurrentAppDomainName), typeof(string), "[CurrentAppDomainName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.Is64BitOperatingSystem), typeof(bool?), "[Is64BitOperatingSystem]", "bit", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.Is64BitProcess), typeof(bool?), "[Is64BitProcess]", "bit", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.OperatingSystemArchitecture), typeof(string), "[OperatingSystemArchitecture]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.OperatingSystemPlatform), typeof(string), "[OperatingSystemPlatform]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.OperatingSystemVersion), typeof(string), "[OperatingSystemVersion]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.ProcessArchitecture), typeof(string), "[ProcessArchitecture]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.EnvironmentInfo.CommandLine), typeof(string), "[CommandLine]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetEnvironmentInfoTableInfo()
		=> _EnvironmentInfoTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _EventCounterTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[EventCounter]",
				[
					new(nameof(Legion.ADF.Logs.Model.EventCounter.IdEventCounter), typeof(Guid), "[IdEventCounter]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.IdEventCounterCategory), typeof(Guid), "[IdEventCounterCategory]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.Name), typeof(string), "[Name]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.DisplayName), typeof(string), "[DisplayName]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.CounterType), typeof(string), "[CounterType]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.DisplayRateTimeScale), typeof(string), "[DisplayRateTimeScale]", "nvarchar(31)", true),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.EventCounter.DisplayUnits), typeof(string), "[DisplayUnits]", "nvarchar(31)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetEventCounterTableInfo()
		=> _EventCounterTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _EventCounterCategoryTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[EventCounterCategory]",
				[
					new(nameof(Legion.ADF.Logs.Model.EventCounterCategory.IdEventCounterCategory), typeof(Guid), "[IdEventCounterCategory]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounterCategory.Source), typeof(string), "[Source]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounterCategory.DisplayName), typeof(string), "[DisplayName]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetEventCounterCategoryTableInfo()
		=> _EventCounterCategoryTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _EventCounterDataTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[EventCounterData]",
				[
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.IdEventCounterData), typeof(Guid), "[IdEventCounterData]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.IdEventCounter), typeof(Guid), "[IdEventCounter]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.Increment), typeof(double?), "[Increment]", "float", true),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.Mean), typeof(double?), "[Mean]", "float", true),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.Count), typeof(int?), "[Count]", "int", true),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.Min), typeof(double?), "[Min]", "float", true),
					new(nameof(Legion.ADF.Logs.Model.EventCounterData.Max), typeof(double?), "[Max]", "float", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetEventCounterDataTableInfo()
		=> _EventCounterDataTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _LocalRequestTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[LocalRequest]",
				[
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.IdLocalRequest), typeof(Guid), "[IdLocalRequest]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.IdRemoteSystem), typeof(Guid?), "[IdRemoteSystem]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.RemoteIp), typeof(string), "[RemoteIp]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.CorrelationId), typeof(Guid), "[CorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.ExternalCorrelationId), typeof(string), "[ExternalCorrelationId]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.SourceClientIdentifier), typeof(string), "[SourceClientIdentifier]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.Url), typeof(string), "[Url]", "nvarchar(2047)", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.Path), typeof(string), "[Path]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.QueryString), typeof(string), "[QueryString]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.Method), typeof(string), "[Method]", "nvarchar(15)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.Headers), typeof(string), "[Headers]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.ContentType), typeof(string), "[ContentType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.CustomCorrelationId), typeof(string), "[CustomCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequest.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetLocalRequestTableInfo()
		=> _LocalRequestTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _LocalRequestPayloadTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[LocalRequestPayload]",
				[
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.IdLocalRequestPayload), typeof(Guid), "[IdLocalRequestPayload]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.IdLocalRequest), typeof(Guid), "[IdLocalRequest]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.CreatedUtc), typeof(DateTime?), "[CreatedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.RequestContentType), typeof(string), "[RequestContentType]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.ContentHeaders), typeof(string), "[ContentHeaders]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.FileName), typeof(string), "[FileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.IsCompressed), typeof(bool), "[IsCompressed]", "bit", false),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.MediaType), typeof(string), "[MediaType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.MultipartFormDataContentName), typeof(string), "[MultipartFormDataContentName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.MultipartFormDataFileName), typeof(string), "[MultipartFormDataFileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalRequestPayload.JsonInputCSharpType), typeof(string), "[JsonInputCSharpType]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetLocalRequestPayloadTableInfo()
		=> _LocalRequestPayloadTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _LocalResponseTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[LocalResponse]",
				[
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.IdLocalResponse), typeof(Guid), "[IdLocalResponse]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.IdLocalRequest), typeof(Guid), "[IdLocalRequest]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.CorrelationId), typeof(Guid), "[CorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.ExternalCorrelationId), typeof(string), "[ExternalCorrelationId]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.StatusCode), typeof(string), "[StatusCode]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.Reason), typeof(string), "[Reason]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.Headers), typeof(string), "[Headers]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.ContentType), typeof(string), "[ContentType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.Error), typeof(string), "[Error]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.ElapsedMilliseconds), typeof(decimal?), "[ElapsedMilliseconds]", "numeric", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.CustomCorrelationId), typeof(string), "[CustomCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponse.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetLocalResponseTableInfo()
		=> _LocalResponseTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _LocalResponsePayloadTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[LocalResponsePayload]",
				[
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.IdLocalResponsePayload), typeof(Guid), "[IdLocalResponsePayload]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.IdLocalResponse), typeof(Guid), "[IdLocalResponse]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.ResponseContentType), typeof(string), "[ResponseContentType]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.ContentHeaders), typeof(string), "[ContentHeaders]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.FileName), typeof(string), "[FileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.IsCompressed), typeof(bool), "[IsCompressed]", "bit", false),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.MediaType), typeof(string), "[MediaType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.MultipartFormDataContentName), typeof(string), "[MultipartFormDataContentName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.MultipartFormDataFileName), typeof(string), "[MultipartFormDataFileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.LocalResponsePayload.JsonInputCSharpType), typeof(string), "[JsonInputCSharpType]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetLocalResponsePayloadTableInfo()
		=> _LocalResponsePayloadTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _LogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[Log]",
				[
					new(nameof(Legion.ADF.Logs.Model.Log.IdLog), typeof(Guid), "[IdLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.Log.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.Log.InternalMessage), typeof(string), "[InternalMessage]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.ClientMessage), typeof(string), "[ClientMessage]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.Detail), typeof(string), "[Detail]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.StackTrace), typeof(string), "[StackTrace]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.Component), typeof(string), "[Component]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.OperationName), typeof(string), "[OperationName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.AggregateName), typeof(string), "[AggregateName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.AggregateIdentifier), typeof(string), "[AggregateIdentifier]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.CustomCorrelationId), typeof(string), "[CustomCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.IdApplicationEntry), typeof(Guid?), "[IdApplicationEntry]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.Log.CorrelationId), typeof(Guid?), "[CorrelationId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.Log.ExternalCorrelationId), typeof(string), "[ExternalCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.ContextProperties), typeof(string), "[ContextProperties]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.IdUser), typeof(Guid?), "[IdUser]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.Log.TenantIdentifier), typeof(Guid?), "[TenantIdentifier]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.Log.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.Logs.Model.Log.LogCode), typeof(string), "[LogCode]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.SourceSystemName), typeof(string), "[SourceSystemName]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.TraceCorrelationId), typeof(Guid?), "[TraceCorrelationId]", "uniqueidentifier", true),
					new(nameof(Legion.ADF.Logs.Model.Log.TraceFrame), typeof(string), "[TraceFrame]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.SourceContext), typeof(string), "[SourceContext]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.Log.IsValidationError), typeof(bool), "[IsValidationError]", "bit", false),
					new(nameof(Legion.ADF.Logs.Model.Log.PropertyName), typeof(string), "[PropertyName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.DisplayPropertyName), typeof(string), "[DisplayPropertyName]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.Log.ValidationFailure), typeof(string), "[ValidationFailure]", "nvarchar(max)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetLogTableInfo()
		=> _LogTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _LogLevelTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[LogLevel]",
				[
					new(nameof(Legion.ADF.Logs.Model.LogLevel.IdLogLevel), typeof(Guid), "[IdLogLevel]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.LogLevel.Code), typeof(string), "[Code]", "nvarchar(31)", false),
					new(nameof(Legion.ADF.Logs.Model.LogLevel.Name), typeof(string), "[Name]", "nvarchar(31)", false),
					new(nameof(Legion.ADF.Logs.Model.LogLevel.ItemCode), typeof(int), "[ItemCode]", "int", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetLogLevelTableInfo()
		=> _LogLevelTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _RemoteRequestTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[RemoteRequest]",
				[
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.IdRemoteRequest), typeof(Guid), "[IdRemoteRequest]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.IdRemoteSystem), typeof(Guid), "[IdRemoteSystem]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.CorrelationId), typeof(Guid), "[CorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.ExternalCorrelationId), typeof(string), "[ExternalCorrelationId]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.SourceClientIdentifier), typeof(string), "[SourceClientIdentifier]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.Url), typeof(string), "[Url]", "nvarchar(2047)", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.Method), typeof(string), "[Method]", "nvarchar(15)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.Headers), typeof(string), "[Headers]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.ContentType), typeof(string), "[ContentType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.CustomCorrelationId), typeof(string), "[CustomCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequest.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetRemoteRequestTableInfo()
		=> _RemoteRequestTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _RemoteRequestPayloadTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[RemoteRequestPayload]",
				[
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.IdRemoteRequestPayload), typeof(Guid), "[IdRemoteRequestPayload]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.IdRemoteRequest), typeof(Guid), "[IdRemoteRequest]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.CreatedUtc), typeof(DateTime?), "[CreatedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.RequestContentType), typeof(string), "[RequestContentType]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.ContentHeaders), typeof(string), "[ContentHeaders]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.FileName), typeof(string), "[FileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.IsCompressed), typeof(bool), "[IsCompressed]", "bit", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.MediaType), typeof(string), "[MediaType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.MultipartFormDataContentName), typeof(string), "[MultipartFormDataContentName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.MultipartFormDataFileName), typeof(string), "[MultipartFormDataFileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteRequestPayload.JsonInputCSharpType), typeof(string), "[JsonInputCSharpType]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetRemoteRequestPayloadTableInfo()
		=> _RemoteRequestPayloadTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _RemoteResponseTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[RemoteResponse]",
				[
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.IdRemoteResponse), typeof(Guid), "[IdRemoteResponse]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.IdRemoteRequest), typeof(Guid), "[IdRemoteRequest]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.CorrelationId), typeof(Guid), "[CorrelationId]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.ExternalCorrelationId), typeof(string), "[ExternalCorrelationId]", "nvarchar(127)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.StatusCode), typeof(string), "[StatusCode]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.Reason), typeof(string), "[Reason]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.Headers), typeof(string), "[Headers]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.ContentType), typeof(string), "[ContentType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.Error), typeof(string), "[Error]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.ElapsedMilliseconds), typeof(decimal?), "[ElapsedMilliseconds]", "numeric", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.CustomCorrelationId), typeof(string), "[CustomCorrelationId]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponse.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetRemoteResponseTableInfo()
		=> _RemoteResponseTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _RemoteResponsePayloadTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[RemoteResponsePayload]",
				[
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.IdRemoteResponsePayload), typeof(Guid), "[IdRemoteResponsePayload]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.IdRemoteResponse), typeof(Guid), "[IdRemoteResponse]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.CreatedUtc), typeof(DateTime?), "[CreatedUtc]", "datetime2", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.ResponseContentType), typeof(string), "[ResponseContentType]", "nvarchar(63)", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.ByteArrayContent), typeof(byte[]), "[ByteArrayContent]", "varbinary(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.JsonContent), typeof(string), "[JsonContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.StringContent), typeof(string), "[StringContent]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.ContentHeaders), typeof(string), "[ContentHeaders]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.DbOid), typeof(long?), "[DbOid]", "bigint", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.FileName), typeof(string), "[FileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.RelativePath), typeof(string), "[RelativePath]", "nvarchar(1023)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.Metadata), typeof(string), "[Metadata]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.IsCompressed), typeof(bool?), "[IsCompressed]", "bit", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.EncryptionKey), typeof(string), "[EncryptionKey]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.ContentEncoding), typeof(string), "[ContentEncoding]", "nvarchar(63)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.MediaType), typeof(string), "[MediaType]", "nvarchar(255)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.MultipartFormDataContentName), typeof(string), "[MultipartFormDataContentName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.MultipartFormDataFileName), typeof(string), "[MultipartFormDataFileName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.RemoteResponsePayload.JsonInputCSharpType), typeof(string), "[JsonInputCSharpType]", "nvarchar(1023)", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetRemoteResponsePayloadTableInfo()
		=> _RemoteResponsePayloadTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _RemoteSystemTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[RemoteSystem]",
				[
					new(nameof(Legion.ADF.Logs.Model.RemoteSystem.IdRemoteSystem), typeof(Guid), "[IdRemoteSystem]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteSystem.Code), typeof(string), "[Code]", "nvarchar(127)", false),
					new(nameof(Legion.ADF.Logs.Model.RemoteSystem.Name), typeof(string), "[Name]", "nvarchar(127)", false),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetRemoteSystemTableInfo()
		=> _RemoteSystemTableInfo.Value;

	private readonly static Lazy<Legion.Database.Metamodel.Info.TableInfo> _UnstructuredLogTableInfo = new(() =>
		new Legion.Database.Metamodel.Info.TableInfo(
				"log", "[UnstructuredLog]",
				[
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.IdUnstructuredLog), typeof(Guid), "[IdUnstructuredLog]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.CreatedUtc), typeof(DateTime), "[CreatedUtc]", "datetime2", false),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.IdLogLevel), typeof(int), "[IdLogLevel]", "int", false),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.Message), typeof(string), "[Message]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.StackTrace), typeof(string), "[StackTrace]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.SourceContext), typeof(string), "[SourceContext]", "nvarchar(max)", true),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.RuntimeUniqueKey), typeof(Guid), "[RuntimeUniqueKey]", "uniqueidentifier", false),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.EventName), typeof(string), "[EventName]", "nvarchar(511)", true),
					new(nameof(Legion.ADF.Logs.Model.UnstructuredLog.EventId), typeof(int?), "[EventId]", "int", true),
				]));

	public static Legion.Database.Metamodel.Info.TableInfo GetUnstructuredLogTableInfo()
		=> _UnstructuredLogTableInfo.Value;

	private readonly static Lazy<Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>> _tableInfoDictionary =
		new(() => new Dictionary<Type, Legion.Database.Metamodel.Info.TableInfo>
		{
			{ typeof(Legion.ADF.Logs.Model.EnvironmentInfo), GetEnvironmentInfoTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.EventCounter), GetEventCounterTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.EventCounterCategory), GetEventCounterCategoryTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.EventCounterData), GetEventCounterDataTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.LocalRequest), GetLocalRequestTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.LocalRequestPayload), GetLocalRequestPayloadTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.LocalResponse), GetLocalResponseTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.LocalResponsePayload), GetLocalResponsePayloadTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.Log), GetLogTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.LogLevel), GetLogLevelTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.RemoteRequest), GetRemoteRequestTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.RemoteRequestPayload), GetRemoteRequestPayloadTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.RemoteResponse), GetRemoteResponseTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.RemoteResponsePayload), GetRemoteResponsePayloadTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.RemoteSystem), GetRemoteSystemTableInfo() },
			{ typeof(Legion.ADF.Logs.Model.UnstructuredLog), GetUnstructuredLogTableInfo() },
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
