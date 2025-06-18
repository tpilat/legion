using Legion;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus;

public static class ScopeContextExtensions
{
	public static bool TryGetUserAccount(this IScopeContext scopeContext, [NotNullWhen(true)] out UserAccount? userAccount)
	{
		userAccount = null;

		if (scopeContext == null)
			return false;

		return scopeContext.Principal?.TryGetUserAccount(out userAccount) == true;
	}

	public static UserAccount GetUserAccount(this IScopeContext scopeContext)
		=> scopeContext?.Principal?.GetUserAccount() ?? throw new InvalidOperationException($"Invalid identity. No {nameof(UserAccount)}");
}
