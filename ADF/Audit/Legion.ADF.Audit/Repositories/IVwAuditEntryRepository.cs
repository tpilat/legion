namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IVwAuditEntryRepository : Legion.ADF.Audit.IAuditQueryRepository<Legion.ADF.Audit.Model.VwAuditEntry>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwAuditEntry>? AccessControlManager { get; }

}
