using System.Text;

namespace Legion;

public class CONFIGURATION
{
	private static readonly Lazy<CONFIGURATION> _current = new(() => new CONFIGURATION());

	public static CONFIGURATION Current => _current.Value;

	private CONFIGURATION()
	{
	}

	public Action<StringBuilder, Exception>? SerializeFaultExceptionDelegate { get; private set; }

	private readonly object _lockSerializeFaultExceptionDelegate = new();
	public CONFIGURATION WithSerializeFaultExceptionDelegate(Action<StringBuilder, Exception> @delegate)
	{
		lock (_lockSerializeFaultExceptionDelegate)
		{
			SerializeFaultExceptionDelegate = @delegate;
		}

		return this;
	}
}
