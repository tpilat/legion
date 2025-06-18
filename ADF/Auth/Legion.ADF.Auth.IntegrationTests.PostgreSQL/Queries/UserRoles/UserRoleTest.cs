using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.UserRoles;

[Category("UserRole tests")]
public class UserRoleTests : TestBase
{
	[Test]
	public async Task ShoudGetUserRole_ByIdUserAndIdRole()
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

		var query = new Auth.Queries.UserRole.GetUserRoleByIdUserAndIdRoleQuery(user.IdUser, role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data?.IdUser == user.IdUser && result.Data?.IdRole == role.IdRole);
	}

	[Test]
	public async Task Remove_ShouldGetUserRole_ByIdUserAndNormalizedRoleName()
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

		var query = new Auth.Queries.UserRole.GetUserRoleByIdUserAndNormalizedRoleNameQuery(user.IdUser, normalizedRoleName, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.DataWasSet && result.Data?.IdRole == role.IdRole);

		await userRoleStore.RemoveFromRoleAsync(user, normalizedRoleName, cancellationToken: default);

		updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var query2 = new Auth.Queries.UserRole.GetUserRoleByIdUserAndIdRoleQuery(user.IdUser, role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result2 = await messageBus.SendAsync(scopeContext, query2);

		Assert.That(!result2.HasError && result2.DataWasSet && result2.Data == null);
	}

	[Test]
	public async Task ShoudIsInRole()
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

		var query = new Auth.Queries.UserRole.IsInRoleQuery(user.IdUser, normalizedRoleName, false, CheckReadPermissions: true, AsNoTracking: true);
		var isInRoleResult = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!isInRoleResult.HasError && isInRoleResult.DataWasSet && isInRoleResult.Data == true);
	}
}