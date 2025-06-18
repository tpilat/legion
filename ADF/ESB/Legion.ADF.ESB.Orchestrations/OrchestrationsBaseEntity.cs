namespace Legion.ADF.ESB.Orchestrations;

public abstract class OrchestrationsBaseEntity : Legion.Model.IEntity
{
	private readonly List<Legion.Model.IDomainEvent> _domainEvents = [];

	public virtual IReadOnlyDictionary<string, string>? GetIgnoredAuditPropertiesWithDefaultValue()
		=> null;

	public virtual List<string>? GetIgnoredSynchronizationProperties()
		=> null;

	public IReadOnlyList<Legion.Model.IDomainEvent> GetDomainEvents()
		=> _domainEvents.ToList();

	public void ClearDomainEvents() => _domainEvents.Clear();

	protected void RaiseDomainEvent(Legion.Model.IDomainEvent domainEvent) =>
		_domainEvents.Add(domainEvent);
}
