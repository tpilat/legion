using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Legion.ADF.Auth.PostgreSQL;

public class RoleConfiguration : IEntityTypeConfiguration<Auth.Model.Role>
{
	public const string PrimaryKeyFormatter = "{{\"IdRole\":\"{0}\"}}";

	public void Configure(EntityTypeBuilder<Auth.Model.Role> entityBuilder)
		=> ConfigureEntity(entityBuilder);

	public static void ConfigureEntity(EntityTypeBuilder<Auth.Model.Role> entityBuilder)
	{
		entityBuilder.HasKey(e => e.IdRole);

		entityBuilder.ToTable("Role", "auth");

		entityBuilder.HasIndex(e => new { e.DeletedUtc, e.Name }, "UQ_Role_Name_Deleted")
				.IsUnique();

		entityBuilder.Property(e => e.IdRole)
			.HasColumnType("uuid")
		.ValueGeneratedNever();

		entityBuilder.Property(e => e.Name)
			.IsRequired()
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.NormalizedName)
			.IsRequired()
			.HasColumnType("varchar(256)")
			.HasMaxLength(256);

		entityBuilder.Property(e => e.Data).HasColumnType("jsonb");

		entityBuilder.Property(e => e.AuditCreatedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.AuditModifiedUtc).HasColumnType("timestamptz");

		entityBuilder.Property(e => e.IdAuditCreatedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.IdAuditModifiedBy).HasColumnType("uuid");

		entityBuilder.Property(e => e.ConcurrencyToken)
			.HasColumnType("uuid")
				.IsConcurrencyToken();

		entityBuilder.Property(e => e.DeletedUtc).HasColumnType("timestamptz");
	}

	public static ModelBuilder Build(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auth.Model.Role>(ConfigureEntity);

		return modelBuilder;
	}
}
