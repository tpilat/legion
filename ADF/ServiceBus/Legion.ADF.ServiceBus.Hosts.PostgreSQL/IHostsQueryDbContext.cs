using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Hosts.PostgreSQL;

public interface IHostsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Hosts.Model.VwHost> VwHost { get; set; }
}
