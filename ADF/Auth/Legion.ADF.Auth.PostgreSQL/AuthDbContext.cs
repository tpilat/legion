using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Auth.PostgreSQL;

public partial class AuthDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Auth.PostgreSQL.IAuthDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static AuthDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Auth.Model.ExternalLogin), PostgreSQL.ExternalLoginConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.LoginProvider), PostgreSQL.LoginProviderConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.Permission), PostgreSQL.PermissionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.Role), PostgreSQL.RoleConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.RolePermission), PostgreSQL.RolePermissionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.User), PostgreSQL.UserConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.UserPermission), PostgreSQL.UserPermissionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.UserRole), PostgreSQL.UserRoleConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.UserToken), PostgreSQL.UserTokenConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Auth.Model.ExternalLogin> ExternalLogin { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.LoginProvider> LoginProvider { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.Permission> Permission { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.Role> Role { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.RolePermission> RolePermission { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.User> User { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.UserPermission> UserPermission { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.UserRole> UserRole { get; set; }
	public virtual DbSet<Legion.ADF.Auth.Model.UserToken> UserToken { get; set; }

	public AuthDbContext(DbContextOptions<AuthDbContext> options, Microsoft.Extensions.Logging.ILogger<AuthDbContext> logger)
		: base(options, logger)
	{
	}

	public AuthDbContext(Microsoft.Extensions.Logging.ILogger<AuthDbContext> logger)
		: base(logger)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			if (ConnectionProvider == null)
				Legion.Throw.InitializationException(ConnectionProvider);

			ConnectionProvider.OnConfiguring(optionsBuilder);
		}
		else
		{
			SetIsDbContextOptionsBuilderPreconfigured();
		}

		if (DbContextSettintgs.AllowLocking == true)
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_PostgreSql());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.ExternalLoginConfiguration.Build(modelBuilder);
		PostgreSQL.LoginProviderConfiguration.Build(modelBuilder);
		PostgreSQL.PermissionConfiguration.Build(modelBuilder);
		PostgreSQL.RoleConfiguration.Build(modelBuilder);
		PostgreSQL.RolePermissionConfiguration.Build(modelBuilder);
		PostgreSQL.UserConfiguration.Build(modelBuilder);
		PostgreSQL.UserPermissionConfiguration.Build(modelBuilder);
		PostgreSQL.UserRoleConfiguration.Build(modelBuilder);
		PostgreSQL.UserTokenConfiguration.Build(modelBuilder);
	}
}
