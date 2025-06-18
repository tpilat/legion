using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.Users;

[Category("User tests")]
public class UserTests : TestBase
{
	internal static async Task<Model.User> CreateUser(
		IScopeContext scopeContext,
		string? login,
		Microsoft.AspNetCore.Identity.IUserStore<Model.User> userStore)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (string.IsNullOrWhiteSpace(login))
			login = $"TEST_{GetDatetimeTicks()}";

		var createUserResult = Model.User.CreateUser(scopeContext, login, login.ToUpperInvariant());
		createUserResult.ThrowIfErrorOrNullData(scopeContext, TestErrorCode, true);
		var user = createUserResult.Data!;

		var identityResult = await userStore.CreateAsync(user, default);
		if (identityResult.Succeeded)
		{
			return user;
		}
		else
		{
			Throw.InvalidOperationException($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}", scopeContext);
			return null;
		}
	}

	[Test]
	public async Task Create_ShouldGetUser_ByExternalLoginProviderIdentifier()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await CreateUser(scopeContext, null, userStore);

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

		var query = new Auth.Queries.User.GetUserByExternalLoginProviderIdentifierQuery("Facebook", externalKey, GlobalContext.Instance.UtcNow, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);
	}

	[Test]
	public async Task Create_ShouldGetUser_ById()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await CreateUser(scopeContext, null, userStore);

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);

		await using var uow = CreateAuthUnitOfWork(scopeContext, sp);
		var userById = await uow.UserRepository
			.GetUserById(new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(userById?.IdUser, Is.EqualTo(user.IdUser));
	}

	[Test]
	public async Task Create_ShouldGetUserBy_NormalizedEmail()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await CreateUser(scopeContext, null, userStore);

		var userEmailStore = userStore as Microsoft.AspNetCore.Identity.IUserEmailStore<Model.User>;
		Assert.That(userEmailStore, !Is.Null);

		await userEmailStore.SetEmailAsync(user, $"test{GetDatetimeTicks()}@test.sk", cancellationToken: default);
		await userEmailStore.SetNormalizedEmailAsync(user, user.Email!.ToUpperInvariant(), cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var foundUser = await userEmailStore.FindByEmailAsync(
			user.NormalizedEmail!,
			cancellationToken: default);

		Assert.That(foundUser?.IdUser, Is.EqualTo(user.IdUser));

		var query = new Auth.Queries.User.GetUserByNormalizedEmailQuery(user.NormalizedEmail!, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);
	}

	[Test]
	public async Task Create_ShouldGetUser_ByNormalizedLogin()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await CreateUser(scopeContext, null, userStore);

		await userStore.SetUserNameAsync(user, $"NEW_LOGIN_{GetDatetimeTicks()}", cancellationToken: default);
		await userStore.SetNormalizedUserNameAsync(user, user.Login!.ToUpperInvariant(), cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var foundUser = await userStore.FindByNameAsync(
			user.NormalizedLogin!,
			cancellationToken: default);

		Assert.That(foundUser?.IdUser, Is.EqualTo(user.IdUser));

		var query = new Auth.Queries.User.GetUserByNormalizedLoginQuery(user.NormalizedLogin!, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);
	}

	[Test]
	public async Task Create_ShouldGetUser_ByNormalizedRoleName()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await CreateUser(scopeContext, null, userStore);

		var normalizedRoleName = "SUPERADMIN";

		var userRoleStore = userStore as Microsoft.AspNetCore.Identity.IUserRoleStore<Model.User>;
		Assert.That(userRoleStore, !Is.Null);

		await userRoleStore.AddToRoleAsync(user, normalizedRoleName, cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var foundUsers = await userRoleStore.GetUsersInRoleAsync(
			normalizedRoleName,
			cancellationToken: default);

		Assert.That(0 < foundUsers.Count && foundUsers.Any(u => u.IdUser == user.IdUser));

		var query = new Auth.Queries.User.GetUserByNormalizedRoleNameQuery(normalizedRoleName, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(u => u.IdUser == user.IdUser));
	}

	[Test]
	public async Task Create_ShouldGetUsers_ByClaimValue()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var foundUsers = await userClaimStore.GetUsersForClaimAsync(
			claim,
			cancellationToken: default);

		Assert.That(0 < foundUsers.Count && foundUsers.Any(u => u.IdUser == user.IdUser));

		var query = new Auth.Queries.User.GetUsersByClaimValueQuery(claim.Value, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(u => u.IdUser == user.IdUser));
	}
}