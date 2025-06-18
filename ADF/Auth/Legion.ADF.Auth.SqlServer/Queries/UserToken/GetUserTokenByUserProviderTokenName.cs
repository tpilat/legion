using Legion.ADF.Auth.Queries.UserToken;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.UserToken;

public class GetUserTokenByUserProviderTokenName :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.UserToken,
		Auth.Model.UserToken?,
		GetUserTokenByUserProviderTokenNameQuery>,
	IGetUserTokenByUserProviderTokenName
{
	public GetUserTokenByUserProviderTokenName(
		IEFConnectionProvider connectionProvider,
		GetUserTokenByUserProviderTokenNameQuery getUserTokenByUserProviderTokenNameQuery)
		: base(connectionProvider, getUserTokenByUserProviderTokenNameQuery)
	{
	}

	protected override IQueryable<Auth.Model.UserToken> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.UserToken
			.Include(el => el.LoginProvider)
			.Include(el => el.User);
	}

	public override IQueryable<Auth.Model.UserToken> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.ValidToUtc.HasValue)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				ut => ut.IdUser == QueryRequest.IdUser
					&& ut.LoginProvider.Name == QueryRequest.LoginProvider
					&& ut.Name == QueryRequest.TokenName
					&& QueryRequest.ValidToUtc.Value <= ut.ValidToUtc
					&& ut.User.DeletedUtc == DateTime.MinValue);
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				el => el.IdUser == QueryRequest.IdUser
					&& el.LoginProvider.Name == QueryRequest.LoginProvider
					&& el.Name == QueryRequest.TokenName);
		}
	}

	public override async Task<Auth.Model.UserToken?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

