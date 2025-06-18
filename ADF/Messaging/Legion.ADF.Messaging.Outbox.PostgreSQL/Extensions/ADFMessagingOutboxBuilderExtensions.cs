using Legion.ADF.Messaging.Outbox;
using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Messaging;

public static class ADFMessagingOutboxBuilderExtensions
{
	public static ADFMessagingOutboxBuilder ConfigureOutboxPostgreSQL(this ADFMessagingOutboxBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.ADFMessagingBuilder.Services.AddInMemoryMessageBus([typeof(OutboxDbContext).Assembly]);

		builder.ADFMessagingBuilder.Services.AddLogging();
		builder.ADFMessagingBuilder.Services.AddPostgreSQLServices();
		builder.ADFMessagingBuilder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.Messaging.ConnectionStringProvider>();
		builder.ADFMessagingBuilder.Services.AddUnitOfWork<IOutboxUnitOfWork>(efConnectionProvider => new OutboxUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.AddQueryUnitOfWork<IOutboxQueryUnitOfWork>(efConnectionProvider => new OutboxQueryUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IOutboxUnitOfWorkFactory, OutboxUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IOutboxQueryUnitOfWorkFactory, OutboxQueryUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.AddDbContext<OutboxDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IOutboxDbContext, OutboxDbContext>();
		builder.ADFMessagingBuilder.Services.AddDbContext<OutboxQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IOutboxQueryDbContext, OutboxQueryDbContext>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<Outbox.ITableInfoProvider, TableInfoProvider>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<Outbox.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
