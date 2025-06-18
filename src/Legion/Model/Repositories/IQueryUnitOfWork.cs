namespace Legion.Model.Repositories;

public partial interface IQueryUnitOfWork : IDisposable, IAsyncDisposable
{
	Database.IConnectionProvider ConnectionProvider { get; }
	IServiceProvider ServiceProvider { get; }
}
