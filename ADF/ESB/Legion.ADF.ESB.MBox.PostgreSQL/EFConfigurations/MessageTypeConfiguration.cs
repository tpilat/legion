using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public class MessageTypeConfiguration : IEntityTypeConfiguration<MBox.Model.MessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MBox.Model.MessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MBox.Model.MessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageType);

		entityBuilder.ToTable("MessageType", "mbox");

		entityBuilder.Property(e => e.IdMessageType).ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasMaxLength(63);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.MimeType)
			.IsRequired()
			.HasMaxLength(1023);

		entityBuilder.HasMany(d => d.Queues)
			.WithOne()
			.HasForeignKey(d => d.IdAdapter)
			.HasConstraintName("FK_Queue_IdAdapter");

		entityBuilder.HasMany(d => d.Queues)
			.WithOne()
			.HasForeignKey(d => d.IdJob)
			.HasConstraintName("FK_Queue_IdJob");

		entityBuilder.HasMany(d => d.Queues)
			.WithOne()
			.HasForeignKey(d => d.IdOrchestration)
			.HasConstraintName("FK_Queue_IdOrchestration");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MBox.Model.MessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
