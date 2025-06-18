namespace Legion.ADF.ESB;

public interface IUserAccount
{
	bool IsAuthenticated { get; }
	bool IsSuperAdmin { get; }
	Guid IdUser { get; }
	string Login { get; }

	IReadOnlyList<Guid> Roles { get; }
	IReadOnlyList<Guid> Permissions { get; }

	bool IsInRole(Guid idRole);

	bool IsInAnyRole(IEnumerable<Guid> idRoles);

	bool IsInAllRoles(IEnumerable<Guid> idRoles);

	bool HasPermission(Guid idPermission);

	bool HasAnyPermission(IEnumerable<Guid> idPermissions);

	bool HasAllPermissions(IEnumerable<Guid> idPermissions);
}
