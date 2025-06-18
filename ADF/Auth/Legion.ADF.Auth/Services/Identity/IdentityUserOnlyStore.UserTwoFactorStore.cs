using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserTwoFactorStore<Model.User>
{
	public Task<bool> GetTwoFactorEnabledAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.MultiFactorEnabled);
	}

	public Task SetTwoFactorEnabledAsync(Model.User user, bool multiFactorEnabled, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetMultiFactorEnabled(scopeContext, multiFactorEnabled);

		return Task.CompletedTask;
	}
}
