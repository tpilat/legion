using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Cache.PostgreSQL;

public interface ICacheDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Cache.Model.ReloadableCacheKey> ReloadableCacheKey { get; }
}
