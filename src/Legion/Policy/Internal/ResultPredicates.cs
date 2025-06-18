namespace Legion.Policy.Internal;

internal class ResultPredicates<TResult>
{
	public static readonly ResultPredicates<TResult> None = new();

	private List<ResultPredicate<TResult>>? _predicates;

	internal void Add(ResultPredicate<TResult> predicate)
	{
		_predicates ??= [];
		_predicates.Add(predicate);
	}

	public bool AnyMatch(TResult result)
	{
		if (_predicates == null)
			return false;

		return _predicates.Any(predicate => predicate(result));
	}
}

