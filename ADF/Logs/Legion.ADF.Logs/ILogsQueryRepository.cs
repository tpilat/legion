namespace Legion.ADF.Logs;

public partial interface ILogsQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface ILogsQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, ILogsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
