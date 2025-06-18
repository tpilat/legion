namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IAuditEntryRepository : Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.AuditEntry>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.AuditEntry>? AccessControlManager { get; }

}
