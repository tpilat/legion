using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityRoleStore : IQueryableRoleStore<Model.Role>
{
	public IQueryable<Model.Role> Roles
		=> UoW.RoleRepository.AsQueryable(ScopeContext.Create("Legion.ADF.Auth.Identity"), true);
}
