using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ServiceBus.Orchestrations;

public partial interface IOrchestrationsUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationRepository OrchestrationRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationInstanceRepository OrchestrationInstanceRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStatusRepository OrchestrationStatusRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStepRepository OrchestrationStepRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStepProcessingRepository OrchestrationStepProcessingRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStepProcessingDirectionRepository OrchestrationStepProcessingDirectionRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStepProcessingLogRepository OrchestrationStepProcessingLogRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStepProcessingMessageRepository OrchestrationStepProcessingMessageRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStepProcessingMessageTypeRepository OrchestrationStepProcessingMessageTypeRepository { get; }

	Legion.ADF.ServiceBus.Orchestrations.Model.Repositories.IOrchestrationStepProcessingStatusRepository OrchestrationStepProcessingStatusRepository { get; }
}
