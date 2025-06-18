using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public class MessageTypeConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageType>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageType\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageType> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageType> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageType);

		entityBuilder.ToTable("MessageType", "mbox");

		entityBuilder.HasIndex(e => e.IdMessageBoxInstance, "IXFK_MessageType_MessageBoxInstance");

		entityBuilder.HasIndex(e => e.Namespace, "UQ_MessageType_Namespace")
				.IsUnique();

		entityBuilder.Property(e => e.IdMessageType)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Code)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(127)")
			.HasMaxLength(127);

		entityBuilder.Property(e => e.Namespace)
			.IsRequired()
			.HasColumnType("varchar(1023)")
			.HasMaxLength(1023);

		entityBuilder.Property(e => e.CreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdMessageBoxInstance).HasColumnType("uuid");

		entityBuilder.HasOne(d => d.MessageBoxInstance)
			.WithMany(p => p.MessageTypes)
			.HasForeignKey(d => d.IdMessageBoxInstance)
			.OnDelete(DeleteBehavior.ClientSetNull)
			.HasConstraintName("FK_MessageType_MessageBoxInstance");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MessageBox.Model.MessageType>(ConfigureEntity);

		return modelBuilder;
	}
}
