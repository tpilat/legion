namespace Legion;

internal class InvocationResult : IInvocationResult
{
	public IInvocationContext InvocationContext { get; set; }
	public bool DelegateWasCalled { get; set; }
	public IResult Result { get; set; }
	public bool IsUnhandledException { get; set; }
	public string DelegateMethodName { get; set; }
	public long ElapsedMilliseconds { get; set; }
}
