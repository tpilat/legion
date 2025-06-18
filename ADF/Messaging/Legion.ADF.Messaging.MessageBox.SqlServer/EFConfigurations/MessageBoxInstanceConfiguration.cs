using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public class MessageBoxInstanceConfiguration : IEntityTypeConfiguration<MessageBox.Model.MessageBoxInstance>
{
	public const string PrimaryKeyFormatter = "{{\"IdMessageBoxInstance\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<MessageBox.Model.MessageBoxInstance> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<MessageBox.Model.MessageBoxInstance> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdMessageBoxInstance);

		entityBuilder.ToTable("MessageBoxInstance", "mbox");

		entityBuilder.Property(e => e.IdMessageBoxInstance)
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
		modelBuilder.Entity<MessageBox.Model.MessageBoxInstance>(ConfigureEntity);

		return modelBuilder;
	}
}
