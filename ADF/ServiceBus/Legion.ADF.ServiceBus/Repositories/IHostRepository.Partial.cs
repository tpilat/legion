namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IHostRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.Host>
{
	Task<bool> IsAliveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
