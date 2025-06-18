using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserLockoutStore<Model.User>
{
	public Task<int> GetAccessFailedCountAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.AccessFailedCount);
	}

	public Task<bool> GetLockoutEnabledAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.LockoutEnabled);
	}

	public Task<DateTimeOffset?> GetLockoutEndDateAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult<DateTimeOffset?>(user.LockoutEndUtc.HasValue ? new DateTimeOffset(user.LockoutEndUtc.Value) : null);
	}

	public Task<int> IncrementAccessFailedCountAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.IncrementAccessFailedCount(scopeContext);

		return Task.FromResult(user.AccessFailedCount);
	}

	public Task ResetAccessFailedCountAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.ResetAccessFailedCount(scopeContext);

		return Task.CompletedTask;
	}

	public Task SetLockoutEnabledAsync(Model.User user, bool lockoutEnabled, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetLockoutEnabled(scopeContext, lockoutEnabled);

		return Task.CompletedTask;
	}

	public Task SetLockoutEndDateAsync(Model.User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetLockoutEndUtc(scopeContext, lockoutEnd?.UtcDateTime);

		return Task.CompletedTask;
	}
}
