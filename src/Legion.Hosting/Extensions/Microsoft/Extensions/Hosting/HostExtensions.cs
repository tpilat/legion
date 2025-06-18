using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class HostExtensions
{
	public static async Task RunWithTasksAsync(this IHost host, CancellationToken cancellationToken = default)
	{
		var serviceProvider = host.Services;

		using var scope = serviceProvider.CreateScope();
		var sp = scope.ServiceProvider;
		await sp.RunStartupTasksAsync(cancellationToken);

		await host.RunAsync(cancellationToken);
	}
}
