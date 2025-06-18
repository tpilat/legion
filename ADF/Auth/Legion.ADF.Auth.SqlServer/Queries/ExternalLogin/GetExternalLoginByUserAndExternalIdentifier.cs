using Legion.ADF.Auth.Queries.ExternalLogin;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.ExternalLogin;

public class GetExternalLoginByUserAndExternalIdentifier :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.ExternalLogin,
		Auth.Model.ExternalLogin?,
		GetExternalLoginByUserAndExternalIdentifierQuery>,
	IGetExternalLoginByUserAndExternalIdentifier
{
	public GetExternalLoginByUserAndExternalIdentifier(
		IEFConnectionProvider connectionProvider,
		GetExternalLoginByUserAndExternalIdentifierQuery getExternalLoginByUserAndExternalIdentifierQuery)
		: base(connectionProvider, getExternalLoginByUserAndExternalIdentifierQuery)
	{
	}

	protected override IQueryable<Auth.Model.ExternalLogin> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.ExternalLogin
			.Include(el => el.LoginProvider);
	}

	public override IQueryable<Auth.Model.ExternalLogin> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.ValidToUtc.HasValue)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				el => el.IdUser == QueryRequest.IdUser
					&& el.LoginProvider.Name == QueryRequest.LoginProvider
					&& el.ExternalUserIdentifier == QueryRequest.ExternalUserIdentifier
					&& QueryRequest.ValidToUtc.Value <= el.ValidToUtc);
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
					&& el.ExternalUserIdentifier == QueryRequest.ExternalUserIdentifier);
		}
	}

	public override async Task<Auth.Model.ExternalLogin?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}
}

