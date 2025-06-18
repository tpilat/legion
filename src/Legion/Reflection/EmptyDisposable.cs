namespace Legion.Reflection;

public class EmptyDisposable : IDisposable
{
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
	public void Dispose()
	{
	}
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
}
