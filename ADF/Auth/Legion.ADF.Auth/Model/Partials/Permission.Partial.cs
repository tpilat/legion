using System.Security.Claims;

namespace Legion.ADF.Auth.Model;

public sealed partial class Permission : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	internal Claim ToCLaim()
		=> string.IsNullOrWhiteSpace(ClaimValue)
			? new Claim(ClaimTypes.AuthorizationDecision, Code)
			: new Claim(ClaimTypes.AuthorizationDecision, ClaimValue);
}
