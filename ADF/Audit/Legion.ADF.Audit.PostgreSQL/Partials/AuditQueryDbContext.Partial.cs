#nullable disable

namespace Legion.ADF.Audit.PostgreSQL;

public partial class AuditQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Audit.PostgreSQL.IAuditQueryDbContext
{
	public override bool IsAuditDbContext => true;
}
