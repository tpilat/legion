using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public class InboxMessageTypeConfiguration : IEntityTypeConfiguration<Inbox.Model.InboxMessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdInboxMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Inbox.Model.InboxMessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Inbox.Model.InboxMessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdInboxMessageType);

		entityBuilder.ToTable("InboxMessageType", "inbox");

		entityBuilder.HasIndex(e => e.IdInboxInstance, "IXFK_InboxMessageType_InboxInstance");

		entityBuilder.HasIndex(e => e.Namespace, "UQ_InboxMessageType_Namespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdInboxMessageType)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("nvarchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("nvarchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("datetime2(7)");

		entityBuilder.Property(e => e.IdInboxInstance).HasColumnType("uniqueidentifier");

		entityBuilder.HasOne(d => d.InboxInstance)
			.WithMany(p => p.InboxMessageTypes)
			.HasForeignKey(d => d.IdInboxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_InboxMessageType_IdInboxInstance");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Inbox.Model.InboxMessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
