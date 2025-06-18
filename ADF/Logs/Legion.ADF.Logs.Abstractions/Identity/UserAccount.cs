namespace Legion.ADF.Logs;

public class UserAccount : IUserAccount
{
	public bool IsAuthenticated { get; private set; }
	public bool IsSuperAdmin { get; private set; }
	public Guid IdUser { get; private set; }
	public string Login { get; }

	public IReadOnlyList<Guid> Roles { get; internal set; }
	public IReadOnlyList<Guid> Permissions { get; internal set; }

	public UserAccount(
		string login,
		List<Guid> roles,
		bool isSuperAdmin,
		List<Guid> permissions)
	{
		Login = !string.IsNullOrWhiteSpace(login)
			? login
			: throw new ArgumentNullException(nameof(login));

		Roles = new List<Guid>();
		Permissions = new List<Guid>();

		IsAuthenticated = 0 < roles.Count;
		IsSuperAdmin = isSuperAdmin;

		if (roles != null)
			Roles = roles;

		if (permissions != null)
			Permissions = permissions;
	}

	public void SetUserId(Guid idUser)
	{
		Throw.IfDefault(IdUser); //TODO: message , $"{nameof(IdUser)} has been already set to {IdUser} | New parameter {idUser}");
		IdUser = idUser;
	}

	public bool IsInRole(Guid idRole)
	{
		if (IsSuperAdmin)
			return true;

		return Roles.Contains(idRole);
	}

	public bool IsInAnyRole(IEnumerable<Guid> idRoles)
	{
		if (IsSuperAdmin)
			return true;

		if (idRoles == null || !idRoles.Any())
			return false;

		return idRoles.Any(x => Roles.Contains(x));
	}

	public bool IsInAllRoles(IEnumerable<Guid> idRoles)
	{
		if (IsSuperAdmin)
			return true;

		if (idRoles == null || !idRoles.Any())
			return false;

		return idRoles.All(x => Roles.Contains(x));
	}

	public bool HasPermission(Guid idPermission)
	{
		if (IsSuperAdmin)
			return true;

		return Permissions.Contains(idPermission);
	}

	public bool HasAnyPermission(IEnumerable<Guid> idPermissions)
	{
		if (IsSuperAdmin)
			return true;

		if (idPermissions == null || !idPermissions.Any())
			return false;

		return idPermissions.Any(x => Permissions.Contains(x));
	}

	public bool HasAllPermissions(IEnumerable<Guid> idPermissions)
	{
		if (IsSuperAdmin)
			return true;

		if (idPermissions == null || !idPermissions.Any())
			return false;

		return idPermissions.All(x => Permissions.Contains(x));
	}
}
