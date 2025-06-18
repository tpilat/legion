using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Messaging.MessageBox.QueryHandlers.MessageBoxInstance;

public class ExistsMessageBoxInstanceByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance.ExistsMessageBoxInstanceByIdQuery, bool>
{
	public override async Task<IResult<bool>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance.ExistsMessageBoxInstanceByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.MessageBoxInstanceRepository.ExistsMessageBoxInstanceById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
