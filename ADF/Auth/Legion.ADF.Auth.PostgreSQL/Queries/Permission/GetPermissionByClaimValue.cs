using Legion.ADF.Auth.Queries.Permission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.Permission;

public class GetPermissionByClaimValue :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Permission,
		Auth.Model.Permission?,
		GetPermissionByClaimValueQuery>,
	IGetPermissionByClaimValue
{
	public GetPermissionByClaimValue(
		IEFConnectionProvider connectionProvider,
		GetPermissionByClaimValueQuery getPermissionByClaimValueQuery)
		: base(connectionProvider, getPermissionByClaimValueQuery)
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

		return ApplyIncludesThenWhere<IAuthAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			p =>
				p.Code.Equals(QueryRequest.ClaimValue)
				|| p.Name.Equals(QueryRequest.ClaimValue)
				|| (p.ClaimValue != null && p.ClaimValue.Equals(QueryRequest.ClaimValue)));
	}

	public override async Task<Auth.Model.Permission?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

