using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IQueryableUserStore<Model.User>
{
	public IQueryable<Model.User> Users
		=> UoW.UserRepository.AsQueryable(ScopeContext.Create("Legion.ADF.Auth.Identity"), true);
}
