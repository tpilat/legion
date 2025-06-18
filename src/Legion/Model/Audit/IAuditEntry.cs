namespace Legion.Model.Audit;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IAuditEntry
{
	DateTime CreatedUtc { get; }
	Guid IdAuditOperation { get; }
	string TableName { get; }
	Guid? IdUser { get; }
	string? PrimaryKey { get; }
	string? OldValues { get; }
	string? NewValues { get; }
	string? AffectedColumns { get; }
	Guid AuditCorrelationId { get; }
	string? TraceFrame { get; }
	Guid? CorrelationId { get; }
}
