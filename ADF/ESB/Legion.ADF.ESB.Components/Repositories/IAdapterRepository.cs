namespace Legion.ADF.ESB.Components.Model.Repositories;

public partial interface IAdapterRepository : Legion.ADF.ESB.Components.IComponentsRepository<Legion.ADF.ESB.Components.Model.Adapter>
{
	Legion.ADF.ESB.Components.Queries.Adapter.IGetAdapterById GetAdapterById(
		Legion.ADF.ESB.Components.Queries.Adapter.GetAdapterByIdQuery getAdapterByIdQuery);

	Legion.ADF.ESB.Components.Queries.Adapter.IGetAllAdapters GetAllAdapters(
		Legion.ADF.ESB.Components.Queries.Adapter.GetAllAdaptersQuery getAllAdaptersQuery);
}
