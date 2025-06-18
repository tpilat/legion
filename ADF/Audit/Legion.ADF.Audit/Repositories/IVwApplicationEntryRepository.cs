namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IVwApplicationEntryRepository : Legion.ADF.Audit.IAuditQueryRepository<Legion.ADF.Audit.Model.VwApplicationEntry>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.VwApplicationEntry>? AccessControlManager { get; }

}
