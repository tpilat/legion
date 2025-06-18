using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.SqlServer;

public interface ICacheQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Cache.Model.VwReloadableCacheKey> VwReloadableCacheKey { get; set; }
}
