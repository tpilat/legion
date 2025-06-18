using Legion.ADF.Auth.Queries.Permission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.Permission;

public class GetPermissionsByClaimValues :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Permission,
		List<Auth.Model.Permission>,
		GetPermissionsByClaimValuesQuery>,
	IGetPermissionsByClaimValues
{
	public GetPermissionsByClaimValues(
		IEFConnectionProvider connectionProvider,
		GetPermissionsByClaimValuesQuery getPermissionsByClaimValuesQuery)
		: base(connectionProvider, getPermissionsByClaimValuesQuery)
	{
	}

	protected override IQueryable<Auth.Model.Permission> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Permission;
	}

	public override IQueryable<Auth.Model.Permission> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.ClaimValues == null
			|| QueryRequest.ClaimValues.Count == 0)
			return Enumerable.Empty<Auth.Model.Permission>().AsAsyncQueryable();

		return ApplyIncludesThenWhere<IAuthAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			p => QueryRequest.ClaimValues.Any(cv =>
				p.Code.Equals(cv)
				|| p.Name.Equals(cv)
				|| (p.ClaimValue != null && p.ClaimValue.Equals(cv))));
	}

	public override async Task<List<Auth.Model.Permission>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}

