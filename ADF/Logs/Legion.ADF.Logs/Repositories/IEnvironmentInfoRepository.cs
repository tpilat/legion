namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IEnvironmentInfoRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.EnvironmentInfo>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.EnvironmentInfo>? AccessControlManager { get; }

	Legion.ADF.Logs.Queries.EnvironmentInfo.IGetEnvironmentInfoById GetEnvironmentInfoById(
		Legion.ADF.Logs.Queries.EnvironmentInfo.GetEnvironmentInfoByIdQuery getEnvironmentInfoByIdQuery);
}
