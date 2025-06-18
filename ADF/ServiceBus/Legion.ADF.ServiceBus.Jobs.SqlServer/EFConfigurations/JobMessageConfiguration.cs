using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

public class JobMessageConfiguration : IEntityTypeConfiguration<Jobs.Model.JobMessage>
{
	public const string PrimaryKeyFormatter = "{{\"IdJobMessage\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Jobs.Model.JobMessage> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Jobs.Model.JobMessage> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdJobMessage);

		entityBuilder.ToTable("JobMessage", "jobs");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_JobMessage_Job");

		entityBuilder.HasIndex(e => e.IdJobMessageType, "IXFK_JobMessage_JobMessageType");

		entityBuilder.Property(e => e.IdJobMessage)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.IdJob).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdMessage).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.IdJobMessageType).HasColumnType("uniqueidentifier");

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.HasOne(d => d.Job)
			.WithMany(p => p.JobMessages)
			.HasForeignKey(d => d.IdJob)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobMessage_IdJob");

		entityBuilder.HasOne(d => d.JobMessageType)
			.WithMany(p => p.JobMessages)
			.HasForeignKey(d => d.IdJobMessageType)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_JobMessage_IdJobMessageType");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Jobs.Model.JobMessage>(ConfigureEntity);

		return modelBuilder;
	}
}
