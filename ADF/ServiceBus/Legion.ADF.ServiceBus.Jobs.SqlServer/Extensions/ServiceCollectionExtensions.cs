using Legion.ADF.ServiceBus.Jobs.SqlServer;
using Legion.Database.SqlServer.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ServiceBus.Jobs.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFServiceBusBuilder ConfigureJobsSqlServer(this ADFServiceBusBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(JobsDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddSqlServerServices();
		builder.Services.AddSqlServerConnectionProvider<Legion.ADF.ServiceBus.ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IJobsUnitOfWork>(efConnectionProvider => new JobsUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IJobsQueryUnitOfWork>(efConnectionProvider => new JobsQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IJobsUnitOfWorkFactory, JobsUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IJobsQueryUnitOfWorkFactory, JobsQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<JobsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IJobsDbContext, JobsDbContext>();
		builder.Services.AddDbContext<JobsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IJobsQueryDbContext, JobsQueryDbContext>();
		builder.Services.TryAddSingleton<Jobs.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Jobs.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
