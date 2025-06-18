#nullable disable

namespace Legion.ADF.Audit.SqlServer;

public partial class AuditQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Audit.SqlServer.IAuditQueryDbContext
{
	public override bool IsAuditDbContext => true;
}
