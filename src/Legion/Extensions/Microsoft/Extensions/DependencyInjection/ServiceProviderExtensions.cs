using Legion.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.Extensions;

public static partial class ServiceProviderExtensions
{
	public static async Task RunStartupTasksAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(serviceProvider);

		var startupTasks = serviceProvider.GetServices<IStartupTask>();

		foreach (var startupTask in startupTasks)
			await startupTask.ExecuteAsync(serviceProvider, cancellationToken);
	}
}
