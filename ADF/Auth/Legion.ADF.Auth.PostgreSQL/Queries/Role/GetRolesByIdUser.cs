using Legion.ADF.Auth.Queries.Role;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL.Queries.Role;

public class GetRolesByIdUser :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.Role,
		List<string>,
		GetRolesByIdUserQuery>,
	IGetRolesByIdUser
{
	public GetRolesByIdUser(
		IEFConnectionProvider connectionProvider,
		GetRolesByIdUserQuery getRolesByIdUserQuery)
		: base(connectionProvider, getRolesByIdUserQuery)
	{
	}

	protected override IQueryable<Auth.Model.Role> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return QueryRequest.GetDeleted
			? context.Role.Include(r => r.UserRoles.Where(ur => ur.IdUser == QueryRequest.IdUser)).ThenInclude(ur => ur.User)
			: context.Role.Include(r => r.UserRoles.Where(ur => ur.IdUser == QueryRequest.IdUser && ur.DeletedUtc == DateTime.MinValue && ur.User.DeletedUtc == DateTime.MinValue)).ThenInclude(ur => ur.User);
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
				r => r.UserRoles.Any(ur => ur.IdUser == QueryRequest.IdUser));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				r => r.DeletedUtc == DateTime.MinValue
					&& r.UserRoles.Any(ur => ur.IdUser == QueryRequest.IdUser && ur.DeletedUtc == DateTime.MinValue && ur.User.DeletedUtc == DateTime.MinValue));
		}
	}

	public override async Task<List<string>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(r => r.Name)
			.ToListAsync(cancellationToken);
	}
}

