using Legion.ADF.Auth.SqlServer;
using Legion.Database.SqlServer.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Auth.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFAuthBuilder ConfigureSqlServer(this ADFAuthBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.Services.AddInMemoryMessageBus([typeof(AuthDbContext).Assembly]);

		builder.Services.AddLogging();
		builder.Services.AddSqlServerServices();
		builder.Services.AddSqlServerConnectionProvider<ConnectionStringProvider>();
		builder.Services.AddUnitOfWork<IAuthUnitOfWork>(efConnectionProvider => new AuthUnitOfWork(efConnectionProvider));
		builder.Services.AddQueryUnitOfWork<IAuthQueryUnitOfWork>(efConnectionProvider => new AuthQueryUnitOfWork(efConnectionProvider));
		builder.Services.TryAddSingleton<IAuthUnitOfWorkFactory, AuthUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IAuthQueryUnitOfWorkFactory, AuthQueryUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IUnitOfWorkFactory<IAuthUnitOfWork>, AuthUnitOfWorkFactory>();
		builder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<IAuthQueryUnitOfWork>, AuthQueryUnitOfWorkFactory>();
		builder.Services.AddDbContext<AuthDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IAuthDbContext, AuthDbContext>();
		builder.Services.AddDbContext<AuthQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.Services.TryAddTransient<IAuthQueryDbContext, AuthQueryDbContext>();
		builder.Services.TryAddSingleton<Auth.ITableInfoProvider, TableInfoProvider>();
		builder.Services.TryAddSingleton<Auth.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
