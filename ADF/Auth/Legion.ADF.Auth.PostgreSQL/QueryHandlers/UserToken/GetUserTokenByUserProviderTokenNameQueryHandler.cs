using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.PostgreSQL.QueryHandlers.UserToken;

public class GetUserTokenByUserProviderTokenNameQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.UserToken.GetUserTokenByUserProviderTokenNameQuery, Legion.ADF.Auth.Model.UserToken?>
{
	public override async Task<IResult<Legion.ADF.Auth.Model.UserToken?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.UserToken.GetUserTokenByUserProviderTokenNameQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Auth.Model.UserToken?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.UserTokenRepository.GetUserTokenByUserProviderTokenName(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
