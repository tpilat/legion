using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public interface IOrchestrationsDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ESB.Orchestrations.Model.Orchestration> Orchestration { get; }
	DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance> OrchestrationInstance { get; }
	DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStatus> OrchestrationStatus { get; }
	DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep> OrchestrationStep { get; }
	DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepInstance> OrchestrationStepInstance { get; }
	DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog> OrchestrationStepLog { get; }
	DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepStatus> OrchestrationStepStatus { get; }
	DbSet<Legion.ADF.ESB.Orchestrations.Model.StepDirection> StepDirection { get; }
}
