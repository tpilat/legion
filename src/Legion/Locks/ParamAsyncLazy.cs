using Legion.Threading;

namespace Legion.Locks;

public class ParamAsyncLazy<TParam, TResult>
{
	private readonly Func<TParam, Task<TResult>> _factory;
	private static readonly AsyncLock _servicesLock = new();
	private Task<TResult>? _task;
	private bool _isValueCreated;

	public bool IsValueCreated => _isValueCreated;

	public ParamAsyncLazy(Func<TParam, Task<TResult>> factory)
	{
		Throw.IfArgumentNull(factory);

		_factory = factory;
	}

	public async Task<TResult> GetValueAsync(TParam param)
	{
		if (_isValueCreated)
			return await _task!;

		using (await _servicesLock.LockAsync())
		{
			if (!_isValueCreated)
			{
				_task = _factory(param);
				_isValueCreated = true;
			}
		}

		return await _task!;
	}
}
