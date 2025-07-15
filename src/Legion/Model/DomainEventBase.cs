using Legion.Extensions;

namespace Legion.Model;

public abstract record DomainEventBase : IDomainEvent, Legion.MessageBus.Messages.IEvent
{
	public virtual Guid Id { get; protected set; }
	public virtual string Namespace { get; protected set; }

	public virtual bool Saved { get; private set; }

	protected DomainEventBase()
	{
		Id = GlobalContext.Instance.NewGuid();
		Namespace = this.GetType().GetSimplifiedAssemblyQualifiedName();
		Saved = false;
	}

	public virtual void SetSaved()
	{
		Saved = true;
	}
}
