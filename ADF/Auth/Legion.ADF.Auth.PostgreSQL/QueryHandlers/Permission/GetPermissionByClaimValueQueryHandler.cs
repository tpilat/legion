using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.PostgreSQL.QueryHandlers.Permission;

public class GetPermissionByClaimValueQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.Permission.GetPermissionByClaimValueQuery, Legion.ADF.Auth.Model.Permission?>
{
	public override async Task<IResult<Legion.ADF.Auth.Model.Permission?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.Permission.GetPermissionByClaimValueQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Auth.Model.Permission?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.PermissionRepository.GetPermissionByClaimValue(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
