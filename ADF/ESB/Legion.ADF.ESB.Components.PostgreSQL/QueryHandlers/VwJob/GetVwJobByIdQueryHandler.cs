using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.ESB.Components.QueryHandlers.VwJob;

public class GetVwJobByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.ESB.Components.Queries.VwJob.GetVwJobByIdQuery, Legion.ADF.ESB.Components.Model.VwJob?>
{
	public override async Task<IResult<Legion.ADF.ESB.Components.Model.VwJob?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.ESB.Components.Queries.VwJob.GetVwJobByIdQuery query,
		IUnitOfWorkProvider unitOfWorkProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.ESB.Components.Model.VwJob?>();

		var uowResult = unitOfWorkProvider.CreateQuery<IComponentsQueryUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		var uow = uowResult.Data!;
		var data = await uow.VwJobRepository.GetVwJobById(query)
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
