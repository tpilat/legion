using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public interface IJobsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.VwJob> VwJob { get; set; }
}
