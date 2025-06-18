using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Config;

public partial interface IConfigUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Config.Model.Repositories.IConfigurationClassRepository ConfigurationClassRepository { get; }

	Legion.ADF.Config.Model.Repositories.IConfigurationKeyValueRepository ConfigurationKeyValueRepository { get; }
}
