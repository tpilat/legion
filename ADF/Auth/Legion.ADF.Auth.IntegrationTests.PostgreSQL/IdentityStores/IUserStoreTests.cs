using Legion.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.IdentityStores;

[Category("IUserStore tests")]
public class IUserStoreTests : TestBase
{
	[Test]
	public async Task IdentityUserOnlyStore_ShouldCreateUser()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);
	}

	[Test]
	public async Task IdentityUserOnlyStore_ShouldUpdateUser()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		user.SetEmail(scopeContext, "test2@test2.test2");

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.Email == user.Email);
	}

	[Test]
	public async Task IdentityUserOnlyStore_ShouldDeleteUser()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);

		var identityResult = await userStore.DeleteAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var result2 = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result2.HasError && result2.Data == null);
	}

	[Test]
	public async Task IdentityUserOnlyStore_ShouldFindById()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var dbUser = await userStore.FindByIdAsync(user.IdUser.ToString(), cancellationToken: default);

		Assert.That(user.IdUser, Is.EqualTo(dbUser?.IdUser));
	}

	[Test]
	public async Task IdentityUserOnlyStore_ShouldFindByName()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var dbUser = await userStore.FindByNameAsync(user.NormalizedLogin!, cancellationToken: default);

		Assert.That(user.IdUser, Is.EqualTo(dbUser?.IdUser));
	}

	[Test]
	public async Task IdentityUserOnlyStore_QueryableUserStore_ShouldGetQueryable()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var queryableUserStore = userStore as Microsoft.AspNetCore.Identity.IQueryableUserStore<Model.User>;
		Assert.That(queryableUserStore, !Is.Null);

		var dbRole1 = await queryableUserStore.Users
			.Where(r => r.IdUser == user.IdUser)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.That(user.IdUser, Is.EqualTo(dbRole1?.IdUser));

		GC.Collect();
		GC.WaitForPendingFinalizers();

		var objectsLifetimes = Trackers.ObjectLifetimeTracker.GetObjectsLifetimeStatus();
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserAuthenticationTokenStore_ShouldGetToken()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

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
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserAuthenticationTokenStore_ShouldSetToken()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

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
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserAuthenticationTokenStore_ShouldRemoveToken()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

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

		await userAuthenticationTokenStore.RemoveTokenAsync(user, "Facebook", tokenName, cancellationToken: default);

		var updateResult2 = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult2.Succeeded)
			Assert.Fail($"{nameof(updateResult2)} ERRORS: {string.Join(Environment.NewLine, updateResult2.Errors)}", scopeContext);

		var dbTokenValue2 = await userAuthenticationTokenStore.GetTokenAsync(user, "Facebook", tokenName, cancellationToken: default);
		Assert.That(dbTokenValue2, Is.Null);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserAuthenticatorKeyStore_ShouldGetAuthenticatorKey()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userAuthenticatorKeyStore = userStore as Microsoft.AspNetCore.Identity.IUserAuthenticatorKeyStore<Model.User>;
		Assert.That(userAuthenticatorKeyStore, !Is.Null);

		var tokenValue = $"value_{GetDatetimeTicks()}";
		await userAuthenticatorKeyStore.SetAuthenticatorKeyAsync(user, tokenValue, cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var dbTokenValue = await userAuthenticatorKeyStore.GetAuthenticatorKeyAsync(user, cancellationToken: default);
		Assert.That(tokenValue, Is.EqualTo(dbTokenValue));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserAuthenticatorKeyStore_ShouldSetAuthenticatorKey()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userAuthenticatorKeyStore = userStore as Microsoft.AspNetCore.Identity.IUserAuthenticatorKeyStore<Model.User>;
		Assert.That(userAuthenticatorKeyStore, !Is.Null);

		var tokenValue = $"value_{GetDatetimeTicks()}";
		await userAuthenticatorKeyStore.SetAuthenticatorKeyAsync(user, tokenValue, cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var dbTokenValue = await userAuthenticatorKeyStore.GetAuthenticatorKeyAsync(user, cancellationToken: default);
		Assert.That(tokenValue, Is.EqualTo(dbTokenValue));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserClaimStore_ShouldAddClaims()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var query = new Auth.Queries.User.GetUsersByClaimValueQuery(claim.Value, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(u => u.IdUser == user.IdUser));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserClaimStore_ShouldGetClaims()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var claims = await userClaimStore.GetClaimsAsync(
			user,
			cancellationToken: default);

		Assert.That(0 < claims.Count && claims.Any(c => c.Value == claim.Value));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserClaimStore_ShouldGetUsersForClaim()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var dbUsers = await userClaimStore.GetUsersForClaimAsync(
			claim,
			cancellationToken: default);

		Assert.That(0 < dbUsers.Count && dbUsers.Any(u => u.IdUser == user.IdUser));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserClaimStore_ShouldRemoveClaims()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var idPermission = Guid.NewGuid();
		var permissionName = $"TEST_{GetDatetimeTicks()}";
		using (var npgsqlConnection = new Npgsql.NpgsqlConnection(SetUp.ConncetionString))
		{
			npgsqlConnection.Open();

			using var cmd = new Npgsql.NpgsqlCommand("INSERT INTO auth.\"Permission\" (\"IdPermission\", \"Code\", \"Name\", \"Description\", \"ClaimValue\", \"IsSystemPermission\") VALUES(@idPermission, @permissionName, @permissionNormName, @permissionNormName, null, false);", npgsqlConnection);
			cmd.Parameters.AddWithValue("@idPermission", NpgsqlTypes.NpgsqlDbType.Uuid, idPermission);
			cmd.Parameters.AddWithValue("@permissionName", NpgsqlTypes.NpgsqlDbType.Varchar, permissionName);
			cmd.Parameters.AddWithValue("@permissionNormName", NpgsqlTypes.NpgsqlDbType.Varchar, permissionName.ToUpperInvariant());

			await cmd.ExecuteNonQueryAsync();
		}

		var newClaim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, permissionName);

		await userClaimStore.AddClaimsAsync(user, [newClaim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var query = new Auth.Queries.UserPermission.GetUserPermissionsByIdUserQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data?.Count == 2);

		await userClaimStore.RemoveClaimsAsync(user, [claim, newClaim], cancellationToken: default);

		var removeResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!removeResult.Succeeded)
			Assert.Fail($"{nameof(removeResult)} ERRORS: {string.Join(Environment.NewLine, removeResult.Errors)}", scopeContext);

		query = new Auth.Queries.UserPermission.GetUserPermissionsByIdUserQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data?.Count == 0);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserClaimStore_ShouldReplaceClaim()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var idPermission = Guid.NewGuid();
		var permissionName = $"TEST_{GetDatetimeTicks()}";
		using (var npgsqlConnection = new Npgsql.NpgsqlConnection(SetUp.ConncetionString))
		{
			npgsqlConnection.Open();

			using var cmd = new Npgsql.NpgsqlCommand("INSERT INTO auth.\"Permission\" (\"IdPermission\", \"Code\", \"Name\", \"Description\", \"ClaimValue\", \"IsSystemPermission\") VALUES(@idPermission, @permissionName, @permissionNormName, @permissionNormName, null, false);", npgsqlConnection);
			cmd.Parameters.AddWithValue("@idPermission", NpgsqlTypes.NpgsqlDbType.Uuid, idPermission);
			cmd.Parameters.AddWithValue("@permissionName", NpgsqlTypes.NpgsqlDbType.Varchar, permissionName);
			cmd.Parameters.AddWithValue("@permissionNormName", NpgsqlTypes.NpgsqlDbType.Varchar, permissionName.ToUpperInvariant());

			await cmd.ExecuteNonQueryAsync();
		}

		var newClaim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, permissionName);

		await userClaimStore.ReplaceClaimAsync(user, claim, newClaim, cancellationToken: default);

		var replaceClaimResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!replaceClaimResult.Succeeded)
			Assert.Fail($"{nameof(replaceClaimResult)} ERRORS: {string.Join(Environment.NewLine, replaceClaimResult.Errors)}", scopeContext);

		var query = new Auth.Queries.UserPermission.GetUserPermissionsByIdUserQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(u => u.IdPermission == idPermission));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserEmailStore_ShouldFindByEmail()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userEmailStore = userStore as Microsoft.AspNetCore.Identity.IUserEmailStore<Model.User>;
		Assert.That(userEmailStore, !Is.Null);

		var dbUser = await userEmailStore.FindByEmailAsync(user.NormalizedEmail!, cancellationToken: default);

		Assert.That(user.IdUser, Is.EqualTo(dbUser?.IdUser));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLockoutStore_ShouldIncrementAccessFailedCount()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userLockoutStore = userStore as Microsoft.AspNetCore.Identity.IUserLockoutStore<Model.User>;
		Assert.That(userLockoutStore, !Is.Null);

		await userLockoutStore.IncrementAccessFailedCountAsync(user, cancellationToken: default);

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser && result.Data.AccessFailedCount == 1);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLockoutStore_ShouldResetAccessFailedCount()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userLockoutStore = userStore as Microsoft.AspNetCore.Identity.IUserLockoutStore<Model.User>;
		Assert.That(userLockoutStore, !Is.Null);

		await userLockoutStore.IncrementAccessFailedCountAsync(user, cancellationToken: default);

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser && result.Data.AccessFailedCount == 1);

		await userLockoutStore.ResetAccessFailedCountAsync(user, cancellationToken: default);

		identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser && result.Data.AccessFailedCount == 0);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLockoutStore_ShouldSetLockoutEnabled()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userLockoutStore = userStore as Microsoft.AspNetCore.Identity.IUserLockoutStore<Model.User>;
		Assert.That(userLockoutStore, !Is.Null);

		await userLockoutStore.SetLockoutEnabledAsync(user, true, cancellationToken: default);

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser && result.Data.LockoutEnabled == true);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLockoutStore_ShouldSetLockoutEndDate()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userLockoutStore = userStore as Microsoft.AspNetCore.Identity.IUserLockoutStore<Model.User>;
		Assert.That(userLockoutStore, !Is.Null);

		var lockoutEndDate = DateTimeOffset.UtcNow.AddDays(1);
		await userLockoutStore.SetLockoutEndDateAsync(user, lockoutEndDate, cancellationToken: default);

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdUser == user.IdUser);

		var lockoutEndDateTicks = lockoutEndDate.UtcDateTime.ToString("yyyy.MM.dd HH:mm:ss.fffff");
		var resultLockoutEndDateTicks = result.Data!.LockoutEndUtc!.Value.ToString("yyyy.MM.dd HH:mm:ss.fffff");

		Assert.That(lockoutEndDateTicks, Is.EqualTo(resultLockoutEndDateTicks));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLoginStore_ShouldAddLogin()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

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
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLoginStore_ShouldFindByLogin()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

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
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLoginStore_ShouldGetLogins()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var externalKey = $"Facebook_user_key{GetDatetimeTicks()}";

		await userLoginStore.AddLoginAsync(
			user,
			new Microsoft.AspNetCore.Identity.UserLoginInfo("Facebook", externalKey, "Facebook"),
			cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var logins = await userLoginStore.GetLoginsAsync(user, cancellationToken: default);

		Assert.That(0 < logins.Count && logins.Any(uli => uli.LoginProvider == "Facebook" && uli.ProviderKey == externalKey));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserLoginStore_ShouldRemoveLogin()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

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

		var removeLoginResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!removeLoginResult.Succeeded)
			Assert.Fail($"{nameof(removeLoginResult)} ERRORS: {string.Join(Environment.NewLine, removeLoginResult.Errors)}", scopeContext);

		foundUser = await userLoginStore.FindByLoginAsync("Facebook", externalKey, cancellationToken: default);

		Assert.That(foundUser, Is.Null);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserPasswordStore_ShouldSetPasswordHash()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userPasswordStore = userStore as Microsoft.AspNetCore.Identity.IUserPasswordStore<Model.User>;
		Assert.That(userPasswordStore, !Is.Null);

		var hash = $"HASH_{GetDatetimeTicks()}";
		await userPasswordStore.SetPasswordHashAsync(user, hash, cancellationToken: default);

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.PasswordHash == hash);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserPhoneNumberStore_ShouldSetPhoneNumber()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userPhoneNumberStore = userStore as Microsoft.AspNetCore.Identity.IUserPhoneNumberStore<Model.User>;
		Assert.That(userPhoneNumberStore, !Is.Null);

		var phone = $"PHONE_{GetDatetimeTicks()}";
		await userPhoneNumberStore.SetPhoneNumberAsync(user, phone, cancellationToken: default);

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.PhoneNumber == phone);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserSecurityStampStore_ShouldSetSecurityStamp()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userSecurityStampStore = userStore as Microsoft.AspNetCore.Identity.IUserSecurityStampStore<Model.User>;
		Assert.That(userSecurityStampStore, !Is.Null);

		var securityStamp = $"HASH_{GetDatetimeTicks()}";
		await userSecurityStampStore.SetSecurityStampAsync(user, securityStamp, cancellationToken: default);

		var identityResult = await userStore.UpdateAsync(user, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.SecurityStamp == securityStamp);
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserTwoFactorRecoveryCodeStore_ShouldCountCodes()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userTwoFactorRecoveryCodeStore = userStore as Microsoft.AspNetCore.Identity.IUserTwoFactorRecoveryCodeStore<Model.User>;
		Assert.That(userTwoFactorRecoveryCodeStore, !Is.Null);

		var codes = new List<string> { $"CODE1_{GetDatetimeTicks()}", $"CODE2_{GetDatetimeTicks()}", $"CODE3_{GetDatetimeTicks()}" };
		await userTwoFactorRecoveryCodeStore.ReplaceCodesAsync(user, codes, cancellationToken: default);

		var updateCodesResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateCodesResult.Succeeded)
			Assert.Fail($"{nameof(updateCodesResult)} ERRORS: {string.Join(Environment.NewLine, updateCodesResult.Errors)}", scopeContext);

		var codesCount = await userTwoFactorRecoveryCodeStore.CountCodesAsync(user, cancellationToken: default);
		Assert.That(codesCount, Is.EqualTo(codes.Count));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserTwoFactorRecoveryCodeStore_ShouldRedeemCode()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userTwoFactorRecoveryCodeStore = userStore as Microsoft.AspNetCore.Identity.IUserTwoFactorRecoveryCodeStore<Model.User>;
		Assert.That(userTwoFactorRecoveryCodeStore, !Is.Null);

		var codes = new List<string> { $"CODE1_{GetDatetimeTicks()}", $"CODE2_{GetDatetimeTicks()}", $"CODE3_{GetDatetimeTicks()}" };
		await userTwoFactorRecoveryCodeStore.ReplaceCodesAsync(user, codes, cancellationToken: default);

		var updateCodesResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateCodesResult.Succeeded)
			Assert.Fail($"{nameof(updateCodesResult)} ERRORS: {string.Join(Environment.NewLine, updateCodesResult.Errors)}", scopeContext);

		var codesCount = await userTwoFactorRecoveryCodeStore.CountCodesAsync(user, cancellationToken: default);
		Assert.That(codesCount, Is.EqualTo(codes.Count));

		var applied = await userTwoFactorRecoveryCodeStore.RedeemCodeAsync(user, codes[0], cancellationToken: default);
		Assert.That(applied, Is.True);

		var redeemResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!redeemResult.Succeeded)
			Assert.Fail($"{nameof(redeemResult)} ERRORS: {string.Join(Environment.NewLine, redeemResult.Errors)}", scopeContext);

		codesCount = await userTwoFactorRecoveryCodeStore.CountCodesAsync(user, cancellationToken: default);
		Assert.That(codesCount, Is.EqualTo(codes.Count -1));
	}

	[Test]
	public async Task IdentityUserOnlyStore_UserTwoFactorStore_ShouldSetTwoFactorEnabled()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var userLoginStore = userStore as Microsoft.AspNetCore.Identity.IUserLoginStore<Model.User>;
		Assert.That(userLoginStore, !Is.Null);

		var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);

		var userTwoFactorStore = userStore as Microsoft.AspNetCore.Identity.IUserTwoFactorStore<Model.User>;
		Assert.That(userTwoFactorStore, !Is.Null);

		await userTwoFactorStore.SetTwoFactorEnabledAsync(user, true, cancellationToken: default);

		var updateCodesResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateCodesResult.Succeeded)
			Assert.Fail($"{nameof(updateCodesResult)} ERRORS: {string.Join(Environment.NewLine, updateCodesResult.Errors)}", scopeContext);

		var query = new Auth.Queries.User.GetUserByIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.MultiFactorEnabled == true);

		var enabled = await userTwoFactorStore.GetTwoFactorEnabledAsync(user, cancellationToken: default);
		Assert.That(!result.HasError && result.Data != null && result.Data.MultiFactorEnabled == enabled);
	}
}
