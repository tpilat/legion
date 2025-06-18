namespace Legion.ADF.Auth;

public partial interface IAuthRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IAuthRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IAuthRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
