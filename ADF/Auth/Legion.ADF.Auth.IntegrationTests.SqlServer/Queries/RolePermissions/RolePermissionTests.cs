using Legion.ADF.Auth.IntegrationTests.Queries.Roles;
using Legion.MessageBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.RolePermissions;

[Category("RolePermission tests")]
public class RolePermissionTests : TestBase
{
	[Test]
	public async Task Create_ShouldGetRolePermissions_ByRoleIdAndClaimValue()
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

		var foundClaims = await roleClaimStore.GetClaimsAsync(role);

		Assert.That(0 < foundClaims.Count && foundClaims.Any(u => u.Value == claim.Value));

		var permissionQuery = new Auth.Queries.Permission.GetPermissionByClaimValueQuery(claim.Value, CheckReadPermissions: true, AsNoTracking: true);
		var permissionResult = await messageBus.SendAsync(scopeContext, permissionQuery);
		Assert.That(!permissionResult.HasError && permissionResult.Data?.Code == claim.Value);

		var query = new Auth.Queries.RolePermission.GetRolePermissionsByRoleIdAndClaimValueQuery(role.IdRole, claim.Value, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(rp => rp.IdPermission == permissionResult.Data!.IdPermission));

		await roleClaimStore.RemoveClaimAsync(role, claim, cancellationToken: default);

		var removeResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!removeResult.Succeeded)
			Assert.Fail($"{nameof(removeResult)} ERRORS: {string.Join(Environment.NewLine, removeResult.Errors)}", scopeContext);

		var remainedDBClaims = await roleClaimStore.GetClaimsAsync(role);

		Assert.That(remainedDBClaims?.Count, Is.EqualTo(0));
	}
}