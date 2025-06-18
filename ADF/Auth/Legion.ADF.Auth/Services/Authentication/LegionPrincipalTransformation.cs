using Legion.ADF.Auth.Events;
using Legion.ADF.Auth.Settings;
using Legion.Database;
using Legion.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Legion.ADF.Auth.Services.Authentication;

internal class LegionPrincipalTransformation : IClaimsTransformation
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IConnectionProviderFactory _connectionProviderFactory;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IdentityStoreOptions _identityStoreOptions;
	private readonly IdentityOptions _identityOptions;
	private readonly ILogger _logger;

	public LegionPrincipalTransformation(
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		IHttpContextAccessor httpContextAccessor,
		IOptions<IdentityStoreOptions> identityStoreOptions,
		IOptions<IdentityOptions> identityOptions,
		ILogger<LegionPrincipalTransformation> logger)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(identityStoreOptions);
		Throw.IfArgumentNull(identityOptions);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_connectionProviderFactory = connectionProviderFactory;
		_httpContextAccessor = httpContextAccessor;
		_identityStoreOptions = identityStoreOptions.Value;
		_identityOptions = identityOptions.Value;
		_logger = logger;
	}

	public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		var httpContext = _httpContextAccessor.HttpContext;

		var scopeContext = ScopeContext.Create(nameof(LegionPrincipalTransformation));
		_logger.LogTraceMessage(scopeContext, x => x.InternalMessage($"Transform request: {httpContext?.Request?.GetAbsoluteUri()}"));

		var userIdString = principal.FindFirstValue("userid");
		if (string.IsNullOrWhiteSpace(userIdString))
			return principal;

		if (!Guid.TryParse(userIdString, out var userId))
			return principal;

		await using var connectionProvider = _connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			_serviceProvider,
			_identityStoreOptions.IdentityStoreId,
			transactionIsolationLevel: null,
			false,
			_identityStoreOptions.EnableAuditing);

		var authUoWResult = connectionProvider.UnitOfWorkProvider.Create<IAuthUnitOfWork>(scopeContext);

		if (authUoWResult.HasError)
			authUoWResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Auth.Exceptions.Internal.ErrorCodes.AuthUnitOfWorkException.InvalidUoW, true);

		await using var authUoW = authUoWResult.Data!;

		var user = await authUoW.UserRepository
			.GetUserPermissionsAndRolesById(new Queries.User.GetValidUserPermissionsAndRolesByIdQuery(userId, CheckReadPermissions: false, AsNoTracking: true, DisableCahce: false))
			.ToResultAsync(scopeContext);

		if (user == null)
			return principal;

		var legionPrincipal = user.ToPrincipal(
			[],
			null,
			null,
			principal.Claims.ToList());

		_serviceProvider.AddApplicationEntryScopeContextPrincipal(legionPrincipal, true);

		connectionProvider.DomainEventStore?.AddDomainEvent(
			scopeContext,
			new LegionPrincipalTransformedEvent(
				legionPrincipal.IdentityBase?.IdUser ?? userId,
				legionPrincipal.IdentityBase?.Login ?? user.Login!),
			this.GetType().GetSimplifiedAssemblyQualifiedName(),
			nameof(TransformAsync));

		var saveResult = await authUoW.SaveAsync(scopeContext, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		return legionPrincipal;
	}
}

//TODO: move to new Legion.AspNetCore.Web library project
internal static class MyExtensions
{
	public static Uri? GetAbsoluteUri(this HttpRequest request)
	{
		if (request == null)
			return null;

		//string absoluteUri = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
		//Uri uri = new Uri(absoluteUri);
		//return uri;

		var hostComponents = request.Host.ToUriComponent().Split(':');

		var uriBuilder = new UriBuilder
		{
			Scheme = request.Scheme,
			Host = hostComponents[0], //request.Host.Host,
			//Port = request.Host.Port ?? (request.IsHttps ? 443 : 80),
			Path = string.Concat(request.PathBase.ToUriComponent(), request.Path.ToUriComponent()), //Request.PathBase reprezentuje VIRTUAL PATH = VirtualPath
			Query = request.QueryString.ToUriComponent()
		};

		if (hostComponents.Length == 2)
		{
			uriBuilder.Port = Convert.ToInt32(hostComponents[1]);
		}

		return uriBuilder.Uri;
	}
}
