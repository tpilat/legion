using Legion.ADF.Auth.Identity;
using Legion.ADF.Auth.Services.Authentication;
using Legion.ADF.Auth.Settings;
using Legion.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Auth.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFAuthBuilder AddADFAuth(
		this IServiceCollection services,
		bool addRoles,
		Action<IdentityOptions>? identitySettings = null,
		Action<IdentityBuilder>? identityConfiguration = null,
		IConfiguration? configuration = null)
	{
		//settings / options
		services.AddAppSettings();

		Assembly[] assemblies = [
			typeof(ADFAuthBuilder).Assembly,
			typeof(IdentityUserOnlyStore).Assembly
		];

		//Add all validators from Legion.ADF.Auth.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		services.AddHttpContextAccessor();
		services.AddTransient<Microsoft.AspNetCore.Authentication.IClaimsTransformation, LegionPrincipalTransformation>();

		var identityBuilder = identitySettings == null
			? services.AddIdentityCore<Model.User>()
			: services.AddIdentityCore<Model.User>(identitySettings);

		if (addRoles)
		{
			identityBuilder.AddRoles<Model.Role>();
			identityBuilder.Services.TryAddScoped<IRoleStore<Model.Role>, Identity.IdentityRoleStore>();
			identityBuilder.Services.TryAddScoped<IUserStore<Model.User>, Identity.IdentityUserRoleStore>();
		}
		else
		{
			identityBuilder.Services.TryAddScoped<IUserStore<Model.User>, Identity.IdentityUserOnlyStore>();
		}

		identityConfiguration?.Invoke(identityBuilder);

		return new ADFAuthBuilder(services, configuration, addRoles);
	}
}
