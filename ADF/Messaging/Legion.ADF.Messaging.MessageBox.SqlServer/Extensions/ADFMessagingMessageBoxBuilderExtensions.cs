using Legion.ADF.Messaging.MessageBox;
using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.Database.SqlServer.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Messaging;

public static class ADFMessagingMessageBoxBuilderExtensions
{
	public static ADFMessagingMessageBoxBuilder ConfigureMessageBoxSqlServer(this ADFMessagingMessageBoxBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.ADFMessagingBuilder.Services.AddInMemoryMessageBus([typeof(MessageBoxDbContext).Assembly]);

		builder.ADFMessagingBuilder.Services.AddLogging();
		builder.ADFMessagingBuilder.Services.AddSqlServerServices();
		builder.ADFMessagingBuilder.Services.AddSqlServerConnectionProvider<Legion.ADF.Messaging.ConnectionStringProvider>();
		builder.ADFMessagingBuilder.Services.AddUnitOfWork<IMessageBoxUnitOfWork>(efConnectionProvider => new MessageBoxUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.AddQueryUnitOfWork<IMessageBoxQueryUnitOfWork>(efConnectionProvider => new MessageBoxQueryUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IMessageBoxUnitOfWorkFactory, MessageBoxUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IMessageBoxQueryUnitOfWorkFactory, MessageBoxQueryUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IUnitOfWorkFactory<IMessageBoxUnitOfWork>, MessageBoxUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IQueryUnitOfWorkFactory<IMessageBoxQueryUnitOfWork>, MessageBoxQueryUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.AddDbContext<MessageBoxDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IMessageBoxDbContext, MessageBoxDbContext>();
		builder.ADFMessagingBuilder.Services.AddDbContext<MessageBoxQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IMessageBoxQueryDbContext, MessageBoxQueryDbContext>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<MessageBox.ITableInfoProvider, TableInfoProvider>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<MessageBox.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
