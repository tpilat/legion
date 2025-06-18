namespace Legion.ADF.Logs;

public partial interface ILogsQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Logs.Model.Repositories.IVwLogRepository VwLogRepository { get; }
}
