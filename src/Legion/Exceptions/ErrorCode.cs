namespace Legion.Exceptions;

public class ErrorCode : IErrorCode
{
	public string Code { get; }
	public string Message { get; }
	public string? Description { get; }

	public ErrorCode(string code, string message, string? description = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(code);

#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(message);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		Code = code;
		Message = message;
		Description = description;
	}
}
