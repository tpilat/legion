using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ServiceBus.PostgreSQL.Model.Repositories;

public partial class HostRepository : Legion.ADF.ServiceBus.PostgreSQL.ServiceBusRepositoryBase, Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.Host>, Legion.ADF.ServiceBus.Model.Repositories.IHostRepository
{
	public async Task<bool> IsAliveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			await using var context = ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ServiceBus.PostgreSQL.IServiceBusDbContext>(scopeContext);
			var result = await context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
			return true; // no exception, DB is alive
		}
		catch
		{
			return false;
		}
	}
}
