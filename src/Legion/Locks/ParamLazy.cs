namespace Legion.Locks;

public class ParamLazy<TParam, TResult>
{
	private readonly object _lock = new();
	private bool _isValueCreated = false;
	private TResult? _value;
	private readonly Func<TParam, TResult> _factory;

	public bool IsValueCreated => _isValueCreated;

	public ParamLazy(Func<TParam, TResult> factory)
	{
		Throw.IfArgumentNull(factory);

		_factory = factory;
	}

	public TResult GetValue(TParam param)
	{
		if (_isValueCreated)
			return _value!;

		lock (_lock)
		{
			if (!_isValueCreated)
			{
				_value = _factory(param);
				_isValueCreated = true;
			}
		}

		return _value!;
	}
}
