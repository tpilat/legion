using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ServiceBus.Jobs;

public partial interface IJobsUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobRepository JobRepository { get; }

	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobDataRepository JobDataRepository { get; }

	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobLogRepository JobLogRepository { get; }

	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobMessageRepository JobMessageRepository { get; }

	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobMessageTypeRepository JobMessageTypeRepository { get; }

	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobRunTypeRepository JobRunTypeRepository { get; }

	Legion.ADF.ServiceBus.Jobs.Model.Repositories.IJobStatusRepository JobStatusRepository { get; }
}
