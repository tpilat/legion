using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class InboxQueueProcessingModeConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxQueueProcessingMode>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxQueueProcessingMode\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxQueueProcessingMode> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxQueueProcessingMode> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxQueueProcessingMode);

		entityBuilder.ToTable("InboxQueueProcessingMode", "inbox");

		entityBuilder.Property(e => e.IdInboxQueueProcessingMode)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(63)")
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxQueueProcessingMode>(ConfigureEntity);

		return modelBuilder;
	}
}
