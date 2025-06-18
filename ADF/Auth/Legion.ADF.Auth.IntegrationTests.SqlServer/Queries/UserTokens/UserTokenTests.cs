using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.UserTokens;

[Category("UserToken tests")]
public class UserTokenTests : TestBase
{
	[Test]
	public async Task ShoudGetUserToken_ByUserProviderTokenName()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var userAuthenticationTokenStore = userStore as Microsoft.AspNetCore.Identity.IUserAuthenticationTokenStore<Model.User>;
		Assert.That(userAuthenticationTokenStore, !Is.Null);

		var tokenName = $"name_{GetDatetimeTicks()}";
		var tokenValue = $"value_{GetDatetimeTicks()}";
		await userAuthenticationTokenStore.SetTokenAsync(user, "Facebook", tokenName, tokenValue, cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var dbTokenValue = await userAuthenticationTokenStore.GetTokenAsync(user, "Facebook", tokenName, cancellationToken: default);
		Assert.That(tokenValue, Is.EqualTo(dbTokenValue));

		var query = new Auth.Queries.UserToken.GetUserTokenByUserProviderTokenNameQuery(user.IdUser, "Facebook", tokenName, GlobalContext.Instance.UtcNow, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);
	}
}