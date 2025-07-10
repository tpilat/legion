namespace Legion.AspNetCore.WebApi;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ApiRoutePrefixAttribute : Attribute
{
	public string Version { get; }

	public ApiRoutePrefixAttribute(string version)
	{
		Version = version;
	}
}
