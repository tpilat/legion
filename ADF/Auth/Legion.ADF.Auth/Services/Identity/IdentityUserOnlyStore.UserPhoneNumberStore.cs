using Legion;
using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserPhoneNumberStore<Model.User>
{
	public Task<string?> GetPhoneNumberAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.PhoneNumber);
	}

	public Task<bool> GetPhoneNumberConfirmedAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		return Task.FromResult(user.PhoneNumberConfirmed);
	}

	public Task SetPhoneNumberAsync(Model.User user, string? phoneNumber, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetPhoneNumber(scopeContext, phoneNumber);

		return Task.CompletedTask;
	}

	public Task SetPhoneNumberConfirmedAsync(Model.User user, bool phoneNumberConfirmed, CancellationToken cancellationToken = default)
	{
		Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Throw.IfArgumentNull(user);

		var scopeContext = ScopeContext.Create("Legion.ADF.Auth.Identity")
			.AddContextProperty(nameof(user.IdUser), user.IdUser.ToString());

		user.SetPhoneNumberConfirmed(scopeContext, phoneNumberConfirmed);

		return Task.CompletedTask;
	}
}
