namespace Legion;

internal class ApplicationEntryScopeContextStore
{
	private readonly IApplicationEntryScopeContext _applicationEntryScopeContext;

	public IApplicationEntryScopeContext ApplicationEntryScopeContext => _applicationEntryScopeContext;

	public ApplicationEntryScopeContextStore(IApplicationEntryScopeContext applicationEntryScopeContext)
	{
		Throw.IfArgumentNull(applicationEntryScopeContext);

		_applicationEntryScopeContext = applicationEntryScopeContext;
	}

	public IApplicationEntryScopeContext GetApplicationEntryScopeContextClone()
	{
		var clone = _applicationEntryScopeContext.Clone();
		return clone;
	}
}
