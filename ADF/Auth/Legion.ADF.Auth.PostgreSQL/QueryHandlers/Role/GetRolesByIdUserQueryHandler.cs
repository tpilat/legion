using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.PostgreSQL.QueryHandlers.Role;

public class GetRolesByIdUserQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.Role.GetRolesByIdUserQuery, System.Collections.Generic.List<string>>
{
	public override async Task<IResult<System.Collections.Generic.List<string>>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.Role.GetRolesByIdUserQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<System.Collections.Generic.List<string>>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.RoleRepository.GetRolesByIdUser(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
