using Legion.Model.Repositories;

namespace Legion.EntityFrameworkCore.Model.Repositories;

public partial interface IDbQueryUnitOfWork : IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	public IEFConnectionProvider ConnectionProvider { get; }
}
