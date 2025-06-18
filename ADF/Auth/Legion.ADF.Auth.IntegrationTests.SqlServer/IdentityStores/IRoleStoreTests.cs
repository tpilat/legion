using Legion.ADF.Auth.IntegrationTests.Queries.Roles;
using Legion.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.IdentityStores;

[Category("IRoleStore tests")]
public class IRoleStoreTests : TestBase
{
	[Test]
	public async Task IdentityRoleStore_ShouldCreateRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var query = new Auth.Queries.Role.GetRoleByIdQuery(role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data?.IdRole == role.IdRole);
	}

	[Test]
	public async Task IdentityRoleStore_ShouldUpdateRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		role.SetName(scopeContext, "Test");
		role.SetNormalizedName(scopeContext, "TEST");

		var updateResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}");

		var query = new Auth.Queries.Role.GetRoleByIdQuery(role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data?.IdRole == role.IdRole && result.Data.Name == role.Name);
	}

	[Test]
	public async Task IdentityRoleStore_ShouldDeleteRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var query = new Auth.Queries.Role.GetRoleByIdQuery(role.IdRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data?.IdRole == role.IdRole);

		var identityResult = await roleStore.DeleteAsync(role, cancellationToken: default);

		if (!identityResult.Succeeded)
		{
			Assert.Fail($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}");
			return;
		}

		var result2 = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result2.Data == null);
	}

	[Test]
	public async Task IdentityRoleStore_ShouldGetRole()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var dbRole1 = await roleStore.FindByIdAsync(role.IdRole.ToString(), cancellationToken: default);

		Assert.That(role.IdRole, Is.EqualTo(dbRole1?.IdRole));

		var dbRole2 = await roleStore.FindByNameAsync(role.NormalizedName, cancellationToken: default);

		Assert.That(role.IdRole, Is.EqualTo(dbRole1?.IdRole));
	}

	[Test]
	public async Task IdentityRoleStore_QueryableRoleStore_ShouldGetQueryable()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var queryableRoleStore = roleStore as Microsoft.AspNetCore.Identity.IQueryableRoleStore<Model.Role>;
		Assert.That(queryableRoleStore, !Is.Null);

		var dbRole1 = await queryableRoleStore.Roles
			.Where(r => r.IdRole == role.IdRole)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.That(role.IdRole, Is.EqualTo(dbRole1?.IdRole));
	}

	[Test]
	public async Task IdentityRoleStore_RoleClaimStore_ShouldAddClaim()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var roleClaimStore = (roleStore as Microsoft.AspNetCore.Identity.IRoleClaimStore<Model.Role>)!;
		Assert.That(roleClaimStore, !Is.Null);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");
		await roleClaimStore.AddClaimAsync(role, claim, cancellationToken: default);

		var updateResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var foundClaims = await roleClaimStore.GetClaimsAsync(role);

		Assert.That(0 < foundClaims.Count && foundClaims.Any(u => u.Value == claim.Value));
	}

	[Test]
	public async Task IdentityRoleStore_RoleClaimStore_ShouldGetClaims()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var roleClaimStore = (roleStore as Microsoft.AspNetCore.Identity.IRoleClaimStore<Model.Role>)!;
		Assert.That(roleClaimStore, !Is.Null);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");
		await roleClaimStore.AddClaimAsync(role, claim, cancellationToken: default);

		var updateResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var foundClaims = await roleClaimStore.GetClaimsAsync(role);

		Assert.That(0 < foundClaims.Count && foundClaims.Any(u => u.Value == claim.Value));
	}

	[Test]
	public async Task IdentityRoleStore_RoleClaimStore_ShouldRemoveClaim()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var role = await RoleTests.CreateRole(scopeContext, null, roleStore);

		var roleClaimStore = (roleStore as Microsoft.AspNetCore.Identity.IRoleClaimStore<Model.Role>)!;
		Assert.That(roleClaimStore, !Is.Null);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");
		await roleClaimStore.AddClaimAsync(role, claim, cancellationToken: default);

		var updateResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var foundClaims = await roleClaimStore.GetClaimsAsync(role);

		Assert.That(0 < foundClaims.Count && foundClaims.Any(u => u.Value == claim.Value));

		await roleClaimStore.RemoveClaimAsync(role, claim, cancellationToken: default);

		var removeResult = await roleStore.UpdateAsync(role, cancellationToken: default);
		if (!removeResult.Succeeded)
			Assert.Fail($"{nameof(removeResult)} ERRORS: {string.Join(Environment.NewLine, removeResult.Errors)}", scopeContext);

		var remainedDBClaims = await roleClaimStore.GetClaimsAsync(role);

		Assert.That(remainedDBClaims?.Count, Is.EqualTo(0));
	}
}