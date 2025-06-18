using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.ServiceBus.Orchestrations.SqlServer;

public interface IOrchestrationsDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration> Orchestration { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance> OrchestrationInstance { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus> OrchestrationStatus { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep> OrchestrationStep { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing> OrchestrationStepProcessing { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection> OrchestrationStepProcessingDirection { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog> OrchestrationStepProcessingLog { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage> OrchestrationStepProcessingMessage { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType> OrchestrationStepProcessingMessageType { get; }
	DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus> OrchestrationStepProcessingStatus { get; }
}
