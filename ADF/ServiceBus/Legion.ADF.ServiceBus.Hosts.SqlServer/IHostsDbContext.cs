using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.ServiceBus.Hosts.SqlServer;

public interface IHostsDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Hosts.Model.Host> Host { get; }
	DbSet<Legion.ADF.ServiceBus.Hosts.Model.HostLog> HostLog { get; }
}
