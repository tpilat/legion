using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public interface IJobsDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.Job> Job { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobData> JobData { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobExecution> JobExecution { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobLog> JobLog { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobMessage> JobMessage { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> JobMessageType { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobRunType> JobRunType { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobStatistics> JobStatistics { get; }
	DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobStatus> JobStatus { get; }
}
