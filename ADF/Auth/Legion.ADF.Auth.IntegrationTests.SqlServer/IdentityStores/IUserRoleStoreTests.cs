using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.IdentityStores;

[Category("IUserRoleStore tests")]
public class IUserRoleStoreTests : TestBase
{
	[Test]
	public async Task IdentityUserRoleStore_UserRoleStore_ShouldAddToRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userRoleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>() as Microsoft.AspNetCore.Identity.IUserRoleStore<Model.User>;
		Assert.That(userRoleStore, !Is.Null);

		var user = await UserTests.CreateUser(scopeContext, null, userRoleStore);

		var roleName = "SuperAdmin";
		var normalizedRoleName = roleName.ToUpperInvariant();

		await userRoleStore.AddToRoleAsync(user, normalizedRoleName, cancellationToken: default);

		var updateResult = await userRoleStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}");

		var query = new Auth.Queries.Role.GetRolesByIdUserQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(r => r == roleName));
	}

	[Test]
	public async Task IdentityUserRoleStore_UserRoleStore_ShouldRemoveFromRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();
		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var normalizedRoleName = "SUPERADMIN";

		var userRoleStore = userStore as Microsoft.AspNetCore.Identity.IUserRoleStore<Model.User>;
		Assert.That(userRoleStore, !Is.Null);

		await userRoleStore.AddToRoleAsync(user, normalizedRoleName, cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var role = await roleStore.FindByNameAsync(normalizedRoleName, cancellationToken: default);
		Assert.That(role, !Is.Null);

		await userRoleStore.RemoveFromRoleAsync(user, normalizedRoleName, cancellationToken: default);

		var removeResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!removeResult.Succeeded)
			Assert.Fail($"{nameof(removeResult)} ERRORS: {string.Join(Environment.NewLine, removeResult.Errors)}", scopeContext);

		var query = new Auth.Queries.UserRole.GetUserRoleByIdUserAndIdRoleQuery(user.IdUser, role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.DataWasSet && result.Data == null);
	}

	[Test]
	public async Task IdentityUserRoleStore_UserRoleStore_ShouldGetRoles()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userRoleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>() as Microsoft.AspNetCore.Identity.IUserRoleStore<Model.User>;
		Assert.That(userRoleStore, !Is.Null);

		var user = await UserTests.CreateUser(scopeContext, null, userRoleStore);

		var roleName = "SuperAdmin";
		var normalizedRoleName = roleName.ToUpperInvariant();

		await userRoleStore.AddToRoleAsync(user, normalizedRoleName, cancellationToken: default);

		var updateResult = await userRoleStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}");

		var foundRoles = await userRoleStore.GetRolesAsync(
			user,
			cancellationToken: default);

		Assert.That(0 < foundRoles.Count && foundRoles.Any(r => r == roleName));
	}

	[Test]
	public async Task IdentityUserRoleStore_UserRoleStore_ShouldIsInRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();
		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var normalizedRoleName = "SUPERADMIN";

		var userRoleStore = userStore as Microsoft.AspNetCore.Identity.IUserRoleStore<Model.User>;
		Assert.That(userRoleStore, !Is.Null);

		await userRoleStore.AddToRoleAsync(user, normalizedRoleName, cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var result = await userRoleStore.IsInRoleAsync(user, normalizedRoleName, cancellationToken: default);

		Assert.That(result, Is.True);
	}

	[Test]
	public async Task IdentityUserRoleStore_UserRoleStore_ShouldGetUsersInRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

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
	}
}
