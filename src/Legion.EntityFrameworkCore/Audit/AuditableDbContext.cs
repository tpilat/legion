using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Legion.EntityFrameworkCore.Audit;

public abstract class AuditableDbContext : DbContextBase, IAuditableDbContext, IDbContext, IDisposable, IAsyncDisposable
{
	public AuditableDbContext(DbContextOptions options, ILogger logger)
		: base(options, logger)
	{
	}

	protected AuditableDbContext(ILogger logger)
		: base(logger)
	{
	}
}
