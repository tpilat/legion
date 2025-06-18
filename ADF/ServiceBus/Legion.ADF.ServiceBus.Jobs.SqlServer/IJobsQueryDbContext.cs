using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

public interface IJobsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.VwJob> VwJob { get; set; }
}
