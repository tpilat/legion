using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.ESB.Components.PostgreSQL;

public interface IComponentsDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ESB.Components.Model.Adapter> Adapter { get; }
	DbSet<Legion.ADF.ESB.Components.Model.AdapterLog> AdapterLog { get; }
	DbSet<Legion.ADF.ESB.Components.Model.AdapterRequest> AdapterRequest { get; }
	DbSet<Legion.ADF.ESB.Components.Model.AdapterRequestPayload> AdapterRequestPayload { get; }
	DbSet<Legion.ADF.ESB.Components.Model.AdapterResponse> AdapterResponse { get; }
	DbSet<Legion.ADF.ESB.Components.Model.AdapterResponsePayload> AdapterResponsePayload { get; }
	DbSet<Legion.ADF.ESB.Components.Model.AdapterStatus> AdapterStatus { get; }
	DbSet<Legion.ADF.ESB.Components.Model.Job> Job { get; }
	DbSet<Legion.ADF.ESB.Components.Model.JobData> JobData { get; }
	DbSet<Legion.ADF.ESB.Components.Model.JobLog> JobLog { get; }
	DbSet<Legion.ADF.ESB.Components.Model.JobStatus> JobStatus { get; }
	DbSet<Legion.ADF.ESB.Components.Model.JobType> JobType { get; }
}
