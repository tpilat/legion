using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IUserTwoFactorRecoveryCodeStore<Model.User>
{
	private const string RecoveryCodeTokenName = "RecoveryCodes";

	public async Task<int> CountCodesAsync(Model.User user, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";
		if (0 < mergedCodes.Length)
		{
#if NET8_0_OR_GREATER
			return mergedCodes.AsSpan().Count(';') + 1;
#else
            // non-allocating version of mergedCodes.Split(';').Length
            var count = 1;
            var index = 0;
            while (index < mergedCodes.Length)
            {
                var semiColonIndex = mergedCodes.IndexOf(';', index);
                if (semiColonIndex < 0)
                {
                    break;
                }
                count++;
                index = semiColonIndex + 1;
            }
            return count;
#endif
		}
		return 0;
	}

	public async Task<bool> RedeemCodeAsync(Model.User user, string code, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);
		Legion.Throw.IfArgumentNullOrWhiteSpace(code);

		var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";
		var splitCodes = mergedCodes.Split(';');
		if (splitCodes.Contains(code))
		{
			var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
			await ReplaceCodesAsync(user, updatedCodes, cancellationToken).ConfigureAwait(false);
			return true;
		}
		return false;
	}

	public Task ReplaceCodesAsync(Model.User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken = default)
	{
		Legion.Throw.IfCancellationRequested(cancellationToken);
		ThrowIfDisposed();
		Legion.Throw.IfArgumentNull(user);

		var mergedCodes = string.Join(";", recoveryCodes);
		return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
	}
}
