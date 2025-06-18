using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace Legion.ADF.Auth.PostgreSQL.QueryHandlers.UserRole;

public class IsInRoleQueryHandler : AsyncMessageHandlerBase<Legion.ADF.Auth.Queries.UserRole.IsInRoleQuery, bool>
{
	public override async Task<IResult<bool>> HandleAsync(
		IInvocationContext invocationContext,
		Legion.ADF.Auth.Queries.UserRole.IsInRoleQuery query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<bool>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.UserRoleRepository.IsInRole(query with { AsNoTracking = true })
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}
}
