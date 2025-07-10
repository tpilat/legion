using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Cache.QueryHandlers.ReloadableCacheKey;

public class GetReloadableCacheKeyByTagsQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByTagsQuery, Legion.ADF.Cache.Model.ReloadableCacheKey?>
{
	public override async Task<IResult<Legion.ADF.Cache.Model.ReloadableCacheKey?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByTagsQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Cache.Model.ReloadableCacheKey?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<ICacheUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.ReloadableCacheKeyRepository.GetReloadableCacheKeyByTags(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
