using Legion.ADF.Auth.Queries.User;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.User;

public class GetUserByNormalizedLogin :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.User,
		Auth.Model.User?,
		GetUserByNormalizedLoginQuery>,
	IGetUserByNormalizedLogin
{
	public GetUserByNormalizedLogin(
		IEFConnectionProvider connectionProvider,
		GetUserByNormalizedLoginQuery getUserByNormalizedLoginQuery)
		: base(connectionProvider, getUserByNormalizedLoginQuery)
	{
	}

	protected override IQueryable<Auth.Model.User> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.User;
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
				u => u.NormalizedLogin == QueryRequest.NormalizedLogin);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				u => u.NormalizedLogin == QueryRequest.NormalizedLogin && u.DeletedUtc == DateTime.MinValue);
		}
	}

	public override async Task<Auth.Model.User?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

