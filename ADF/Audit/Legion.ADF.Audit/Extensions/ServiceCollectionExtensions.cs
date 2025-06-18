using Legion.ADF.Audit.Services;
using Legion.ADF.Audit.Services.Internal;
using Legion.ADF.Audit.Settings;
using Legion.Extensions;
using Legion.Model.Audit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Audit.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFAuditBuilder AddADFAudit(
		this IServiceCollection services,
		IConfiguration? configuration = null)
	{
		//settings / options
		services.AddAppSettings();

		Assembly[] assemblies = [
			typeof(ADFAuditBuilder).Assembly,
			typeof(AuditStore).Assembly
		];

		//Add all validators from Legion.ADF.Audit.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		services.TryAddSingleton<IAuditEntryStoreFactory, AuditEntryStoreFactory>();
		services.TryAddTransient<AuditStore>();
		services.TryAddTransient<Legion.Model.Audit.IAuditEntryStore, AuditStore>();

		return new ADFAuditBuilder(services, configuration);
	}
}
