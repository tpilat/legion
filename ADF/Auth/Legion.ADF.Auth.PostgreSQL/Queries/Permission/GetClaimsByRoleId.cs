using Legion.ADF.Auth.Queries.Permission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.Permission;

public class GetClaimsByRoleId :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Permission,
		List<System.Security.Claims.Claim>,
		GetClaimsByRoleIdQuery>,
	IGetClaimsByRoleId
{
	public GetClaimsByRoleId(
		IEFConnectionProvider connectionProvider,
		GetClaimsByRoleIdQuery getClaimsByRoleIdQuery)
		: base(connectionProvider, getClaimsByRoleIdQuery)
	{
	}

	protected override IQueryable<Auth.Model.Permission> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Permission
			.Include(x => x.RolePermissions).ThenInclude(rp => rp.Role);
	}

	public override IQueryable<Auth.Model.Permission> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				p => p.RolePermissions.Any(rp => rp.IdRole == QueryRequest.IdRole));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				p => p.RolePermissions.Any(rp => rp.IdRole == QueryRequest.IdRole && rp.DeletedUtc == DateTime.MinValue && rp.Role.DeletedUtc == DateTime.MinValue));
		}
	}

	public override async Task<List<System.Security.Claims.Claim>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(p => string.IsNullOrWhiteSpace(p.ClaimValue)
				? new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, p.Code)
				: new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, p.ClaimValue!))
			.ToListAsync(cancellationToken);
	}
}

