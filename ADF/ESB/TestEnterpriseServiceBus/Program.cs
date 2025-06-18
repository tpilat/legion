using Legion.ADF.Config.PostgreSQL.Configuration;
using Legion.ADF.Config.Extensions;
using Legion.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace TestEnterpriseServiceBus;

public class Program
{
	public static async Task Main(string[] args)
	{
		Test.Run();

		var hostBuilder =
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(
					cfg => cfg.AddDBConfiguration(
						() => new DBConfigurationManager(
							opt => opt.UseNpgsql(string.Format("Host=host.docker.internal;Database=legion_adf_esb;Port=35432;Username=legion_adf_esb_usr;Password=legion_adf_esb_pwd", System.Environment.GetEnvironmentVariable("PGPASSWORD")))))
															//TODO: ConnectionString
				)
				.UseWindowsService(options =>
				{
					options.ServiceName = $"{nameof(TestEnterpriseServiceBus)}";
				})
				.ConfigureServices((hostContext, services) =>
					new Startup(hostContext.Configuration)
						.ConfigureServices(services));

		var host = hostBuilder.Build();
		await host.RunWithTasksAsync();
	}
}