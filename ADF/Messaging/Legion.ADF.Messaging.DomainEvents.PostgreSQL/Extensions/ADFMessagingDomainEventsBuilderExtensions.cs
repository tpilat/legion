using Legion.ADF.Messaging.DomainEvents;
using Legion.ADF.Messaging.DomainEvents.PostgreSQL;
using Legion.Database.PostgreSQL.Extensions;
using Legion.EntityFrameworkCore.Database;
using Legion.EntityFrameworkCore.Extensions;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Messaging;

public static class ADFMessagingDomainEventsBuilderExtensions
{
	public static ADFMessagingDomainEventsBuilder ConfigureDomainEventsPostgreSQL(this ADFMessagingDomainEventsBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		builder.ADFMessagingBuilder.Services.AddInMemoryMessageBus([typeof(DomainEventsDbContext).Assembly]);

		builder.ADFMessagingBuilder.Services.AddLogging();
		builder.ADFMessagingBuilder.Services.AddPostgreSQLServices();
		builder.ADFMessagingBuilder.Services.AddPostgreSQLConnectionProvider<Legion.ADF.Messaging.ConnectionStringProvider>();
		builder.ADFMessagingBuilder.Services.AddUnitOfWork<IDomainEventsUnitOfWork>(efConnectionProvider => new DomainEventsUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.AddQueryUnitOfWork<IDomainEventsQueryUnitOfWork>(efConnectionProvider => new DomainEventsQueryUnitOfWork(efConnectionProvider));
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IDomainEventsUnitOfWorkFactory, DomainEventsUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<IDomainEventsQueryUnitOfWorkFactory, DomainEventsQueryUnitOfWorkFactory>();
		builder.ADFMessagingBuilder.Services.AddDbContext<DomainEventsDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IDomainEventsDbContext, DomainEventsDbContext>();
		builder.ADFMessagingBuilder.Services.AddDbContext<DomainEventsQueryDbContext>(options => options.AddRowNumberSupport(), ServiceLifetime.Transient);
		builder.ADFMessagingBuilder.Services.TryAddTransient<IDomainEventsQueryDbContext, DomainEventsQueryDbContext>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<DomainEvents.ITableInfoProvider, TableInfoProvider>();
		builder.ADFMessagingBuilder.Services.TryAddSingleton<DomainEvents.IQueryTableInfoProvider, QueryTableInfoProvider>();

		return builder;
	}
}
