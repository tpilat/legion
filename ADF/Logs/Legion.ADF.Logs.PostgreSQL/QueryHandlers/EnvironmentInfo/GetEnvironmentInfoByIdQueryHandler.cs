using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Logs.PostgreSQL.QueryHandlers.EnvironmentInfo;

public class GetEnvironmentInfoByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Logs.Queries.EnvironmentInfo.GetEnvironmentInfoByIdQuery, Legion.ADF.Logs.Model.EnvironmentInfo?>
{
	public override async Task<IResult<Legion.ADF.Logs.Model.EnvironmentInfo?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Logs.Queries.EnvironmentInfo.GetEnvironmentInfoByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Logs.Model.EnvironmentInfo?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<ILogsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.EnvironmentInfoRepository.GetEnvironmentInfoById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
