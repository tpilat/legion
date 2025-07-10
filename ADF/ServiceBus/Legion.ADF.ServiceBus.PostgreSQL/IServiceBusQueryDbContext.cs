using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.PostgreSQL;

public interface IServiceBusQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Model.VwHost> VwHost { get; set; }
	DbSet<Legion.ADF.ServiceBus.Model.VwJob> VwJob { get; set; }
	DbSet<Legion.ADF.ServiceBus.Model.VwOrchestration> VwOrchestration { get; set; }
}
