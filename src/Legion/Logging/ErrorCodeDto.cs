namespace Legion.Logging;

public class ErrorCodeDto : IErrorCode
{
	public string Code { get; set; }
	public string Message { get; set; }
	public string? Description { get; set; }

	public ErrorCodeDto()
	{
	}

	public ErrorCodeDto(IErrorCode? other)
	{
		if (other != null)
		{
			Code = other.Code;
			Message = other.Message;
			Description = other.Description;
		}
	}
}
