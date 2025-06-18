using Legion.ADF.Auth.Queries.User;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.User;

public class GetUserByNormalizedRoleName :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.User,
		List<Auth.Model.User>,
		GetUserByNormalizedRoleNameQuery>,
	IGetUserByNormalizedRoleName
{
	public GetUserByNormalizedRoleName(
		IEFConnectionProvider connectionProvider,
		GetUserByNormalizedRoleNameQuery getUserByNormalizedRoleNameQuery)
		: base(connectionProvider, getUserByNormalizedRoleNameQuery)
	{
	}

	protected override IQueryable<Auth.Model.User> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return context.User
			.Include(u => u.UserRoles.Where(ur => ur.Role.NormalizedName == QueryRequest.NormalizedRoleName))
				.ThenInclude(up => up.Role);
		}
		else
		{
			return context.User
			.Include(u => u.UserRoles.Where(ur => ur.Role.NormalizedName == QueryRequest.NormalizedRoleName && ur.DeletedUtc == DateTime.MinValue))
				.ThenInclude(up => up.Role);
		}
	}

	public override IQueryable<Auth.Model.User> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				u => u.UserRoles.Any(ur => ur.Role.NormalizedName == QueryRequest.NormalizedRoleName));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				u => u.DeletedUtc == DateTime.MinValue
					&& u.UserRoles.Any(ur => ur.Role.NormalizedName == QueryRequest.NormalizedRoleName
						&& ur.DeletedUtc == DateTime.MinValue
						&& ur.Role.DeletedUtc == DateTime.MinValue));
		}
	}

	public override async Task<List<Auth.Model.User>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}
}

