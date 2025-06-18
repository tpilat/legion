using Microsoft.EntityFrameworkCore.ChangeTracking;
using Legion.Model.Audit;

namespace Legion.EntityFrameworkCore.Audit.Internal;

internal class AuditEntryInternal : IAuditEntry
{
	public EntityEntry Entry { get; }

	public DateTime CreatedUtc { get; set; }
	public Guid IdAuditOperation { get; set; }
	public Guid? IdUser { get; set; }
	public string TableName { get; }
	public Dictionary<string, object?> KeyValuesDict { get; }
	public Dictionary<string, object?> OldValuesDict { get; }
	public Dictionary<string, object?> NewValuesDict { get; }
	public List<PropertyEntry> TemporaryProperties { get; }
	public Guid AuditCorrelationId { get; set; }
	public List<string> ChangedColumns { get; }
	public bool HasTemporaryProperties => 0 < TemporaryProperties.Count;
	public string? TraceFrame { get; set; }
	public Guid? CorrelationId { get; set; }

	public string? PrimaryKey => Newtonsoft.Json.JsonConvert.SerializeObject(KeyValuesDict);
	public string? OldValues => OldValuesDict.Count == 0 ? null : Newtonsoft.Json.JsonConvert.SerializeObject(OldValuesDict);
	public string? NewValues => NewValuesDict.Count == 0 ? null : Newtonsoft.Json.JsonConvert.SerializeObject(NewValuesDict);
	public string? AffectedColumns => ChangedColumns.Count == 0 ? null : Newtonsoft.Json.JsonConvert.SerializeObject(ChangedColumns);

	public AuditEntryInternal(EntityEntry entry)
	{
		Throw.IfArgumentNull(entry);

		Entry = entry;
		TableName = entry.Entity.GetType().Name;
		KeyValuesDict = [];
		OldValuesDict = [];
		NewValuesDict = [];
		TemporaryProperties = [];
		ChangedColumns = [];
	}
}
