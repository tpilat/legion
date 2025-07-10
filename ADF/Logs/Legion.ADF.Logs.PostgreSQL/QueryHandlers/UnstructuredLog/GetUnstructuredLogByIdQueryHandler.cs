using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Logs.PostgreSQL.QueryHandlers.UnstructuredLog;

public class GetUnstructuredLogByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Logs.Queries.UnstructuredLog.GetUnstructuredLogByIdQuery, Legion.ADF.Logs.Model.UnstructuredLog?>
{
	public override async Task<IResult<Legion.ADF.Logs.Model.UnstructuredLog?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Logs.Queries.UnstructuredLog.GetUnstructuredLogByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Logs.Model.UnstructuredLog?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<ILogsUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.UnstructuredLogRepository.GetUnstructuredLogById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
