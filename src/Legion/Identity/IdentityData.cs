namespace Legion.Identity;

public class IdentityData
{
	public static readonly Guid NO_TENANT = Guid.Empty;

	public Guid IdUser { get; set; }
	public string Login { get; set; }
	public string DisplayName { get; set; }
	public object? UserData { get; set; }
	public bool IsSuperAdmin { get; set; }

	public Guid? CurrentIdTenant { get; set; }

	public List<Guid>? Tenants { get; set; }

	/// <summary>
	/// Dictionary&lt;IdTenant, List&lt;IdRole&gt;&gt;
	/// </summary>
	public Dictionary<Guid, List<Guid>>? RoleIds { get; set; }

	/// <summary>
	/// Dictionary&lt;IdTenant, List&lt;IdPermission&gt;&gt;
	/// </summary>
	public Dictionary<Guid, List<Guid>>? PermissionIds { get; set; }

	public string? Password { get; set; }
	public string? Salt { get; set; }

	public override string? ToString()
	{
		return $"{nameof(IdUser)} = {IdUser} | {nameof(Login)} = {Login}";
	}
}
