using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ServiceBus;

public partial interface IServiceBusUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.ServiceBus.Model.Repositories.IHostRepository HostRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IHostActivityRepository HostActivityRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IHostLogRepository HostLogRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobRepository JobRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobActivityRepository JobActivityRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobDataRepository JobDataRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobExecutionRepository JobExecutionRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobLogRepository JobLogRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobMessageRepository JobMessageRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobMessageTypeRepository JobMessageTypeRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobRunTypeRepository JobRunTypeRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobStatisticsRepository JobStatisticsRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IJobStatusRepository JobStatusRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationRepository OrchestrationRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationInstanceRepository OrchestrationInstanceRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStatusRepository OrchestrationStatusRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepRepository OrchestrationStepRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingRepository OrchestrationStepProcessingRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingDirectionRepository OrchestrationStepProcessingDirectionRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingLogRepository OrchestrationStepProcessingLogRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingMessageRepository OrchestrationStepProcessingMessageRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingMessageTypeRepository OrchestrationStepProcessingMessageTypeRepository { get; }

	Legion.ADF.ServiceBus.Model.Repositories.IOrchestrationStepProcessingStatusRepository OrchestrationStepProcessingStatusRepository { get; }
}
