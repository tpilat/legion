using Legion.ADF.Config.Configuration;
using Legion.ADF.Config.PostgreSQL;
using Legion.ADF.Config.PostgreSQL.Configuration.Internal;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Config.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFConfigBuilder ConfigurePostgreSQL(this ADFConfigBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(ConfigDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IConfigUnitOfWork>(efConnectionProvider => new ConfigUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IConfigQueryUnitOfWork>(efConnectionProvider => new ConfigQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IConfigUnitOfWorkFactory, ConfigUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IConfigQueryUnitOfWorkFactory, ConfigQueryUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IUnitOfWorkFactory<IConfigUnitOfWork>, ConfigUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<IConfigQueryUnitOfWork>, ConfigQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<ConfigDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IConfigDbContext, ConfigDbContext>();
		builder.Services.AddDbContext<ConfigQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IConfigQueryDbContext, ConfigQueryDbContext>();
		builder.Services.TryAddSingleton<Config.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Config.IQueryTableInfoProvider, QueryTableInfoProvider>();

		builder.Services.TryAddTransient<IDBConfigurationDataProvider, DBConfigurationDataProvider>();

		return builder;
	}
}
