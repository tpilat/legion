namespace Legion.ADF.Auditing;

public abstract class EntityBase : Legion.Model.IEntity
{
	private readonly List<Legion.Model.IDomainEvent> _domainEvents = new();

	public virtual IReadOnlyList<string>? GetIgnoredAuditProperties()
		=> null;

	public virtual List<string>? GetIgnoredSynchronizationProperties()
		=> null;

	public IReadOnlyList<Legion.Model.IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

	public void ClearDomainEvents() => _domainEvents.Clear();

	protected void RaiseDomainEvent(Legion.Model.IDomainEvent domainEvent) =>
		_domainEvents.Add(domainEvent);
}
