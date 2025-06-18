namespace Legion.Logging;

public static class FallbackLogger
{
	public static Action<Exception?, string>? LogError { get; set; }
}
