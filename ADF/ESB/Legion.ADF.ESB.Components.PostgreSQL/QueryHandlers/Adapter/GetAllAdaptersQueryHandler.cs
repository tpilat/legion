using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.ESB.Components.QueryHandlers.Adapter;

public class GetAllAdaptersQueryHandler : AsyncMessageHandlerBase<Legion.ADF.ESB.Components.Queries.Adapter.GetAllAdaptersQuery, System.Collections.Generic.List<Legion.ADF.ESB.Components.Model.Adapter>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.ESB.Components.Model.Adapter>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.ESB.Components.Queries.Adapter.GetAllAdaptersQuery query,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.ESB.Components.Model.Adapter>>();

		var uowResult = unitOfWorkProvider.Create<IComponentsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		var uow = uowResult.Data!;
		var data = await uow.AdapterRepository.GetAllAdapters(query)
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
