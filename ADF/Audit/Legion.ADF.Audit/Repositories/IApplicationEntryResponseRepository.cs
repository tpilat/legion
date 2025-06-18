namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IApplicationEntryResponseRepository : Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.ApplicationEntryResponse>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntryResponse>? AccessControlManager { get; }

}
