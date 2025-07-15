using Legion.Threading;

namespace Legion.Locks;

public class AsyncLazy<T>
{
	private readonly Func<Task<T>> _factory;
	private static readonly AsyncLock _servicesLock = new();

	private Task<T>? _task;
	private bool _isValueCreated;

	public bool IsValueCreated => _isValueCreated;
	public Task<T> ValueAsync => GetValueAsync();

	public AsyncLazy(Func<Task<T>> factory)
	{
		Throw.IfArgumentNull(factory);

		_factory = factory;
	}

	public async Task<T> GetValueAsync()
	{
		if (_isValueCreated)
			return await _task!;

		using (await _servicesLock.LockAsync())
		{
			if (!_isValueCreated)
			{
				_task = _factory();
				_isValueCreated = true;
			}
		}

		return await _task!;
	}
}
