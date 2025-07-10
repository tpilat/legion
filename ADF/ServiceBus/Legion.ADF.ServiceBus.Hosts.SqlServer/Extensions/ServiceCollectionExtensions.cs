using Legion.ADF.ServiceBus.Hosts.SqlServer;
using Legion.Database.SqlServer.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ServiceBus.Hosts.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFServiceBusBuilder ConfigureHostsSqlServer(this ADFServiceBusBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(HostsDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddSqlServerServices();
		builder.Services.AddSqlServerConnectionProvider<Legion.ADF.ServiceBus.ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IHostsUnitOfWork>(efConnectionProvider => new HostsUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IHostsQueryUnitOfWork>(efConnectionProvider => new HostsQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IHostsUnitOfWorkFactory, HostsUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IHostsQueryUnitOfWorkFactory, HostsQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<HostsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IHostsDbContext, HostsDbContext>();
		builder.Services.AddDbContext<HostsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IHostsQueryDbContext, HostsQueryDbContext>();
		builder.Services.TryAddSingleton<Hosts.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Hosts.IQueryTableInfoProvider, QueryTableInfoProvider>();

		builder
			.ConfigureHosts();

		return builder;
	}
}
