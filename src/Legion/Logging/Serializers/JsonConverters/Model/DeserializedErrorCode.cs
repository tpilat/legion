namespace Legion.Logging.Serializers.JsonConverters.Model;

public class DeserializedErrorCode : IErrorCode
{
	public string Code { get; set; }
	public string Message { get; set; }
	public string? Description { get; set; }
}
