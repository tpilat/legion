namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IApplicationEntryRequestRepository : Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.ApplicationEntryRequest>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntryRequest>? AccessControlManager { get; }

}
