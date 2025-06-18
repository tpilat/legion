namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IApplicationEntryRepository : Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.ApplicationEntry>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntry>? AccessControlManager { get; }

}
