using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Legion.Configuration;

public sealed class DictionaryConfigurationProvider : IConfigurationProvider
{
	private readonly IDictionary<string, string?> Data;

	private ConfigurationReloadToken _reloadToken = new();

	public DictionaryConfigurationProvider(IDictionary<string, string?> config)
	{
		Throw.IfArgumentNull(config);

		Data = config;
	}

	public void Set(string key, string? value)
		=> Data[key] = value;

	public bool TryGet(string key, out string? value)
		=> Data.TryGetValue(key, out value);

	public void Load()
	{
	}

	public IEnumerable<string> GetChildKeys(
		IEnumerable<string> earlierKeys,
		string? parentPath)
	{
		var results = new List<string>();

		if (parentPath is null)
		{
			foreach (KeyValuePair<string, string?> kv in Data)
			{
				results.Add(Segment(kv.Key, 0));
			}
		}
		else
		{
			foreach (KeyValuePair<string, string?> kv in Data)
			{
				if (kv.Key.Length > parentPath.Length &&
					kv.Key.StartsWith(parentPath, StringComparison.OrdinalIgnoreCase) &&
					kv.Key[parentPath.Length] == ':')
				{
					results.Add(Segment(kv.Key, parentPath.Length + 1));
				}
			}
		}

		results.AddRange(earlierKeys);

		results.Sort(ConfigurationKeyComparer.Comparison);

		return results;
	}

	private static string Segment(string key, int prefixLength)
	{
		int indexOf = key.IndexOf(':', prefixLength);
		return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
	}

	public IChangeToken GetReloadToken()
		=> _reloadToken;
	
	public void OnReload()
	{
		ConfigurationReloadToken previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
		previousToken.OnReload();
	}

	public override string ToString()
		=> GetType().Name;
}

internal class ConfigurationReloadToken : IChangeToken
{
	private readonly CancellationTokenSource _cts = new();

	public bool ActiveChangeCallbacks { get; private set; } = true;

	public bool HasChanged => _cts.IsCancellationRequested;

	public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
	{
		return ChangeCallbackRegistrar.UnsafeRegisterChangeCallback(
			callback,
			state,
			_cts.Token,
			static s => s.ActiveChangeCallbacks = false, // Reset the flag to indicate to future callers that this wouldn't work.
			this);
	}

	public void OnReload() => _cts.Cancel();
}

internal class ConfigurationKeyComparer : IComparer<string>
{
	private const char KeyDelimiter = ':';

	public static ConfigurationKeyComparer Instance { get; } = new ConfigurationKeyComparer();

	internal static Comparison<string> Comparison { get; } = Instance.Compare;

	public int Compare(string? x, string? y)
	{
		ReadOnlySpan<char> xSpan = x.AsSpan();
		ReadOnlySpan<char> ySpan = y.AsSpan();

		xSpan = SkipAheadOnDelimiter(xSpan);
		ySpan = SkipAheadOnDelimiter(ySpan);

		// Compare each part until we get two parts that are not equal
		while (!xSpan.IsEmpty && !ySpan.IsEmpty)
		{
			int xDelimiterIndex = xSpan.IndexOf(KeyDelimiter);
			int yDelimiterIndex = ySpan.IndexOf(KeyDelimiter);

			int compareResult = Compare(
				xDelimiterIndex == -1 ? xSpan : xSpan.Slice(0, xDelimiterIndex),
				yDelimiterIndex == -1 ? ySpan : ySpan.Slice(0, yDelimiterIndex));

			if (compareResult != 0)
			{
				return compareResult;
			}

			xSpan = xDelimiterIndex == -1 ? default :
				SkipAheadOnDelimiter(xSpan.Slice(xDelimiterIndex + 1));
			ySpan = yDelimiterIndex == -1 ? default :
				SkipAheadOnDelimiter(ySpan.Slice(yDelimiterIndex + 1));
		}

		return xSpan.IsEmpty ? (ySpan.IsEmpty ? 0 : -1) : 1;

		static ReadOnlySpan<char> SkipAheadOnDelimiter(ReadOnlySpan<char> a)
		{
			while (!a.IsEmpty && a[0] == KeyDelimiter)
			{
				a = a.Slice(1);
			}
			return a;
		}

		static int Compare(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
		{
			bool aIsInt = int.TryParse(a.ToString(), out int value1);
			bool bIsInt = int.TryParse(b.ToString(), out int value2);
			int result;

			if (!aIsInt && !bIsInt)
			{
				// Both are strings
				result = a.CompareTo(b, StringComparison.OrdinalIgnoreCase);
			}
			else if (aIsInt && bIsInt)
			{
				// Both are int
				result = value1 - value2;
			}
			else
			{
				// Only one of them is int
				result = aIsInt ? -1 : 1;
			}

			return result;
		}
	}
}

internal static class ChangeCallbackRegistrar
{
	/// <returns>The <see cref="CancellationToken"/> registration.</returns>
	internal static IDisposable UnsafeRegisterChangeCallback<T>(Action<object?> callback, object? state, CancellationToken token, Action<T> onFailure, T onFailureState)
	{
		// Don't capture the current ExecutionContext and its AsyncLocals onto the token registration causing them to live forever
		bool restoreFlow = false;
		if (!ExecutionContext.IsFlowSuppressed())
		{
			ExecutionContext.SuppressFlow();
			restoreFlow = true;
		}

		try
		{
			return token.Register(callback, state);
		}
		catch (ObjectDisposedException)
		{
			onFailure(onFailureState);
		}
		finally
		{
			// Restore the current ExecutionContext
			if (restoreFlow)
			{
				ExecutionContext.RestoreFlow();
			}
		}

		return EmptyDisposable.Instance;
	}
}

internal sealed class EmptyDisposable : IDisposable
{
	public static EmptyDisposable Instance { get; } = new EmptyDisposable();

	private EmptyDisposable()
	{
	}

	public void Dispose()
	{
	}
}