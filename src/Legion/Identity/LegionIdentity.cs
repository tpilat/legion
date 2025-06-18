using Legion.Converters;
using Legion.Exceptions;
using Legion.Exceptions.Internal;
using Legion.Extensions;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;

namespace Legion.Identity;

public class LegionIdentity : ClaimsIdentity
{
	public const string ISSUER = "https://claims.legion.sk";
	public const string LOGIN_CLAIM_NAME = "login";
	public const string DISPLAYNAME_CLAIM_NAME = "displayName";
	public const string USER_ID_CLAIM_NAME = "userId";
	public const string ROLE_ID_CLAIM_NAME = "roleId";
	public const string PERMISSION_ID_CLAIM_NAME = "permissionId";

	private readonly Lazy<IReadOnlyList<Guid>> EMPTY_TENANTS = new(() => new List<Guid>());
	private readonly Lazy<IReadOnlyDictionary<Guid, IReadOnlyCollection<Guid>>> EMPTY_ROLE_IDS = new(() => new Dictionary<Guid, IReadOnlyCollection<Guid>>());
	private readonly Lazy<IReadOnlyDictionary<Guid, IReadOnlyCollection<Guid>>> EMPTY_PERMISSIONS_IDS = new(() => new Dictionary<Guid, IReadOnlyCollection<Guid>>());

	private readonly IReadOnlyCollection<Guid> _allRoleIds;
	private readonly IReadOnlyCollection<Guid> _allPermissionIds;

	public Guid IdUser { get; }

	public string Login { get; }

	public string DisplayName { get; }

	public object? UserData { get; }

	public bool IsSuperAdmin { get; }

	public Guid? CurrentIdTenant { get; }

	public IReadOnlyList<Guid>? Tenants { get; }

	/// <summary>
	/// IReadOnlyDictionary&lt;IdTenant, IReadOnlyCollection&lt;IdRole&gt;&gt;
	/// </summary>
	public IReadOnlyDictionary<Guid, IReadOnlyCollection<Guid>>? RoleIds { get; }

	/// <summary>
	/// IReadOnlyDictionary&lt;IdTenant, IReadOnlyCollection&lt;IdPermission&gt;&gt;
	/// </summary>
	public IReadOnlyDictionary<Guid, IReadOnlyCollection<Guid>>? PermissionIds { get; }

	public LegionIdentity(
		IIdentity identity,
		IdentityData data,
		bool saveRolesAsClams,
		bool saveRermissionsAsClaims)
		: base(identity)
	{
		Throw.IfArgumentNull(identity);
		Throw.IfArgumentNull(data);

		Throw.IfDefault(data.IdUser);
		Throw.IfNullOrWhiteSpace(data.Login);

		if (data.CurrentIdTenant.HasValue
			&& data.Tenants?.Contains(data.CurrentIdTenant.Value) != true)
			Throw.AuthenticationException(ErrorCodes.AuthenticationException.MissingTenant(data.CurrentIdTenant.Value));

		IdUser = data.IdUser;
		Login = data.Login;
		DisplayName = string.IsNullOrWhiteSpace(data.DisplayName)
			? data.Login
			: data.DisplayName;
		UserData = data.UserData;
		IsSuperAdmin = data.IsSuperAdmin;
		CurrentIdTenant = data.CurrentIdTenant;
		Tenants = data.Tenants ?? EMPTY_TENANTS.Value;
		RoleIds = data.RoleIds?.ToDictionary(x => x.Key, x => (IReadOnlyCollection<Guid>)x.Value.AsReadOnly()).AsReadOnly() ?? EMPTY_ROLE_IDS.Value;
		_allRoleIds = new ReadOnlyCollection<Guid>(RoleIds?.Values.SelectMany(x => x).Distinct().ToList() ?? []);

		PermissionIds = data.PermissionIds?.ToDictionary(x => x.Key, x => (IReadOnlyCollection<Guid>)x.Value.AsReadOnly()).AsReadOnly() ?? EMPTY_PERMISSIONS_IDS.Value;
		_allPermissionIds = new ReadOnlyCollection<Guid>(PermissionIds?.Values.SelectMany(x => x).Distinct().ToList() ?? []);

		AddImplicitClaims(saveRolesAsClams, saveRermissionsAsClaims);
	}

	private void AddImplicitClaims(bool rolesToClams, bool permissionsToClaims)
	{
		AddClaim(new Claim(LOGIN_CLAIM_NAME, Login));
		AddClaim(new Claim(DISPLAYNAME_CLAIM_NAME, DisplayName));
		AddClaim(new Claim(USER_ID_CLAIM_NAME, IdUser.ToString()!));

		if (CurrentIdTenant.HasValue)
		{
			if (rolesToClams && 0 < RoleIds?.Count)
				AddClaims(ROLE_ID_CLAIM_NAME, RoleIds[CurrentIdTenant.Value].Distinct());

			if (permissionsToClaims && 0 < PermissionIds?.Count)
				AddClaims(PERMISSION_ID_CLAIM_NAME, PermissionIds[CurrentIdTenant.Value].Distinct());
		}
		else
		{
			if (rolesToClams && 0 < RoleIds?.Count)
				AddClaims(ROLE_ID_CLAIM_NAME, _allRoleIds);

			if (permissionsToClaims && 0 < PermissionIds?.Count)
				AddClaims(PERMISSION_ID_CLAIM_NAME, _allPermissionIds);
		}
	}

	public bool IsInRole(Guid role)
	{
		return IsSuperAdmin || _allRoleIds.Contains(role);
	}

	public bool IsInAllRoles(params Guid[] roles)
	{
		Throw.IfArgumentNull(roles);

		return IsSuperAdmin || roles.All(r => _allRoleIds.Contains(r));
	}

	public bool IsInAnyRole(params Guid[] roles)
	{
		Throw.IfArgumentNull(roles);

		return IsSuperAdmin || roles.Any(r => _allRoleIds.Contains(r));
	}

	public bool IsInRoleForCurrentTenant(Guid role)
	{
		return IsSuperAdmin
			|| (CurrentIdTenant.HasValue
				&& RoleIds?.TryGetValue(CurrentIdTenant.Value, out var roleIds) == true && roleIds.Contains(role));
	}

	public bool IsInAllRolesForCurrentTenant(params Guid[] roles)
	{
		Throw.IfArgumentNull(roles);

		return IsSuperAdmin
			|| (CurrentIdTenant.HasValue
				&& RoleIds?.TryGetValue(CurrentIdTenant.Value, out var roleIds) == true
				&& roles.All(r => roleIds.Contains(r)));
	}

	public bool IsInAnyRoleForCurrentTenant(params Guid[] roles)
	{
		Throw.IfArgumentNull(roles);

		return IsSuperAdmin
			|| (CurrentIdTenant.HasValue
				&& RoleIds?.TryGetValue(CurrentIdTenant.Value, out var roleIds) == true
				&& roles.Any(r => roleIds.Contains(r)));
	}

	public bool IsInRole(Guid idTenant, Guid role)
	{
		return IsSuperAdmin || RoleIds?.TryGetValue(idTenant, out var roleIds) == true && roleIds.Contains(role);
	}

	public bool IsInAllRoles(Guid idTenant, params Guid[] roles)
	{
		Throw.IfArgumentNull(roles);

		return IsSuperAdmin
			|| (RoleIds?.TryGetValue(idTenant, out var roleIds) == true
				&& roles.All(r => roleIds.Contains(r)));
	}

	public bool IsInAnyRole(Guid idTenant, params Guid[] roles)
	{
		Throw.IfArgumentNull(roles);

		return IsSuperAdmin
			|| (RoleIds?.TryGetValue(idTenant, out var roleIds) == true
				&& roles.Any(r => roleIds.Contains(r)));
	}

	public bool HasPermission(Guid permission)
	{
		return IsSuperAdmin || _allPermissionIds.Contains(permission);
	}

	public bool HasAllPermissions(params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin || permissions.All(p => _allPermissionIds.Contains(p));
	}

	public bool HasAnyPermission(params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin || permissions.Any(p => _allPermissionIds.Contains(p));
	}

	public bool HasPermissionForCurrentTenant(Guid permission)
	{
		return IsSuperAdmin
			|| (CurrentIdTenant.HasValue
				&& PermissionIds?.TryGetValue(CurrentIdTenant.Value, out var permissionIds) == true && permissionIds.Contains(permission));
	}

	public bool HasAllPermissionsForCurrentTenant(params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin
			|| (CurrentIdTenant.HasValue
				&& PermissionIds?.TryGetValue(CurrentIdTenant.Value, out var permissionIds) == true
				&& permissions.All(p => permissionIds.Contains(p)));
	}

	public bool HasAnyPermissionForCurrentTenant(params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin
			|| (CurrentIdTenant.HasValue
				&& PermissionIds?.TryGetValue(CurrentIdTenant.Value, out var permissionIds) == true
				&& permissions.Any(p => permissionIds.Contains(p)));
	}

	public bool HasPermission(Guid idTenant, Guid permission)
	{
		return IsSuperAdmin || PermissionIds?.TryGetValue(idTenant, out var permissionIds) == true && permissionIds.Contains(permission);
	}

	public bool HasAllPermissions(Guid idTenant, params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin
			|| (PermissionIds?.TryGetValue(idTenant, out var permissionIds) == true
				&& permissions.All(p => permissionIds.Contains(p)));
	}

	public bool HasAnyPermission(Guid idTenant, params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin
			|| (PermissionIds?.TryGetValue(idTenant, out var permissionIds) == true
				&& permissions.Any(p => permissionIds.Contains(p)));
	}

	public bool HasPermissionClaim(Guid permission)
	{
		return IsSuperAdmin || HasLegionClaim(PERMISSION_ID_CLAIM_NAME, permission);
	}

	public bool HasAllPermissionClaims(params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin || permissions.All(permission => HasLegionClaim(PERMISSION_ID_CLAIM_NAME, permission));
	}

	public bool HasAnyPermissionClaim(params Guid[] permissions)
	{
		Throw.IfArgumentNull(permissions);

		return IsSuperAdmin || permissions.Any(permission => HasLegionClaim(PERMISSION_ID_CLAIM_NAME, permission));
	}

	public override void AddClaim(Claim claim)
	{
		Throw.IfArgumentNull(claim);

		var newClaim = new Claim(
				claim.Type,
				claim.Value,
				claim.ValueType,
				ISSUER,
				ISSUER,
				claim.Subject);

		RemoveClaimSafe(newClaim);

		base.AddClaim(newClaim);
	}

	public override void AddClaims(IEnumerable<Claim?> claims)
	{
		Throw.IfArgumentNull(claims);

		var newClaims = claims
			.Where(claim => claim != null)
			.Select(claim =>
				new Claim(
					claim!.Type,
					claim.Value,
					claim.ValueType,
					ISSUER,
					ISSUER,
					claim.Subject));

		foreach (var newClaim in newClaims.Where(c => c != null))
			RemoveClaimSafe(newClaim);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
		base.AddClaims(newClaims);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
	}

	protected void AddClaims(string type, IEnumerable<string> values)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(values);

		var claims = values
			.Select(v => new Claim(type, v, null, ISSUER))
			.ToList();

		foreach (var claim in claims.Where(c => c != null))
			RemoveClaimSafe(claim);

		base.AddClaims(claims);
	}

	protected void AddClaims(string type, IEnumerable<Guid> values)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(values);

		var claims = values
			.Select(v => new Claim(type, v.ToString()!, null, ISSUER));

		foreach (var claim in claims.Where(c => c != null))
			RemoveClaimSafe(claim);

		base.AddClaims(claims);
	}

	public bool RemoveClaimSafe(Claim claim)
	{
		if (claim == null)
			return false;

		var foundClaims = FindAll(c =>
			string.Equals(c.Type, claim.Type, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(c.Value, claim.Value, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(c.ValueType, claim.ValueType, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(c.Issuer, claim.Issuer, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(c.OriginalIssuer, claim.OriginalIssuer, StringComparison.OrdinalIgnoreCase))
			.ToList();

		foreach (var foundClaim in foundClaims)
			base.RemoveClaim(foundClaim);

		return foundClaims.Any();
	}

	public Claim? FindFirstClaim(string type, string value)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(value))
			return null;

		foreach (Claim claim in Claims)
			if (claim != null
				&& string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase)
				&& string.Equals(claim.Value, value, StringComparison.Ordinal))
				return claim;

		return null;
	}

	public bool HasClaim(string type)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type))
			return false;

		foreach (Claim claim in Claims)
			if (claim != null
				&& string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase))
				return true;

		return false;
	}

	public static bool IsLegionClaim(Claim claim)
	{
		Throw.IfArgumentNull(claim);

		return
			string.Equals(claim.Issuer, ISSUER, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(claim.OriginalIssuer, ISSUER, StringComparison.OrdinalIgnoreCase);
	}

	public IEnumerable<Claim?> FindAllLegionClaims(string type)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type))
			yield return null;
		else
			foreach (Claim claim in Claims)
				if (claim != null)
					if (string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase) && IsLegionClaim(claim))
						yield return claim;
	}

	public IEnumerable<Claim> FindAllLegionClaims(Predicate<Claim> match)
	{
		Throw.IfArgumentNull(match);

		foreach (Claim claim in Claims)
			if (match(claim) && IsLegionClaim(claim))
				yield return claim;
	}

	public Claim? FindFirstLegionClaim(Predicate<Claim> match)
	{
		Throw.IfArgumentNull(match);

		foreach (Claim claim in Claims)
			if (match(claim) && IsLegionClaim(claim))
				return claim;

		return null;
	}

	public Claim? FindFirstLegionClaim(string type)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type))
			return null;

		foreach (Claim claim in Claims)
			if (claim != null
				&& string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase)
				&& IsLegionClaim(claim))
				return claim;

		return null;
	}

	public Claim? FindFirstLegionClaim(string type, string value)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(value))
			return null;

		foreach (Claim claim in Claims)
			if (claim != null
				&& string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase)
				&& string.Equals(claim.Value, value, StringComparison.Ordinal)
				&& IsLegionClaim(claim))
				return claim;

		return null;
	}

	public bool HasLegionClaim(string type)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type))
			return false;

		foreach (Claim claim in Claims)
			if (claim != null
				&& string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase)
				&& IsLegionClaim(claim))
				return true;

		return false;
	}

	public bool HasLegionClaim(string type, string value)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(value);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(value))
			return false;

		foreach (Claim claim in Claims)
			if (claim != null
				&& string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase)
				&& string.Equals(claim.Value, value, StringComparison.Ordinal)
				&& IsLegionClaim(claim))
				return true;

		return false;
	}

	public bool HasLegionClaim(string type, Guid value)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(type);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		if (string.IsNullOrWhiteSpace(type))
			return false;

		foreach (Claim claim in Claims)
			if (claim != null
				&& string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase)
				&& ConverterHelper.TryConvertFrom(claim.Value, out Guid GuidValue) && GuidValue.Equals(value)
				&& IsLegionClaim(claim))
				return true;

		return false;
	}

	public bool HasLegionClaim(Predicate<Claim> match)
	{
		Throw.IfArgumentNull(match);

		foreach (Claim claim in Claims)
			if (match(claim) && IsLegionClaim(claim))
				return true;

		return false;
	}
}
