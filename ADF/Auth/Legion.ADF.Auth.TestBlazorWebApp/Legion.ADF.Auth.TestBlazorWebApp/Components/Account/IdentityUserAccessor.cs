using Microsoft.AspNetCore.Identity;

namespace Legion.ADF.Auth.TestBlazorWebApp.Components.Account;
internal sealed class IdentityUserAccessor(UserManager<Legion.ADF.Auth.Model.User> userManager, IdentityRedirectManager redirectManager)
{
	public async Task<Legion.ADF.Auth.Model.User> GetRequiredUserAsync(HttpContext context)
	{
		var user = await userManager.GetUserAsync(context.User);

		if (user is null)
		{
			redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
		}

		return user;
	}
}
