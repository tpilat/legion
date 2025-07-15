using Legion.ADF.Logs.Services;
using Legion.ADF.Logs.SqlServer;
using Legion.Database.SqlServer.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Logs.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFLogsBuilder ConfigureSqlServer(this ADFLogsBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(LogsDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddSqlServerServices();
		builder.Services.AddSqlServerConnectionProvider<ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<ILogsUnitOfWork>(efConnectionProvider => new LogsUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<ILogsQueryUnitOfWork>(efConnectionProvider => new LogsQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<ILogsUnitOfWorkFactory, LogsUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<ILogsQueryUnitOfWorkFactory, LogsQueryUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IUnitOfWorkFactory<ILogsUnitOfWork>, LogsUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<ILogsQueryUnitOfWork>, LogsQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<LogsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<ILogsDbContext, LogsDbContext>();
		builder.Services.AddDbContext<LogsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<ILogsQueryDbContext, LogsQueryDbContext>();
		builder.Services.TryAddSingleton<Logs.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Logs.IQueryTableInfoProvider, QueryTableInfoProvider>();

		builder.Services.TryAddSingleton<IADFLoggerStore, SqlServerADFLogger>();

		return builder;
	}
}
