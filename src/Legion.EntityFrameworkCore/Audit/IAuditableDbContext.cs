namespace Legion.EntityFrameworkCore.Audit;

public interface IAuditableDbContext : IDbContext, IDisposable, IAsyncDisposable
{
}
