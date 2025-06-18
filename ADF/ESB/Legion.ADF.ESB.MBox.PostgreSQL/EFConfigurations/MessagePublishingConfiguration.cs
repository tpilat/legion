using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class MessagePublishingConfiguration : IEntityTypeConfiguration<MBox.Model.MessagePublishing>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessagePublishing\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.MessagePublishing> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.MessagePublishing> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessagePublishing);

		entityBuilder.ToTable("MessagePublishing", "mbox");

		entityBuilder.HasIndex(e => e.IdAdapter, "IXFK_MessagePublishing_IdAdapter");

		entityBuilder.HasIndex(e => e.IdJob, "IXFK_MessagePublishing_IdJob");

		entityBuilder.HasIndex(e => e.IdMessage, "IXFK_MessagePublishing_IdMessage");

		entityBuilder.HasIndex(e => e.IdStepInstance, "IXFK_MessagePublishing_IdStepInstance");

		entityBuilder.Property(e => e.IdMessagePublishing).ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamp(6)");

		entityBuilder.HasOne(d => d.Message)
			.WithMany(p => p.MessagePublishings)
			.HasForeignKey(d => d.IdMessage)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessagePublishing_IdMessage");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.MessagePublishing>(ConfigureEntity);

		return modelBuilder;
	}
}
