using Legion.Model.Repositories;

namespace Legion.EntityFrameworkCore.Model.Repositories;

public interface IDbUnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
	IEFConnectionProvider ConnectionProvider { get; }
}
