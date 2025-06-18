using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public interface IMBoxDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.ESB.MBox.Model.Message> Message { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.MessageContent> MessageContent { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.MessageProcessingLog> MessageProcessingLog { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.MessageProcessingStatus> MessageProcessingStatus { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.MessagePublishing> MessagePublishing { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.MessageStatus> MessageStatus { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.MessageType> MessageType { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.Queue> Queue { get; }
	DbSet<Legion.ADF.ESB.MBox.Model.QueuedMessage> QueuedMessage { get; }
}
