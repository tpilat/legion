using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ESB.MBox.PostgreSQL.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMBoxContexts(this IServiceCollection services)
	{
		services.AddLogging();
		services.AddPostgreSQLServices();
		services.AddPostgreSQLConnectionProvider<ConnectionStringProvider>();
		services.AddUnitOfWork<IMBoxUnitOfWork>(unitOfWorkContext => new MBoxUnitOfWork(unitOfWorkContext));
		services.AddQueryUnitOfWork<IMBoxQueryUnitOfWork>(unitOfWorkContext => new MBoxQueryUnitOfWork(unitOfWorkContext));
		services.AddDbContext<MBoxDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IMBoxDbContext, MBoxDbContext>();
		services.AddDbContext<MBoxQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		services.TryAddTransient<IMBoxQueryDbContext, MBoxQueryDbContext>();
		services.TryAddSingleton<MBox.ITableInfoProvider, TableInfoProvider>();
		services.TryAddSingleton<MBox.IQueryTableInfoProvider, QueryTableInfoProvider>();
		return services;
	}
}
