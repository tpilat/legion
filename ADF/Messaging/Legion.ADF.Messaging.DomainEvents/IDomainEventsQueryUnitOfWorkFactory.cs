namespace Legion.ADF.Messaging.DomainEvents;

public partial interface IDomainEventsQueryUnitOfWorkFactory
{
	IDomainEventsQueryUnitOfWork Create(Legion.Database.IConnectionProvider connectionProvider);
	IDomainEventsQueryUnitOfWork Create(Legion.Model.Repositories.IUnitOfWork unitOfWork);
	IDomainEventsQueryUnitOfWork Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork);
	IDomainEventsQueryUnitOfWork Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore);
	IDomainEventsQueryUnitOfWork CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore);
}
