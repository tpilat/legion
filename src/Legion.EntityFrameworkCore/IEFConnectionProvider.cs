using Legion.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Legion.EntityFrameworkCore;

public interface IEFConnectionProvider : IConnectionProvider, IDisposable, IAsyncDisposable
{
	IDbContextTransaction? DbContextTransaction { get; }
	Action<DbContextOptionsBuilder>? DbContextOptionsConfiguration { get; }

	bool HasDbContext<TDbContext>()
		where TDbContext : IDbContext;

	TDbContext GetOrCreateDbContext<TDbContext>(IScopeContext scopeContext, bool? allowLocking = null)
		where TDbContext : IDbContext;

	//bool SetConnection(IScopeContext scopeContext, DbConnection DbConnection);

	void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
}
