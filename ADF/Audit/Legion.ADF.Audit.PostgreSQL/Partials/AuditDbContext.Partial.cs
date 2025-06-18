#nullable disable

namespace Legion.ADF.Audit.PostgreSQL;

public partial class AuditDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Audit.PostgreSQL.IAuditDbContext
{
	public override bool IsAuditDbContext => true;
}
