#nullable disable

namespace Legion.ADF.Audit.SqlServer;

public partial class AuditDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Audit.SqlServer.IAuditDbContext
{
	public override bool IsAuditDbContext => true;
}
