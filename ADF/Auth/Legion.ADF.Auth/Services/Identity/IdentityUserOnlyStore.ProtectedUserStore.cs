using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.Identity;

public partial class IdentityUserOnlyStore : IProtectedUserStore<Model.User>
{
}
