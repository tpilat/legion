using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.PostgreSQL.QueryHandlers.RolePermission;

public class GetRolePermissionsByRoleIdAndClaimValueQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.RolePermission.GetRolePermissionsByRoleIdAndClaimValueQuery, System.Collections.Generic.List<Legion.ADF.Auth.Model.RolePermission>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Auth.Model.RolePermission>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.RolePermission.GetRolePermissionsByRoleIdAndClaimValueQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Auth.Model.RolePermission>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.RolePermissionRepository.GetRolePermissionsByRoleIdAndClaimValue(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
