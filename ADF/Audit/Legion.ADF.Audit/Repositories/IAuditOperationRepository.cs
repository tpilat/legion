namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IAuditOperationRepository : Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.AuditOperation>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.AuditOperation>? AccessControlManager { get; }

}
