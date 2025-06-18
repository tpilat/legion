using Legion.ADF.Auth.Services.Authentication;
using Legion.ADF.Auth.Settings;
using Legion.Database;
using Legion.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace Legion.ADF.Auth.IntegrationTests.Authentication;

[Category("LegionPrincipalTransformation tests")]
public class LegionPrincipalTransformationTests : TestBase
{
	private Mock<IHttpContextAccessor> _httpContextAccessorMock;

	protected override void SetupTestInternal()
	{
		_httpContextAccessorMock = new Mock<IHttpContextAccessor>();
	}

	[Test]
	public async Task TransformAsync_ShouldReturnSamePrincipal_WhenUserIdIsNull()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var transformation = new LegionPrincipalTransformation(
			sp,
			sp.GetRequiredService<IConnectionProviderFactory>(),
			_httpContextAccessorMock.Object,
			sp.GetRequiredService<IOptions<IdentityStoreOptions>>(),
			sp.GetRequiredService<IOptions<IdentityOptions>>(),
			sp.GetRequiredService<ILoggerFactory>().CreateLogger<LegionPrincipalTransformation>());

		var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userid", "") }));

		var result = await transformation.TransformAsync(principal);

		Assert.That(result, Is.EqualTo(principal));
	}

	[Test]
	public async Task TransformAsync_ShouldReturnSamePrincipal_WhenUserIdIsInvalid()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var transformation = new LegionPrincipalTransformation(
			sp,
			sp.GetRequiredService<IConnectionProviderFactory>(),
			_httpContextAccessorMock.Object,
			sp.GetRequiredService<IOptions<IdentityStoreOptions>>(),
			sp.GetRequiredService<IOptions<IdentityOptions>>(),
			sp.GetRequiredService<ILoggerFactory>().CreateLogger<LegionPrincipalTransformation>());

		var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userid", "invalid-guid") }));

		var result = await transformation.TransformAsync(principal);

		Assert.That(result, Is.EqualTo(principal));
	}

	[Test]
	public async Task TransformAsync_ShouldReturnTransformedPrincipal_WhenUserExistsInDB()
	{
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var scopeContext = ScopeContext.Create("TEST");

		Guid? idUser;
		using (var userStore = sp.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Model.User>>())
		{
			var user = await Queries.Users.UserTests.CreateUser(scopeContext, null, userStore);
			idUser = user?.IdUser;
		}

		var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userid", idUser?.ToString() ?? "") }));

		sp = await SetUp.CreateScopedServiceProviderAsync();
		var transformation = new LegionPrincipalTransformation(
			sp,
			sp.GetRequiredService<IConnectionProviderFactory>(),
			_httpContextAccessorMock.Object,
			sp.GetRequiredService<IOptions<IdentityStoreOptions>>(),
			sp.GetRequiredService<IOptions<IdentityOptions>>(),
			sp.GetRequiredService<ILoggerFactory>().CreateLogger<LegionPrincipalTransformation>());

		var result = await transformation.TransformAsync(principal);

		Assert.That(result, Is.TypeOf<LegionPrincipal>());
	}
}
