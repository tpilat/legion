using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ServiceBus.Hosts;

public partial interface IHostsUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.ServiceBus.Hosts.Model.Repositories.IHostRepository HostRepository { get; }

	Legion.ADF.ServiceBus.Hosts.Model.Repositories.IHostLogRepository HostLogRepository { get; }
}
