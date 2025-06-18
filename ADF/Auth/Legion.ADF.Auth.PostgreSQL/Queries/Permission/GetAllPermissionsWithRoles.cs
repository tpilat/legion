using Legion.ADF.Auth.Queries.Permission;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.Permission;

public class GetAllPermissionsWithRoles :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Permission,
		List<Auth.Model.Permission>,
		GetAllPermissionsWithRolesQuery>,
	IGetAllPermissionsWithRoles
{
	public GetAllPermissionsWithRoles(
		IEFConnectionProvider connectionProvider,
		GetAllPermissionsWithRolesQuery getAllPermissionsWithRolesQuery)
		: base(connectionProvider, getAllPermissionsWithRolesQuery)
	{
	}

	protected override IQueryable<Auth.Model.Permission> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return context.Permission
				.Include(x => x.RolePermissions);
		}
		else
		{
			return context.Permission
				.Include(x => x.RolePermissions.Where(rp => rp.DeletedUtc == DateTime.MinValue));
		}
	}

	public override IQueryable<Auth.Model.Permission> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IAuthAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
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

