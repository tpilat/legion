using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public class DomainEventContentConfiguration : IEntityTypeConfiguration<DomainEvents.Model.DomainEventContent>
{
	public const string PrimaryKeyFormatter = "{{\"IdDomainEventContent\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<DomainEvents.Model.DomainEventContent> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<DomainEvents.Model.DomainEventContent> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdDomainEventContent);

		entityBuilder.ToTable("DomainEventContent", "devt");

		entityBuilder.Property(e => e.IdDomainEventContent)
			.HasColumnType("uniqueidentifier")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Content)
			.IsRequired()
			.HasColumnType("nvarchar(max)");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DomainEvents.Model.DomainEventContent>(ConfigureEntity);

		return modelBuilder;
	}
}
