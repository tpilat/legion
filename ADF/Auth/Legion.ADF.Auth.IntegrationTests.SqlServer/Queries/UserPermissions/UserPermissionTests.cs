using Legion.ADF.Auth.IntegrationTests.Queries.Users;
using Legion.Database.SqlServer.Extensions;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth.IntegrationTests.Queries.UserPermissions;

[Category("UserPermission tests")]
public class UserPermissionTests : TestBase
{
	[Test]
	public async Task ShoudGetUserPermissions_ByIdUser()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var idPermission = Guid.NewGuid();
		var permissionName = $"TEST_{GetDatetimeTicks()}";
		using (var npgsqlConnection = new Microsoft.Data.SqlClient.SqlConnection(SetUp.ConncetionString))
		{
			npgsqlConnection.Open();

			using var cmd = new Microsoft.Data.SqlClient.SqlCommand("INSERT INTO auth.\"Permission\" (\"IdPermission\", \"Code\", \"Name\", \"Description\", \"ClaimValue\", \"IsSystemPermission\") VALUES(@idPermission, @permissionName, @permissionNormName, @permissionNormName, null, 0);", npgsqlConnection);
			cmd.Parameters.AddWithValue("@idPermission", System.Data.SqlDbType.UniqueIdentifier, idPermission);
			cmd.Parameters.AddWithValue("@permissionName", System.Data.SqlDbType.NVarChar, permissionName);
			cmd.Parameters.AddWithValue("@permissionNormName", System.Data.SqlDbType.NVarChar, permissionName.ToUpperInvariant());

			await cmd.ExecuteNonQueryAsync();
		}

		var newClaim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, permissionName);

		await userClaimStore.ReplaceClaimAsync(user, claim, newClaim, cancellationToken: default);

		updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var query = new Auth.Queries.UserPermission.GetUserPermissionsByIdUserQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(u => u.IdPermission == idPermission));
	}

	[Test]
	public async Task ShoudGetUserPermissions_ByIdUserAndClaimValue()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		var permissionQuery = new Auth.Queries.Permission.GetPermissionByClaimValueQuery("Access", CheckReadPermissions: true, AsNoTracking: true);
		var permissionResult = await messageBus.SendAsync(scopeContext, permissionQuery);

		Assert.That(permissionResult?.Data, !Is.Null);

		var query = new Auth.Queries.UserPermission.GetUserPermissionsByIdUserAndClaimValueQuery(user.IdUser, "Access", false, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && 0 < result.Data?.Count && result.Data.Any(up => up.IdPermission == permissionResult.Data.IdPermission));

		var query2 = new Auth.Queries.UserPermission.GetUserPermissionsByIdUserAndClaimValuesQuery(user.IdUser, ["Access"], false, CheckReadPermissions: true, AsNoTracking: true);
		var result2 = await messageBus.SendAsync(scopeContext, query2);

		Assert.That(!result2.HasError && 0 < result2.Data?.Count && result2.Data.Any(up => up.IdPermission == permissionResult.Data.IdPermission));
	}

	[Test]
	public async Task ShoudGetUserPermissions_ByIdUserAndClaimValues()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create("TEST");

		using var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>();

		var user = await UserTests.CreateUser(scopeContext, null, userStore);

		var claim = new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.AuthorizationDecision, "Access");

		var userClaimStore = userStore as Microsoft.AspNetCore.Identity.IUserClaimStore<Model.User>;
		Assert.That(userClaimStore, !Is.Null);

		await userClaimStore.AddClaimsAsync(user, [claim], cancellationToken: default);

		var idPermission = Guid.NewGuid();
		var permissionName = $"TEST_{GetDatetimeTicks()}";
		using (var npgsqlConnection = new Microsoft.Data.SqlClient.SqlConnection(SetUp.ConncetionString))
		{
			npgsqlConnection.Open();

			using var cmd = new Microsoft.Data.SqlClient.SqlCommand("INSERT INTO auth.\"Permission\" (\"IdPermission\", \"Code\", \"Name\", \"Description\", \"ClaimValue\", \"IsSystemPermission\") VALUES(@idPermission, @permissionName, @permissionNormName, @permissionNormName, null, 0);", npgsqlConnection);
			cmd.Parameters.AddWithValue("@idPermission", System.Data.SqlDbType.UniqueIdentifier, idPermission);
			cmd.Parameters.AddWithValue("@permissionName", System.Data.SqlDbType.NVarChar, permissionName);
			cmd.Parameters.AddWithValue("@permissionNormName", System.Data.SqlDbType.NVarChar, permissionName.ToUpperInvariant());

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

		updateResult = await userStore.UpdateAsync(user, cancellationToken: default);
		if (!updateResult.Succeeded)
			Assert.Fail($"{nameof(updateResult)} ERRORS: {string.Join(Environment.NewLine, updateResult.Errors)}", scopeContext);

		query = new Auth.Queries.UserPermission.GetUserPermissionsByIdUserQuery(user.IdUser, false, CheckReadPermissions: true, AsNoTracking: true);
		result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data?.Count == 0);
	}
}