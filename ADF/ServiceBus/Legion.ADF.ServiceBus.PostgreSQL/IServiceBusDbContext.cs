using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.ServiceBus.PostgreSQL;

public interface IServiceBusDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Model.Host> Host { get; }
	DbSet<Legion.ADF.ServiceBus.Model.HostActivity> HostActivity { get; }
	DbSet<Legion.ADF.ServiceBus.Model.HostLog> HostLog { get; }
	DbSet<Legion.ADF.ServiceBus.Model.Job> Job { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobActivity> JobActivity { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobData> JobData { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobExecution> JobExecution { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobLog> JobLog { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobMessage> JobMessage { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobMessageType> JobMessageType { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobRunType> JobRunType { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobStatistics> JobStatistics { get; }
	DbSet<Legion.ADF.ServiceBus.Model.JobStatus> JobStatus { get; }
	DbSet<Legion.ADF.ServiceBus.Model.Orchestration> Orchestration { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationInstance> OrchestrationInstance { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStatus> OrchestrationStatus { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStep> OrchestrationStep { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing> OrchestrationStepProcessing { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection> OrchestrationStepProcessingDirection { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog> OrchestrationStepProcessingLog { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage> OrchestrationStepProcessingMessage { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType> OrchestrationStepProcessingMessageType { get; }
	DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus> OrchestrationStepProcessingStatus { get; }
}
