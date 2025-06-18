namespace Legion.ADF.Audit.DTOs;

public class ApplicationEntryTokenHistory
{
	public DateTime CreatedUtc { get; set; }

	public string? MethodInfo { get; set; }

	public string? AggregateName { get; set; }

	public string? Description { get; set; }
}
