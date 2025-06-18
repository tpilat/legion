using Legion.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
		where T : class, IStartupTask
		=> services.AddTransient<IStartupTask, T>();
}
