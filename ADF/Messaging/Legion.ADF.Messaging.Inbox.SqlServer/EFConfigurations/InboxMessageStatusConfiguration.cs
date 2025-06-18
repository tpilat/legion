using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class InboxMessageStatusConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxMessageStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxMessageStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxMessageStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxMessageStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxMessageStatus);

		entityBuilder.ToTable("InboxMessageStatus", "inbox");

		entityBuilder.Property(e => e.IdInboxMessageStatus)
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
		modelBuilder.Entity<Inbox.Model.InboxMessageStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
