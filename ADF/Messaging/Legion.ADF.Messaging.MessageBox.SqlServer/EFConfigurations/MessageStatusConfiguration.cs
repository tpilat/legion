using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class MessageStatusConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageStatus>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageStatus\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageStatus> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageStatus> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageStatus);

		entityBuilder.ToTable("MessageStatus", "mbox");

		entityBuilder.Property(e => e.IdMessageStatus)
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
		modelBuilder.Entity<MessageBox.Model.MessageStatus>(ConfigureEntity);

		return modelBuilder;
	}
}
