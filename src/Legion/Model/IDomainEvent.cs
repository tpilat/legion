namespace Legion.Model;

public interface IDomainEvent : Legion.MessageBus.Messages.IEvent
{
	Guid Id { get; }
	string Namespace { get; }

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	bool Saved { get; }

	void SetSaved();
}
