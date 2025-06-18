using Legion.Database;
using Legion.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.EntityFrameworkCore.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSqlServerConnectionProvider<TConnectionStringProvider>(
		this IServiceCollection services,
		Action<DbContextOptionsBuilder>? dbContextOptionsBuilder = null)
		where TConnectionStringProvider : class, IConnectionStringProvider
	{
		var sqlServerConnectionProviderFactory = new SqlServerConnectionProviderFactory(dbContextOptionsBuilder);

		services.TryAddSingleton<TConnectionStringProvider>();
		services.TryAddSingleton<IEFConnectionProviderFactory>(sqlServerConnectionProviderFactory);
		services.TryAddSingleton<IConnectionProviderFactory>(sqlServerConnectionProviderFactory);
		return services;
	}
}
