using Legion.ADF.Cache.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Cache.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFCacheBuilder ConfigurePostgreSQL(this ADFCacheBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(CacheDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<ICacheUnitOfWork>(efConnectionProvider => new CacheUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<ICacheQueryUnitOfWork>(efConnectionProvider => new CacheQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<ICacheUnitOfWorkFactory, CacheUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<ICacheQueryUnitOfWorkFactory, CacheQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<CacheDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<ICacheDbContext, CacheDbContext>();
		builder.Services.AddDbContext<CacheQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<ICacheQueryDbContext, CacheQueryDbContext>();
		builder.Services.TryAddSingleton<Cache.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Cache.IQueryTableInfoProvider, QueryTableInfoProvider>();
		
		return builder;
	}
}
