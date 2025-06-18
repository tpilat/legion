namespace Legion.ADF.Auth;

public partial interface IAuthQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Auth.Model.Repositories.IVwUserRepository VwUserRepository { get; }
}
