using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.LoginProviders;

[Category("LoginProviderTests tests")]
public class LoginProviderTests : TestBase
{
	[Test]
	public async Task ShoudGetExternalLogin_ByExternalIdentifier()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var externalKey = $"Facebook_user_key{GetDatetimeTicks()}";

		await userLoginStore.AddLoginAsync(
			user,
			new Microsoft.AspNetCore.Identity.UserLoginInfo("Facebook", externalKey, "Facebook"),
			cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var query = new Auth.Queries.ExternalLogin.GetExternalLoginByExternalIdentifierQuery("Facebook", externalKey, GlobalContext.Instance.UtcNow, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);

		var loginProviderByNameQuery = new Auth.Queries.LoginProvider.GetLoginProviderByNameQuery("Facebook", false, CheckReadPermissions: true, AsNoTracking: true);
		var loginProviderByNameQueryResult = await messageBus.SendAsync(scopeContext, loginProviderByNameQuery);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);
	}
}