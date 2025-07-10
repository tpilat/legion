namespace Legion.ADF.ServiceBus.Hosts.Services.Internal;

internal partial class HostService : IDisposable
{
	public class Codes
	{
		public const string Started = nameof(Started);
		public const string Heartbeat = nameof(Heartbeat);
		public const string Disabled = nameof(Disabled);
		public const string InvalidConfig = nameof(InvalidConfig);
		public const string Stopped = nameof(Stopped);
	}
}
