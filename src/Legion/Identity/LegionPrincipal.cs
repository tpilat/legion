using System.Security.Claims;
using System.Security.Principal;

namespace Legion.Identity;

public class LegionPrincipal : ClaimsPrincipal
{
	public LegionIdentity? IdentityBase => Identity as LegionIdentity;

	public LegionPrincipal()
		: base()
	{
	}

	public LegionPrincipal(IEnumerable<ClaimsIdentity> identities)
		: base(identities)
	{
	}

	public LegionPrincipal(BinaryReader reader)
		: base(reader)
	{
	}

	public LegionPrincipal(IIdentity identity)
		: base(identity)
	{
	}

	public LegionPrincipal(IPrincipal principal)
		: base(principal)
	{
	}

	public bool IsInRole(Guid role)
	{
		return IdentityBase != null && IdentityBase.IsInRole(role);
	}

	public bool IsInAllRoles(params Guid[] roles)
	{
		return IdentityBase != null && IdentityBase.IsInAllRoles(roles);
	}

	public bool IsInAnyRole(params Guid[] roles)
	{
		return IdentityBase != null && IdentityBase.IsInAnyRole(roles);
	}

	public bool IsInRoleForCurrentTenant(Guid role)
	{
		return IdentityBase != null && IdentityBase.IsInRoleForCurrentTenant(role);
	}

	public bool IsInAllRolesForCurrentTenant(params Guid[] roles)
	{
		return IdentityBase != null && IdentityBase.IsInAllRolesForCurrentTenant(roles);
	}

	public bool IsInAnyRoleForCurrentTenant(params Guid[] roles)
	{
		return IdentityBase != null && IdentityBase.IsInAnyRoleForCurrentTenant(roles);
	}

	public bool IsInRole(Guid idTenant, Guid role)
	{
		return IdentityBase != null && IdentityBase.IsInRole(idTenant, role);
	}

	public bool IsInAllRoles(Guid idTenant, params Guid[] roles)
	{
		return IdentityBase != null && IdentityBase.IsInAllRoles(idTenant, roles);
	}

	public bool IsInAnyRole(Guid idTenant, params Guid[] roles)
	{
		return IdentityBase != null && IdentityBase.IsInAnyRole(idTenant, roles);
	}

	public bool HasPermission(Guid permission)
	{
		return IdentityBase != null && IdentityBase.HasPermission(permission);
	}

	public bool HasAllPermissions(params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAllPermissions(permissions);
	}

	public bool HasAnyPermission(params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAnyPermission(permissions);
	}

	public bool HasPermissionForCurrentTenant(Guid permission)
	{
		return IdentityBase != null && IdentityBase.HasPermissionForCurrentTenant(permission);
	}

	public bool HasAllPermissionsForCurrentTenant(params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAllPermissionsForCurrentTenant(permissions);
	}

	public bool HasAnyPermissionForCurrentTenant(params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAnyPermissionForCurrentTenant(permissions);
	}

	public bool HasPermission(Guid idTenant, Guid permission)
	{
		return IdentityBase != null && IdentityBase.HasPermission(idTenant, permission);
	}

	public bool HasAllPermissions(Guid idTenant, params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAllPermissions(idTenant, permissions);
	}

	public bool HasAnyPermission(Guid idTenant, params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAnyPermission(idTenant, permissions);
	}

	public bool HasPermissionClaim(Guid permission)
	{
		return IdentityBase != null && IdentityBase.HasPermissionClaim(permission);
	}

	public bool HasAllPermissionClaims(params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAllPermissionClaims(permissions);
	}

	public bool HasAnyPermissionClaim(params Guid[] permissions)
	{
		return IdentityBase != null && IdentityBase.HasAnyPermissionClaim(permissions);
	}

	public void AddClaim(Claim claim)
	{
		if (IdentityBase == null)
			throw new InvalidOperationException($"{nameof(IdentityBase)} == null");

		IdentityBase.AddClaim(claim);
	}

	public void AddClaims(IEnumerable<Claim> claims)
	{
		if (IdentityBase == null)
			throw new InvalidOperationException($"{nameof(IdentityBase)} == null");

		IdentityBase.AddClaims(claims);
	}

	public Claim? FindFirstClaim(string type, string value)
	{
		return IdentityBase?.FindFirstClaim(type, value);
	}

	public bool HasClaim(string type)
	{
		return IdentityBase != null && IdentityBase.HasClaim(type);
	}

	public IEnumerable<Claim?>? FindAllLegionClaims(string type)
	{
		return IdentityBase?.FindAllLegionClaims(type);
	}

	public virtual IEnumerable<Claim>? FindAllLegionClaims(Predicate<Claim> match)
	{
		return IdentityBase?.FindAllLegionClaims(match);
	}

	public virtual Claim? FindFirstLegionClaim(Predicate<Claim> match)
	{
		return IdentityBase?.FindFirstLegionClaim(match);
	}

	public virtual Claim? FindFirstLegionClaim(string type)
	{
		return IdentityBase?.FindFirstLegionClaim(type);
	}

	public virtual Claim? FindFirstLegionClaim(string type, string value)
	{
		return IdentityBase?.FindFirstLegionClaim(type, value);
	}

	public virtual bool HasLegionClaim(string type)
	{
		return IdentityBase != null && IdentityBase.HasLegionClaim(type);
	}

	public virtual bool HasLegionClaim(string type, string value)
	{
		return IdentityBase != null && IdentityBase.HasLegionClaim(type, value);
	}

	public virtual bool HasLegionClaim(Predicate<Claim> match)
	{
		return IdentityBase != null && IdentityBase.HasLegionClaim(match);
	}

	public static LegionPrincipal? Create(string authenticationSchemeType, IdentityData authenticatedUser)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(authenticationSchemeType);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(authenticatedUser);

		var claimsIdentity = new ClaimsIdentity(authenticationSchemeType);
		claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, authenticatedUser.Login));
		return Create(claimsIdentity, authenticatedUser, true, true);
	}

	public static LegionPrincipal? Create(
		IIdentity? identity,
		IdentityData? authenticatedUser,
		bool rolesToClams,
		bool permissionsToClaims)
	{
		if (identity == null || authenticatedUser == null)
			return null;

		var LegionIdentity = new LegionIdentity(
			identity,
			authenticatedUser,
			rolesToClams,
			permissionsToClaims);

		var LegionPrincipal = new LegionPrincipal(LegionIdentity);
		return LegionPrincipal;
	}
}
