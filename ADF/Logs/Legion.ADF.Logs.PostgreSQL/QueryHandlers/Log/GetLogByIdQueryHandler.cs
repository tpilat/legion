using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Logs.PostgreSQL.QueryHandlers.Log;

public class GetLogByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Logs.Queries.Log.GetLogByIdQuery, Legion.ADF.Logs.Model.Log?>
{
	public override async Task<IResult<Legion.ADF.Logs.Model.Log?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Logs.Queries.Log.GetLogByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Logs.Model.Log?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<ILogsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.LogRepository.GetLogById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
