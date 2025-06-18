using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserEmailStore<Model.User>
{
	public async Task<Model.User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(normalizedEmail), normalizedEmail);

		return await UoW.UserRepository
			.GetUserByNormalizedEmail(
				new Queries.User.GetValidUserByNormalizedEmailQuery(normalizedEmail, CheckReadPermissions: false))
			.ToResultAsync(scopeContext, cancellationToken);
	}

	public Task<string?> GetEmailAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.Email);
	}

	public Task<bool> GetEmailConfirmedAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.EmailConfirmed);
	}

	public Task<string?> GetNormalizedEmailAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.NormalizedEmail);
	}

	public Task SetEmailAsync(Model.User user, string? email, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetEmail(scopeContext, email);

		return Task.CompletedTask;
	}

	public Task SetEmailConfirmedAsync(Model.User user, bool emailConfirmed, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetEmailConfirmed(scopeContext, emailConfirmed);

		return Task.CompletedTask;
	}

	public Task SetNormalizedEmailAsync(Model.User user, string? normalizedEmail, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetNormalizedEmail(scopeContext, normalizedEmail);

		return Task.CompletedTask;
	}
}
