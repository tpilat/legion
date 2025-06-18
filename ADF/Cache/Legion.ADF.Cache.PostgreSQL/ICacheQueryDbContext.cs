using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.PostgreSQL;

public interface ICacheQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Cache.Model.VwReloadableCacheKey> VwReloadableCacheKey { get; set; }
}
