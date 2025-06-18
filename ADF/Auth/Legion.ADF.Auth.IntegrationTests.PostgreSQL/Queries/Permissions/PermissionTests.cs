using Legion.ADF.Auth.IntegrationTests.Queries.Roles;
using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.MessageBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.Permissions;

[Category("Permission tests")]
public class PermissionTests : TestBase
{
	[Test]
	public async Task ShoudGetPermissions_ByRoleId()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var roleClaimStore = roleStore as IRoleClaimStore<Model.Role>;
		Assert.That(roleClaimStore, !Is.Null);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");
		await roleClaimStore.AddClaimAsync(role, claim, cancellationToken: default);

		var updateResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var query = new Auth.Queries.Permission.GetPermissionsByRoleIdQuery(role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(u => u.Code == "Access"));
	}

	[Test]
	public async Task ShoudGetUserPermissions_ByIdUser()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<IUserStore<Model.User>>();

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var claims = await userClaimStore.GetClaimsAsync(user, cancellationToken: default);

		Assert.That(0 < claims.Count && claims.Any(c => c.Type == claim.Type && c.Value == claim.Value));

		var query = new Auth.Queries.Permission.GetClaimsByUserIdQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(c => c.Type == claim.Type && c.Value == claim.Value));

		var query2 = new Auth.Queries.Permission.GetPermissionsByClaimValuesQuery([claim.Value], CheckReadPermissions: true, AsNoTracking: true);
		var result2 = await messageBus.SendAsync(scopeContext, query2);

		Assert.That(!result2.HasError && 0 < result2.Data?.Count && result2.Data.Any(p => p.Code == claim.Value));
	}

	[Test]
	public async Task ShoudGetPermissions_ByRoleIdAndClaimValue()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var roleClaimStore = (roleStore as IRoleClaimStore<Model.Role>)!;
		Assert.That(roleClaimStore, !Is.Null);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");
		await roleClaimStore.AddClaimAsync(role, claim, cancellationToken: default);

		var updateResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var query = new Auth.Queries.Permission.GetPermissionsByRoleIdAndClaimValueQuery(role.IdRole, claim.Value, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(id => id == new Guid("00000001-0000-0000-0000-000000000000")));

		var query2 = new Auth.Queries.Permission.GetClaimsByRoleIdQuery(role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result2 = await messageBus.SendAsync(scopeContext, query2);

		Assert.That(!result2.HasError && 0 < result2.Data?.Count && result2.Data.Any(c => c.Type == claim.Type && c.Value == claim.Value));
	}
}