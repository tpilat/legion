using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.SqlServer.QueryHandlers.UserRole;

public class GetUserRoleByIdUserAndNormalizedRoleNameQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.UserRole.GetUserRoleByIdUserAndNormalizedRoleNameQuery, Legion.ADF.Auth.Model.UserRole?>
{
	public override async Task<IResult<Legion.ADF.Auth.Model.UserRole?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.UserRole.GetUserRoleByIdUserAndNormalizedRoleNameQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Auth.Model.UserRole?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.UserRoleRepository.GetUserRoleByIdUserAndNormalizedRoleName(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
