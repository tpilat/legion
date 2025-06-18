namespace Legion.Model;

public class SaveOptions
{
	public bool? SetConcurrencyToken { get; set; }
	public bool? SetSyncToken { get; set; }
	public bool? SetCorrelationId { get; set; }
	public bool? SetSelfAuditInfo { get; set; }
	public bool? SaveAuditEntries { get; set; }
}
