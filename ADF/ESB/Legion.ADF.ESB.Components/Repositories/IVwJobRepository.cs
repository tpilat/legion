namespace Legion.ADF.ESB.Components.Model.Repositories;

public partial interface IVwJobRepository : Legion.ADF.ESB.Components.IComponentsQueryRepository<Legion.ADF.ESB.Components.Model.VwJob>
{
	Legion.ADF.ESB.Components.Queries.VwJob.IGetVwJobById GetVwJobById(
		Legion.ADF.ESB.Components.Queries.VwJob.GetVwJobByIdQuery getVwJobByIdQuery);
}
