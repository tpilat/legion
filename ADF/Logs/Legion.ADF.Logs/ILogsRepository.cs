namespace Legion.ADF.Logs;

public partial interface ILogsRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface ILogsRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, ILogsRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
