using Legion.ADF.Auth.Queries.Role;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.Role;

public class GetAllRolesWithPermissions :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Role,
		List<Auth.Model.Role>,
		GetAllRolesWithPermissionsQuery>,
	IGetAllRolesWithPermissions
{
	public GetAllRolesWithPermissions(
		IEFConnectionProvider connectionProvider,
		GetAllRolesWithPermissionsQuery getAllRolesWithPermissionsQuery)
		: base(connectionProvider, getAllRolesWithPermissionsQuery)
	{
	}

	protected override IQueryable<Auth.Model.Role> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return context.Role
				.Include(x => x.RolePermissions);
		}
		else
		{
			return context.Role
				.Include(x => x.RolePermissions.Where(rp => rp.DeletedUtc == DateTime.MinValue));
		}
	}

	public override IQueryable<Auth.Model.Role> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				null);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				r => r.DeletedUtc == DateTime.MinValue);
		}
	}

	public override async Task<List<Auth.Model.Role>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}

