using Legion;
using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserSecurityStampStore<Model.User>
{
	public Task<string?> GetSecurityStampAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.SecurityStamp);
	}

	public Task SetSecurityStampAsync(Model.User user, string securityStamp, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);
		Throw.IfArgumentNullOrWhiteSpace(securityStamp);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetSecurityStamp(scopeContext, securityStamp);

		return Task.CompletedTask;
	}
}
