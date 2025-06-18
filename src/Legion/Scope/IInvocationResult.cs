namespace Legion;

public interface IInvocationResult
{
	IInvocationContext InvocationContext { get; }
	bool DelegateWasCalled { get; }
	IResult Result { get; }
	bool IsUnhandledException { get; }
	string DelegateMethodName { get; }
	long ElapsedMilliseconds { get; }
}
