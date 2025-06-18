using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Auth.PostgreSQL;

public interface IAuthDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Auth.Model.ExternalLogin> ExternalLogin { get; }
	DbSet<Legion.ADF.Auth.Model.LoginProvider> LoginProvider { get; }
	DbSet<Legion.ADF.Auth.Model.Permission> Permission { get; }
	DbSet<Legion.ADF.Auth.Model.Role> Role { get; }
	DbSet<Legion.ADF.Auth.Model.RolePermission> RolePermission { get; }
	DbSet<Legion.ADF.Auth.Model.User> User { get; }
	DbSet<Legion.ADF.Auth.Model.UserPermission> UserPermission { get; }
	DbSet<Legion.ADF.Auth.Model.UserRole> UserRole { get; }
	DbSet<Legion.ADF.Auth.Model.UserToken> UserToken { get; }
}
