using Legion.ADF.Auth.Queries.Permission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.Permission;

public class GetPermissionsByRoleIdAndClaimValue :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Permission,
		List<Guid>,
		GetPermissionsByRoleIdAndClaimValueQuery>,
	IGetPermissionsByRoleIdAndClaimValue
{
	public GetPermissionsByRoleIdAndClaimValue(
		IEFConnectionProvider connectionProvider,
		GetPermissionsByRoleIdAndClaimValueQuery getPermissionsByRoleIdAndClaimValueQuery)
		: base(connectionProvider, getPermissionsByRoleIdAndClaimValueQuery)
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
				p => p.RolePermissions.Any(rp => rp.IdRole == QueryRequest.IdRole)
					&& ((string.IsNullOrWhiteSpace(p.ClaimValue) && p.Code == QueryRequest.ClaimValue)
						|| (!string.IsNullOrWhiteSpace(p.ClaimValue) && p.ClaimValue == QueryRequest.ClaimValue)));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				p => p.RolePermissions.Any(rp => rp.IdRole == QueryRequest.IdRole && rp.DeletedUtc == DateTime.MinValue && rp.Role.DeletedUtc == DateTime.MinValue)
					&& ((string.IsNullOrWhiteSpace(p.ClaimValue) && p.Code == QueryRequest.ClaimValue)
						|| (!string.IsNullOrWhiteSpace(p.ClaimValue) && p.ClaimValue == QueryRequest.ClaimValue)));
		}
	}

	public override async Task<List<Guid>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(p => p.IdPermission)
			.ToListAsync(cancellationToken);
	}
}

