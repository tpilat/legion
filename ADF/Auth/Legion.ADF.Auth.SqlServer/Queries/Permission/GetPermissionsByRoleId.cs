using Legion.ADF.Auth.Queries.Permission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.Permission;

public class GetPermissionsByRoleId :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Permission,
		List<Auth.Model.Permission>,
		GetPermissionsByRoleIdQuery>,
	IGetPermissionsByRoleId
{
	public GetPermissionsByRoleId(
		IEFConnectionProvider connectionProvider,
		GetPermissionsByRoleIdQuery getPermissionsByRoleIdQuery)
		: base(connectionProvider, getPermissionsByRoleIdQuery)
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

	public override async Task<List<Auth.Model.Permission>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}

