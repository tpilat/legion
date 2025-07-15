namespace Legion.ADF.Cache.DTOs;

public class CachedValueDto
{
	public string Value { get; set; }
	public Guid? RowVersion { get; set; }
}
