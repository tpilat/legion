namespace Legion.ADF.ESB.MBox;

public partial interface IMBoxQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork
{
	Legion.ADF.ESB.MBox.Model.Repositories.IVwQueuedMessageRepository VwQueuedMessageRepository { get; }
}
