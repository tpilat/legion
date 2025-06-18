using Legion.ADF.Auth.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Auth.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFAuthBuilder ConfigurePostgreSQL(this ADFAuthBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(AuthDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddPostgreSQLServices();
		builder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.Auth.ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IAuthUnitOfWork>(efConnectionProvider => new AuthUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IAuthQueryUnitOfWork>(efConnectionProvider => new AuthQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IAuthUnitOfWorkFactory, AuthUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IAuthQueryUnitOfWorkFactory, AuthQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<AuthDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IAuthDbContext, AuthDbContext>();
		builder.Services.AddDbContext<AuthQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IAuthQueryDbContext, AuthQueryDbContext>();
		builder.Services.TryAddSingleton<Auth.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Auth.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
