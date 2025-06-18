namespace Legion.ADF.ESB.Components;

public partial interface IComponentsQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork
{
	Legion.ADF.ESB.Components.Model.Repositories.IVwJobRepository VwJobRepository { get; }
}
