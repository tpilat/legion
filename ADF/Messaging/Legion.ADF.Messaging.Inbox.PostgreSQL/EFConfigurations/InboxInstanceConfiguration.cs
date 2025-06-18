using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public class InboxInstanceConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxInstance>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxInstance\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxInstance> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxInstance> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxInstance);

		entityBuilder.ToTable("InboxInstance", "inbox");

		entityBuilder.Property(e => e.IdInboxInstance)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Version)
			.IsRequired()
			.HasColumnType("varchar(15)")
			.HasMaxLength(15);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
