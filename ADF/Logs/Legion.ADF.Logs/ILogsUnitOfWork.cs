using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Logs;

public partial interface ILogsUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Logs.Model.Repositories.IEnvironmentInfoRepository EnvironmentInfoRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IEventCounterRepository EventCounterRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IEventCounterCategoryRepository EventCounterCategoryRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IEventCounterDataRepository EventCounterDataRepository { get; }

	Legion.ADF.Logs.Model.Repositories.ILocalRequestRepository LocalRequestRepository { get; }

	Legion.ADF.Logs.Model.Repositories.ILocalRequestPayloadRepository LocalRequestPayloadRepository { get; }

	Legion.ADF.Logs.Model.Repositories.ILocalResponseRepository LocalResponseRepository { get; }

	Legion.ADF.Logs.Model.Repositories.ILocalResponsePayloadRepository LocalResponsePayloadRepository { get; }

	Legion.ADF.Logs.Model.Repositories.ILogRepository LogRepository { get; }

	Legion.ADF.Logs.Model.Repositories.ILogLevelRepository LogLevelRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IRemoteRequestRepository RemoteRequestRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IRemoteRequestPayloadRepository RemoteRequestPayloadRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IRemoteResponseRepository RemoteResponseRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IRemoteResponsePayloadRepository RemoteResponsePayloadRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IRemoteSystemRepository RemoteSystemRepository { get; }

	Legion.ADF.Logs.Model.Repositories.IUnstructuredLogRepository UnstructuredLogRepository { get; }
}
