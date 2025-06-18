using Legion.Extensions;

namespace Legion.ADF.ESB.TestConsole;

internal class Program
{
	static async Task Main(string[] args)
	{
		try
		{
			await Test.RunAsync();
		}
		catch (Exception ex)
		{
			var error = ex.ToStringTrace();
			await Console.Out.WriteLineAsync(error);
		}
	}
}
