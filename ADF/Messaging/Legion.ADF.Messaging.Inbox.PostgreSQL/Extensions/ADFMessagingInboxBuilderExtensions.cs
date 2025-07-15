using Legion.ADF.Messaging.Inbox;
using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Messaging;

public static class ADFMessagingInboxBuilderExtensions
{
	public static ADFMessagingInboxBuilder ConfigureInboxPostgreSQL(this ADFMessagingInboxBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.ADFMessagingBuilder.Services.AddInMemoryMessageBus([typeof(InboxDbContext).Assembly]);

		builder.ADFMessagingBuilder.Services.AddLogging();
		builder.ADFMessagingBuilder.Services.AddPostgreSQLServices();
		builder.ADFMessagingBuilder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.Messaging.ConnectionStringProvider>();
		builder.ADFMessagingBuilder.Services.AddUnitOfWork<IInboxUnitOfWork>(efConnectionProvider => new InboxUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.AddQueryUnitOfWork<IInboxQueryUnitOfWork>(efConnectionProvider => new InboxQueryUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IInboxUnitOfWorkFactory, InboxUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IInboxQueryUnitOfWorkFactory, InboxQueryUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IUnitOfWorkFactory<IInboxUnitOfWork>, InboxUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<IInboxQueryUnitOfWork>, InboxQueryUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.AddDbContext<InboxDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IInboxDbContext, InboxDbContext>();
		builder.ADFMessagingBuilder.Services.AddDbContext<InboxQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IInboxQueryDbContext, InboxQueryDbContext>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<Inbox.ITableInfoProvider, TableInfoProvider>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<Inbox.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
