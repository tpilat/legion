using Legion.ADF.ServiceBus.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ServiceBus.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFServiceBusBuilder ConfigurePostgreSQL(this ADFServiceBusBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(ServiceBusDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.ServiceBus.ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IServiceBusUnitOfWork>(efConnectionProvider => new ServiceBusUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IServiceBusQueryUnitOfWork>(efConnectionProvider => new ServiceBusQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IServiceBusUnitOfWorkFactory, ServiceBusUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IServiceBusQueryUnitOfWorkFactory, ServiceBusQueryUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IUnitOfWorkFactory<IServiceBusUnitOfWork>, ServiceBusUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<IServiceBusQueryUnitOfWork>, ServiceBusQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<ServiceBusDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IServiceBusDbContext, ServiceBusDbContext>();
		builder.Services.AddDbContext<ServiceBusQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IServiceBusQueryDbContext, ServiceBusQueryDbContext>();
		builder.Services.TryAddSingleton<ServiceBus.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<ServiceBus.IQueryTableInfoProvider, QueryTableInfoProvider>();

		builder
			.ConfigureEnterpriseServiceBus();

		return builder;
	}

	public static ADFServiceBusMonitorBuilder ConfigurePostgreSQL(this ADFServiceBusMonitorBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(ServiceBusDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.ServiceBus.ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IServiceBusUnitOfWork>(efConnectionProvider => new ServiceBusUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IServiceBusQueryUnitOfWork>(efConnectionProvider => new ServiceBusQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IServiceBusUnitOfWorkFactory, ServiceBusUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IServiceBusQueryUnitOfWorkFactory, ServiceBusQueryUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IUnitOfWorkFactory<IServiceBusUnitOfWork>, ServiceBusUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<IServiceBusQueryUnitOfWork>, ServiceBusQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<ServiceBusDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IServiceBusDbContext, ServiceBusDbContext>();
		builder.Services.AddDbContext<ServiceBusQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IServiceBusQueryDbContext, ServiceBusQueryDbContext>();
		builder.Services.TryAddSingleton<ServiceBus.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<ServiceBus.IQueryTableInfoProvider, QueryTableInfoProvider>();

		builder
			.ConfigureServiceBusMonitor();

		return builder;
	}
}
