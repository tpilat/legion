using Legion.ADF.Auth.Queries.User;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.SqlServer.Queries.User;

public class GetUserByExternalLoginProviderIdentifier :
	QueryDefinition<
		IAuthDbContext,
		Auth.Model.User,
		Auth.Model.User?,
		GetUserByExternalLoginProviderIdentifierQuery>,
	IGetUserByExternalLoginProviderIdentifier
{
	public GetUserByExternalLoginProviderIdentifier(
		IEFConnectionProvider connectionProvider,
		GetUserByExternalLoginProviderIdentifierQuery getUserByExternalLoginProviderIdentifierQuery)
		: base(connectionProvider, getUserByExternalLoginProviderIdentifierQuery)
	{
	}

	protected override IQueryable<Auth.Model.User> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);

		if (QueryRequest.DeletedOrValidToUtc.HasValue)
		{
			return context.User
				.Include(u => u.ExternalLogins
					.Where(el => QueryRequest.DeletedOrValidToUtc.Value <= el.ValidToUtc
						&& el.LoginProvider.Name == QueryRequest.LoginProvider
						&& el.ExternalUserIdentifier == QueryRequest.ExternalUserIdentifier))
					.ThenInclude(el => el.LoginProvider);
		}
		else
		{
			return context.User
				.Include(u => u.ExternalLogins
					.Where(el => el.LoginProvider.Name == QueryRequest.LoginProvider
						&& el.ExternalUserIdentifier == QueryRequest.ExternalUserIdentifier))
					.ThenInclude(el => el.LoginProvider);
		}
	}

	public override IQueryable<Auth.Model.User> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.DeletedOrValidToUtc.HasValue)
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				u => (u.DeletedUtc == DateTime.MinValue || QueryRequest.DeletedOrValidToUtc.Value <= u.DeletedUtc)
					&& u.ExternalLogins.Any(
					el => QueryRequest.DeletedOrValidToUtc.Value <= el.ValidToUtc
						&& el.LoginProvider.Name == QueryRequest.LoginProvider
						&& el.ExternalUserIdentifier == QueryRequest.ExternalUserIdentifier));
		}
		else
		{
			return ApplyIncludesThenWhere<IAuthAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				u => u.ExternalLogins.Any(
					el => el.LoginProvider.Name == QueryRequest.LoginProvider
					&& el.ExternalUserIdentifier == QueryRequest.ExternalUserIdentifier));
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

