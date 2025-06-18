namespace Legion.Calendar;

public class CronTimerSettings
{
	public string Expression { get; }

	public bool IncludeSeconds { get; }

	public CronExpression CronExpression { get; }

	public CronTimerSettings(string expression, bool includeSeconds)
	{
		Throw.IfArgumentNullOrWhiteSpace(expression);

		Expression = expression;
		IncludeSeconds = includeSeconds;
		CronExpression = CronExpression.Parse(expression, includeSeconds ? CronFormat.IncludeSeconds : CronFormat.Standard);
	}
}
