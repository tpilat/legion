using Legion.ADF.Auth.Queries.User;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.User;

public class GetUserPermissionsAndRolesById :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.User,
		Auth.Model.User?,
		GetUserPermissionsAndRolesByIdQuery>,
	IGetUserPermissionsAndRolesById
{
	public GetUserPermissionsAndRolesById(
		IEFConnectionProvider connectionProvider,
		GetUserPermissionsAndRolesByIdQuery getUserPermissionsAndRolesByIdQuery)
		: base(connectionProvider, getUserPermissionsAndRolesByIdQuery)
	{
	}

	protected override IQueryable<Auth.Model.User> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);

		if (QueryRequest.GetDeleted)
		{
			return context.User
				.Include(x => x.UserPermissions)
				.Include(x => x.UserRoles);
		}
		else
		{
			return context.User
				.Include(x => x.UserPermissions.Where(up => up.DeletedUtc == DateTime.MinValue))
				.Include(x => x.UserRoles.Where(ur => ur.DeletedUtc == DateTime.MinValue));
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
				u => u.IdUser == QueryRequest.IdUser);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				u => u.IdUser == QueryRequest.IdUser && u.DeletedUtc == DateTime.MinValue);
		}
	}

	private string? _queryRequestType;
	private string? _entityType;
	private Caching.CacheKeyAndTags GetCacheKey(IQueryable<Auth.Model.User> queryable)
	{
		//var expression = queryable.ToExpressionString();
		//var sql = queryable.ToQueryString();

		var queryRequestType = _queryRequestType ??= QueryRequest.GetType().FullName;
		var entityType = _entityType ??= typeof(Auth.Model.User).FullName;
		return new Caching.CacheKeyAndTags($"#{entityType}#|{queryRequestType}|{QueryRequest.CheckReadPermissions}", [entityType]);
	}

	public override async Task<Auth.Model.User?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return QueryRequest.DisableCahce
			? await GetQuery(scopeContext).FirstOrDefaultAsync(cancellationToken)
			: await ToPermanentlyCachedResultAsync(
				scopeContext,
				GetCacheKey,
				async (queryable, ct) => await queryable.FirstOrDefaultAsync(ct),
				forceSet: false,
				setNullValue: false,
				cloneFactory: null,
				cancellationToken);
	}
}

