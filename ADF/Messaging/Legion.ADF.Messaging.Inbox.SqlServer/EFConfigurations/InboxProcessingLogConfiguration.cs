using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class InboxProcessingLogConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxProcessingLog>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxProcessingLog\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxProcessingLog> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxProcessingLog> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxProcessingLog);

		entityBuilder.ToTable("InboxProcessingLog", "inbox");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_InboxProcessingLog_InboxInstance");

		entityBuilder.HasIndex(e => e.IdInboxQueue, "IXFK_InboxProcessingLog_InboxQueue");

		entityBuilder.Property(e => e.IdInboxProcessingLog)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdInboxQueue).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.TraceCorrelationId).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdLogMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Detail).HasColumnType("nvarchar(max)");

		entityBuilder.HasOne(d => d.InboxInstance)
			.WithMany(p => p.InboxProcessingLogs)
			.HasForeignKey(d => d.IdInboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxProcessingLog_IdInboxInstance");

		entityBuilder.HasOne(d => d.InboxQueue)
			.WithMany(p => p.InboxProcessingLogs)
			.HasForeignKey(d => d.IdInboxQueue)
			.HasConstraintName("FK_InboxProcessingLog_InboxQueue");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxProcessingLog>(ConfigureEntity);

		return modelBuilder;
	}
}
