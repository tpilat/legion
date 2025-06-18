using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.Database.PostgreSQL.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPostgreSQLServices(this IServiceCollection services)
	{
		services.TryAddSingleton<ITableInfoBulkInsertFactory, TableInfoBulkInsertFactory>();
		return services;
	}
}
