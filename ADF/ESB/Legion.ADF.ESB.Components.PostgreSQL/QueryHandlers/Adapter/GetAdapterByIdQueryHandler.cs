using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.ESB.Components.QueryHandlers.Adapter;

public class GetAdapterByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.ESB.Components.Queries.Adapter.GetAdapterByIdQuery, Legion.ADF.ESB.Components.Model.Adapter?>
{
	public override async Task<IResult<Legion.ADF.ESB.Components.Model.Adapter?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.ESB.Components.Queries.Adapter.GetAdapterByIdQuery query,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.ESB.Components.Model.Adapter?>();

		var uowResult = unitOfWorkProvider.Create<IComponentsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		var uow = uowResult.Data!;
		var data = await uow.AdapterRepository.GetAdapterById(query)
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
