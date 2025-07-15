using Legion.ADF.Audit.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Audit.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFAuditBuilder ConfigurePostgreSQL(this ADFAuditBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(AuditDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.Audit.ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IAuditUnitOfWork>(efConnectionProvider => new AuditUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IAuditQueryUnitOfWork>(efConnectionProvider => new AuditQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IAuditUnitOfWorkFactory, AuditUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IAuditQueryUnitOfWorkFactory, AuditQueryUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IUnitOfWorkFactory<IAuditUnitOfWork>, AuditUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<IAuditQueryUnitOfWork>, AuditQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<AuditDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IAuditDbContext, AuditDbContext>();
		builder.Services.AddDbContext<AuditQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IAuditQueryDbContext, AuditQueryDbContext>();
		builder.Services.TryAddSingleton<Audit.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Audit.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
