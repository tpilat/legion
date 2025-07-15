namespace Legion.ADF.ServiceBus.Services.Internal;

internal partial class JobService
{
	public class Codes
	{
		public const string Started = nameof(Started);
		public const string Disabling = nameof(Disabling);
		public const string Disabled = nameof(Disabled);
		public const string Running = nameof(Running);
		public const string Idle = nameof(Idle);
		public const string Canceled = nameof(Canceled);
	}
}
