using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.PostgreSQL.QueryHandlers.Role;

public class GetRoleByIdQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.Role.GetRoleByIdQuery, Legion.ADF.Auth.Model.Role?>
{
	public override async Task<IResult<Legion.ADF.Auth.Model.Role?>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.Role.GetRoleByIdQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<Legion.ADF.Auth.Model.Role?>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.RoleRepository.GetRoleById(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
