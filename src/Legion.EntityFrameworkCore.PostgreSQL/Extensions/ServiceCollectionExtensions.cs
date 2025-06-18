using Legion.Database;
using Legion.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.EntityFrameworkCore.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPostgreSQLConnectionProvider<TConnectionStringProvider>(
		this IServiceCollection services,
		Action<DbContextOptionsBuilder>? dbContextOptionsBuilder = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var postgreSQLConnectionProviderFactory = new PostgreSQLConnectionProviderFactory(dbContextOptionsBuilder);

		services.TryAddSingleton<TConnectionStringProvider>();
		services.TryAddSingleton<IEFConnectionProviderFactory>(postgreSQLConnectionProviderFactory);
		services.TryAddSingleton<IConnectionProviderFactory>(postgreSQLConnectionProviderFactory);
		return services;
	}
}
