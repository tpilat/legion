using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Audit.SqlServer.QueryHandlers.ApplicationEntryToken;

public class GetApplicationEntryTokenByTokenVersionFilePathQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Audit.Queries.ApplicationEntryToken.GetApplicationEntryTokenByTokenVersionFilePathQuery, Legion.ADF.Audit.Model.ApplicationEntryToken?>
{
	public override async Task<IResult<Legion.ADF.Audit.Model.ApplicationEntryToken?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Audit.Queries.ApplicationEntryToken.GetApplicationEntryTokenByTokenVersionFilePathQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Audit.Model.ApplicationEntryToken?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuditUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.ApplicationEntryTokenRepository.GetApplicationEntryTokenByTokenVersionFilePath(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
