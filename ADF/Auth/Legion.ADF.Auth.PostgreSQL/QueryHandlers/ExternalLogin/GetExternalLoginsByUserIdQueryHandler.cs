using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.PostgreSQL.QueryHandlers.ExternalLogin;

public class GetExternalLoginsByUserIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginsByUserIdQuery, System.Collections.Generic.List<Legion.ADF.Auth.Model.ExternalLogin>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Auth.Model.ExternalLogin>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginsByUserIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Auth.Model.ExternalLogin>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.ExternalLoginRepository.GetExternalLoginsByUserId(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
