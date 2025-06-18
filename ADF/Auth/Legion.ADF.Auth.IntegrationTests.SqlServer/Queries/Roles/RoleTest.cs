using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.Roles;

[Category("Role tests")]
public class RoleTests : TestBase
{
	internal static async Task<Model.Role> CreateRole(
		IScopeContext scopeContext,
		string? roleName,
		Microsoft.AspNetCore.Identity.IRoleStore<Model.Role> roleStore)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (string.IsNullOrWhiteSpace(roleName))
			roleName = $"TEST_{GetDatetimeTicks()}";

		var createRoleResult = Model.Role.CreateRole(scopeContext, roleName, roleName.ToUpperInvariant());
		createRoleResult.ThrowIfErrorOrNullData(scopeContext, TestErrorCode, true);
		var role = createRoleResult.Data!;

		var identityResult = await roleStore.CreateAsync(role, default);
		if (identityResult.Succeeded)
		{
			return role;
		}
		else
		{
			Throw.InvalidOperationException($"{nameof(identityResult)} ERRORS: {string.Join(Environment.NewLine, identityResult.Errors)}", scopeContext);
			return null;
		}
	}

	[Test]
	public async Task Create_ShouldGetRole_ById()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var idRole = new Guid("00000001-0000-0000-0000-000000000000");

		var query = new Auth.Queries.Role.GetRoleByIdQuery(idRole, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.IdRole == idRole);

		await using var uow = CreateAuthUnitOfWork(scopeContext, sp);
		var roleById = await uow.RoleRepository
			.GetRoleById(new Auth.Queries.Role.GetRoleByIdQuery(idRole, false, CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(roleById?.IdRole, Is.EqualTo(idRole));
	}

	[Test]
	public async Task Create_ShouldGetRoleBy_NormalizedName()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var roleStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IRoleStore<Model.Role>>();

		var normalizedRoleName = "SUPERADMIN";

		var query = new Auth.Queries.Role.GetRoleByNormalizedNameQuery(normalizedRoleName, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.NormalizedName == normalizedRoleName);

		await using var uow = CreateAuthUnitOfWork(scopeContext, sp);
		var foundRole = await uow.RoleRepository
			.GetRoleByNormalizedName(new Auth.Queries.Role.GetRoleByNormalizedNameQuery(normalizedRoleName, false, CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(foundRole?.NormalizedName, Is.EqualTo(normalizedRoleName));
	}

	[Test]
	public async Task Create_ShouldGetRolesBy_IdUser()
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

		var query = new Auth.Queries.Role.GetRolesByIdUserQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(r => r == roleName));
	}
}