namespace Legion.ADF.Audit.Model.Repositories;

public partial interface IApplicationEntryTokenRepository : Legion.ADF.Audit.IAuditRepository<Legion.ADF.Audit.Model.ApplicationEntryToken>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Audit.Model.ApplicationEntryToken>? AccessControlManager { get; }

	Legion.ADF.Audit.Queries.ApplicationEntryToken.IGetApplicationEntryTokenByTokenVersionFilePath GetApplicationEntryTokenByTokenVersionFilePath(
		Legion.ADF.Audit.Queries.ApplicationEntryToken.GetApplicationEntryTokenByTokenVersionFilePathQuery getApplicationEntryTokenByTokenVersionFilePath);
}
