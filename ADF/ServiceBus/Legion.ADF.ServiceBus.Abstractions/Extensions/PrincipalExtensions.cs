using Legion.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus;

public static class PrincipalExtensions
{
	public static bool TryGetUserAccount(this LegionPrincipal principal, [NotNullWhen(true)] out UserAccount? userAccount)
		=> TryGetUserAccount(principal?.IdentityBase, out userAccount);

	public static UserAccount GetUserAccount(this LegionPrincipal principal)
		=> GetUserAccount(principal?.IdentityBase);

	public static bool TryGetUserAccount(this LegionIdentity? identity, [NotNullWhen(true)] out UserAccount? userAccount)
		=> (userAccount = (identity?.UserData as UserAccount)) != null;

	public static UserAccount GetUserAccount(this LegionIdentity? identity)
		=> (identity?.UserData as UserAccount) ?? throw new InvalidOperationException($"Invalid {nameof(identity)}. No {nameof(UserAccount)}");
}
