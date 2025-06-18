namespace Legion;

public interface IErrorCode
{
	public string Code { get; }
	public string Message { get; }
	public string? Description { get; }
}
