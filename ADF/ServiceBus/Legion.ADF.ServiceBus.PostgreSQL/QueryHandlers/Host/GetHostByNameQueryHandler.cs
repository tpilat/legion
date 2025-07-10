using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.ServiceBus.QueryHandlers.Host;

public class GetHostByNameQueryHandler : AsyncMessageHandlerBase<Legion.ADF.ServiceBus.Queries.Host.GetHostByNameQuery, Legion.ADF.ServiceBus.Model.Host?>
{
	public override async Task<IResult<Legion.ADF.ServiceBus.Model.Host?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.ServiceBus.Queries.Host.GetHostByNameQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.ServiceBus.Model.Host?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IServiceBusUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.HostRepository.GetHostByName(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
