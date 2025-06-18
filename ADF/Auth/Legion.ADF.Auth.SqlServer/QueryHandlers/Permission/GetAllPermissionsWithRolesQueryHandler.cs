using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.SqlServer.QueryHandlers.Permission;

public class GetAllPermissionsWithRolesQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.Permission.GetAllPermissionsWithRolesQuery, System.Collections.Generic.List<Legion.ADF.Auth.Model.Permission>>
{
	public override async Task<IResult<System.Collections.Generic.List<Legion.ADF.Auth.Model.Permission>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.Permission.GetAllPermissionsWithRolesQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<Legion.ADF.Auth.Model.Permission>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.PermissionRepository.GetAllPermissionsWithRoles(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
