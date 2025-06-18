using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public interface IOrchestrationsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ESB.Orchestrations.Model.VwOrchestration> VwOrchestration { get; set; }
}
