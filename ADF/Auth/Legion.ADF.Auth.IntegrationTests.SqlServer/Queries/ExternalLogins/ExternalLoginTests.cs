using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.ExternalLogins;

[Category("ExternalLogin tests")]
public class ExternalLoginTests : TestBase
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
	}

	[Test]
	public async Task Remove_ShouldGetExternalLogin_ByUserAndExternalIdentifier()
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

		var foundUser = await userLoginStore.FindByLoginAsync("Facebook", externalKey, cancellationToken: default);

		Assert.That(foundUser?.IdUser, Is.EqualTo(user.IdUser));

		var query = new Auth.Queries.ExternalLogin.GetExternalLoginByUserAndExternalIdentifierQuery(user.IdUser, "Facebook", externalKey, GlobalContext.Instance.UtcNow, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser && result.Data.IdLoginProvider == new Guid("00000003-0000-0000-0000-000000000000"));

		await userLoginStore.RemoveLoginAsync(
			user,
			"Facebook",
			externalKey,
			cancellationToken: default);

		updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		foundUser = await userLoginStore.FindByLoginAsync("Facebook", externalKey, cancellationToken: default);

		Assert.That(foundUser, Is.Null);
	}

	[Test]
	public async Task ShoudGetExternalLogins_ByUserId()
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

		var userLoginInfo = await userLoginStore.GetLoginsAsync(user, cancellationToken: default);

		Assert.That(userLoginInfo?.Count == 1 && userLoginInfo[0].LoginProvider == "Facebook" && userLoginInfo[0].ProviderKey == externalKey);

		var query = new Auth.Queries.ExternalLogin.GetExternalLoginsByUserIdQuery(user.IdUser, GlobalContext.Instance.UtcNow, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(el => el.IdUser == user.IdUser && el.IdLoginProvider == new Guid("00000003-0000-0000-0000-000000000000")));
	}
}