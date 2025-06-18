using Legion.ADF.Logs.PostgreSQL;
using Legion.ADF.Logs.Services;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Logs.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFLogsBuilder ConfigurePostgreSQL(this ADFLogsBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(LogsDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<ILogsUnitOfWork>(efConnectionProvider => new LogsUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<ILogsQueryUnitOfWork>(efConnectionProvider => new LogsQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<ILogsUnitOfWorkFactory, LogsUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<ILogsQueryUnitOfWorkFactory, LogsQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<LogsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<ILogsDbContext, LogsDbContext>();
		builder.Services.AddDbContext<LogsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<ILogsQueryDbContext, LogsQueryDbContext>();
		builder.Services.TryAddSingleton<Logs.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Logs.IQueryTableInfoProvider, QueryTableInfoProvider>();

		builder.Services.TryAddSingleton<IADFLoggerStore, PostgreSqlADFLogger>();

		return builder;
	}
}
