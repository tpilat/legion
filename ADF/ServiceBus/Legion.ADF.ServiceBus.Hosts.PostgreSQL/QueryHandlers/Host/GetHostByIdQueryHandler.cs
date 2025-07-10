using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.ServiceBus.Hosts.QueryHandlers.Host;

public class GetHostByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.ServiceBus.Hosts.Queries.Host.GetHostByIdQuery, Legion.ADF.ServiceBus.Hosts.Model.Host?>
{
	public override async Task<IResult<Legion.ADF.ServiceBus.Hosts.Model.Host?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.ServiceBus.Hosts.Queries.Host.GetHostByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.ServiceBus.Hosts.Model.Host?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IHostsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.HostRepository.GetHostById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
