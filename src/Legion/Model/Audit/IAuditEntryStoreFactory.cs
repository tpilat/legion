using Legion.Database;

namespace Legion.Model.Audit;

public interface IAuditEntryStoreFactory
{
	IAuditEntryStore Create(IConnectionProvider connectionProvider);
}
