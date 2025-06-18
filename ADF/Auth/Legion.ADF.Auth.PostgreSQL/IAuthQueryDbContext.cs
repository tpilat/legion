using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auth.PostgreSQL;

public interface IAuthQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Auth.Model.VwUser> VwUser { get; set; }
}
