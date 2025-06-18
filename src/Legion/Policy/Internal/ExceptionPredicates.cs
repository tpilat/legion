namespace Legion.Policy.Internal;

internal class ExceptionPredicates
{
	public static readonly ExceptionPredicates None = new();

	private List<ExceptionPredicate>? _predicates;

	internal void Add(ExceptionPredicate predicate)
	{
		_predicates ??= [];
		_predicates.Add(predicate);
	}

	public Exception? FirstMatchOrDefault(Exception ex)
		=> _predicates?.Select(predicate => predicate(ex)).FirstOrDefault(e => e != null);
}
