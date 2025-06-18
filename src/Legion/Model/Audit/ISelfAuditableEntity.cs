namespace Legion.Model.Audit;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface ISelfAuditableEntity : IEntity
{
	DateTime AuditCreatedUtc { get; }
	Guid? IdAuditCreatedBy { get; }
	DateTime? AuditModifiedUtc { get; }
	Guid? IdAuditModifiedBy { get; }

	void SetAuditCreated(DateTime auditCreatedUtc, Guid? idAuditCreatedBy);

	void SetAuditModified(DateTime auditModifiedUtc, Guid? idAuditModifiedBy);
}
