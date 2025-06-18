namespace Legion.ADF.Messaging.MessageBox;

public partial interface IMessageBoxQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IMessageBoxQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IMessageBoxQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
