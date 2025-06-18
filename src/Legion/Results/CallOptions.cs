using Legion.Extensions;
using Legion.Policy;

namespace Legion;

public class CallOptions
{
	/// <summary>
	/// Applicable only for <see cref="IResult"/>. Cannot use with <see cref="IResult{TData}"/>
	/// </summary>
	public bool FireAndForget { get; private set; }

	/// <summary>
	/// Applicable only if <see cref="FireAndForget"/> is false.
	/// </summary>
	public TimeSpan? Timeout { get; private set; }

	/// <summary>
	/// Applicable only if <see cref="FireAndForget"/> is false.
	/// </summary>
	public IRetryOptions? RetryOptions { get; private set; }

	private CallOptions()
	{
	}

	public static CallOptions CreateFireAndForget()
		=> new()
		{
			FireAndForget = true,
			Timeout = null,
			RetryOptions = null
		};

	public static CallOptions Create(TimeSpan timeout)
	{
		Throw.IfArgumentIsLessThanOrEqual(timeout, TimeSpan.Zero);

		return new()
		{
			FireAndForget = false,
			Timeout = timeout,
			RetryOptions = null
		};
	}

	public static CallOptions Create(IRetryOptions retryOptions)
	{
		Throw.IfArgumentNull(retryOptions);

		return new()
		{
			FireAndForget = false,
			Timeout = null,
			RetryOptions = retryOptions
		};
	}

	public static CallOptions Create(IRetryOptions retryOptions, TimeSpan timeout)
	{
		Throw.IfArgumentNull(retryOptions);
		Throw.IfArgumentIsLessThanOrEqual(timeout, TimeSpan.Zero);

		return new()
		{
			FireAndForget = false,
			Timeout = timeout,
			RetryOptions = retryOptions
		};
	}

	public void FireAndForgetExtension<T1, T2, T3>(
		Action<T1, T2, T3> action,
		T1 t1,
		T2 t2,
		T3 t3)
	{
		Task.Run(() =>
		{
			try
			{
				action(t1, t2, t3);
			}
			catch { }
		})
		.OrTimeoutAsync(TimeSpan.FromDays(1));
	}
}
