namespace Legion.ADF.Messaging.MessageBox;

public abstract class MessageBoxBaseEntity : Legion.Model.IEntity
{
	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	public bool __IsNewObject {get; set; }

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	private readonly List<Legion.Model.IDomainEvent> _domainEvents = [];

	public virtual IReadOnlyDictionary<string, string>? GetIgnoredAuditPropertiesWithDefaultValue()
		=> null;

	public abstract string? GetPrimaryKeyValue();

	public virtual List<string>? GetIgnoredSynchronizationProperties()
		=> null;

	public IReadOnlyList<Legion.Model.IDomainEvent> GetDomainEvents()
		=> _domainEvents.Where(de => !de.Saved).ToList();

	public void ClearDomainEvents()
	{
		foreach (var domainEvent in _domainEvents)
			domainEvent.SetSaved();

		_domainEvents.Clear();
	}

	protected void RaiseDomainEventOnCommit(Legion.Model.IDomainEvent domainEvent) =>
		_domainEvents.Add(domainEvent);
}
