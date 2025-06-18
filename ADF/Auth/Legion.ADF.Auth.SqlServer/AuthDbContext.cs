using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Auth.SqlServer;

public partial class AuthDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Auth.SqlServer.IAuthDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static AuthDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Auth.Model.ExternalLogin), SqlServer.ExternalLoginConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.LoginProvider), SqlServer.LoginProviderConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.Permission), SqlServer.PermissionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.Role), SqlServer.RoleConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.RolePermission), SqlServer.RolePermissionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.User), SqlServer.UserConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.UserPermission), SqlServer.UserPermissionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.UserRole), SqlServer.UserRoleConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auth.Model.UserToken), SqlServer.UserTokenConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.ExternalLoginConfiguration.Build(modelBuilder);
		SqlServer.LoginProviderConfiguration.Build(modelBuilder);
		SqlServer.PermissionConfiguration.Build(modelBuilder);
		SqlServer.RoleConfiguration.Build(modelBuilder);
		SqlServer.RolePermissionConfiguration.Build(modelBuilder);
		SqlServer.UserConfiguration.Build(modelBuilder);
		SqlServer.UserPermissionConfiguration.Build(modelBuilder);
		SqlServer.UserRoleConfiguration.Build(modelBuilder);
		SqlServer.UserTokenConfiguration.Build(modelBuilder);
	}
}
