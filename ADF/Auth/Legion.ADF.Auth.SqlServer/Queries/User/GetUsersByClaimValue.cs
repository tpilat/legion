using Legion.ADF.Auth.Queries.User;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.User;

public class GetUsersByClaimValue :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.User,
		List<Auth.Model.User>,
		GetUsersByClaimValueQuery>,
	IGetUsersByClaimValue
{
	public GetUsersByClaimValue(
		IEFConnectionProvider connectionProvider,
		GetUsersByClaimValueQuery getUsersByClaimValueQuery)
		: base(connectionProvider, getUsersByClaimValueQuery)
	{
	}

	protected override IQueryable<Auth.Model.User> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return context.User
				.Include(u => u.UserPermissions.Where(up =>
					up.Permission.Code.Equals(QueryRequest.ClaimValue)
					|| up.Permission.Name.Equals(QueryRequest.ClaimValue)
					|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(QueryRequest.ClaimValue))))
					.ThenInclude(up => up.Permission);
		}
		else
		{
			return context.User
				.Include(u => u.UserPermissions.Where(up =>
					up.DeletedUtc == DateTime.MinValue
					&& (up.Permission.Code.Equals(QueryRequest.ClaimValue)
						|| up.Permission.Name.Equals(QueryRequest.ClaimValue)
						|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(QueryRequest.ClaimValue)))))
					.ThenInclude(up => up.Permission);
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
				u => u.UserPermissions.Any(up =>
					up.Permission.Code.Equals(QueryRequest.ClaimValue)
					|| up.Permission.Name.Equals(QueryRequest.ClaimValue)
					|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(QueryRequest.ClaimValue))));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				u => u.DeletedUtc == DateTime.MinValue
					&& u.UserPermissions.Any(up =>
						up.DeletedUtc == DateTime.MinValue
						&& (up.Permission.Code.Equals(QueryRequest.ClaimValue)
							|| up.Permission.Name.Equals(QueryRequest.ClaimValue)
							|| (up.Permission.ClaimValue != null && up.Permission.ClaimValue.Equals(QueryRequest.ClaimValue)))));
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

