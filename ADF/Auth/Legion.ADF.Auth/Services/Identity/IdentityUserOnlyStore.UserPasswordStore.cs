using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserPasswordStore<Model.User>
{
	public Task<string?> GetPasswordHashAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		//return Task.FromResult(new PasswordHasher<Model.User>().HashPassword(user, "a"));

		return Task.FromResult(user.PasswordHash);
	}

	public Task<bool> HasPasswordAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		return Task.FromResult(user.PasswordHash != null);
	}

	public Task SetPasswordHashAsync(Model.User user, string? passwordHash, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetPasswordHash(scopeContext, passwordHash);

		return Task.CompletedTask;
	}
}
