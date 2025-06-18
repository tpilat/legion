using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.SqlServer.QueryHandlers.ExternalLogin;

public class GetExternalLoginByUserAndExternalIdentifierQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginByUserAndExternalIdentifierQuery, Legion.ADF.Auth.Model.ExternalLogin?>
{
	public override async Task<IResult<Legion.ADF.Auth.Model.ExternalLogin?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginByUserAndExternalIdentifierQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Auth.Model.ExternalLogin?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.ExternalLoginRepository.GetExternalLoginByUserAndExternalIdentifier(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
