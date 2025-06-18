using Legion.Model;

namespace Legion.ADF.Messaging.DomainEvents.IntegrationTests.PostgreSQL;

public record FailureTestDomainEvent : DomainEventBase, IDomainEvent, Legion.MessageBus.Messages.IEvent
{
	public string TestValue { get; set; }

	public FailureTestDomainEvent(string testValue)
		: base()
	{
		TestValue = testValue;
	}

	public bool IsTheSame(object obj)
	{
		if (obj is not FailureTestDomainEvent failureTestDomainEvent)
			return false;

		return failureTestDomainEvent.Id == Id
			&& failureTestDomainEvent.TestValue == TestValue;
	}
}
