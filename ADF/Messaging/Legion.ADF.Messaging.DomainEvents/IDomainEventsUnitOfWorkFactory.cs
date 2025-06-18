namespace Legion.ADF.Messaging.DomainEvents;

public partial interface IDomainEventsUnitOfWorkFactory
{
	IDomainEventsUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IDomainEventsUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IDomainEventsUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IDomainEventsUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IDomainEventsUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
