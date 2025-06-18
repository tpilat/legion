using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public interface IOrchestrationsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration> VwOrchestration { get; set; }
}
