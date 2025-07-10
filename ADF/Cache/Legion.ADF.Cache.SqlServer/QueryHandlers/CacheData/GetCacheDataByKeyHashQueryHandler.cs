using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Cache.QueryHandlers.CacheData;

public class GetCacheDataByKeyHashQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Cache.Queries.CacheData.GetCacheDataByKeyHashQuery, Legion.ADF.Cache.Model.CacheData?>
{
	public override async Task<IResult<Legion.ADF.Cache.Model.CacheData?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Cache.Queries.CacheData.GetCacheDataByKeyHashQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Cache.Model.CacheData?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<ICacheUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.CacheDataRepository.GetCacheDataByKeyHash(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
