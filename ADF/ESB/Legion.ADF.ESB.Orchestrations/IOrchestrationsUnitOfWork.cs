using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ESB.Orchestrations;

public partial interface IOrchestrationsUnitOfWork : Legion.Model.Repositories.IUnitOfWork
{

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationRepository OrchestrationRepository { get; }

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationInstanceRepository OrchestrationInstanceRepository { get; }

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStatusRepository OrchestrationStatusRepository { get; }

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepRepository OrchestrationStepRepository { get; }

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepInstanceRepository OrchestrationStepInstanceRepository { get; }

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepLogRepository OrchestrationStepLogRepository { get; }

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IOrchestrationStepStatusRepository OrchestrationStepStatusRepository { get; }

	Legion.ADF.ESB.Orchestrations.Model.Repositories.IStepDirectionRepository StepDirectionRepository { get; }
}
