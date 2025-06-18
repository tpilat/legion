using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

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
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(255)")
			.HasMaxLength(255);

		entityBuilder.Property(e => e.Version)
			.IsRequired()
			.HasColumnType("nvarchar(15)")
			.HasMaxLength(15);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
