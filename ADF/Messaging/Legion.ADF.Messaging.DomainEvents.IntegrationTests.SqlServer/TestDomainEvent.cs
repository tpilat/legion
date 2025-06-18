using Legion.Model;

namespace Legion.ADF.Messaging.DomainEvents.IntegrationTests.SqlServer;

public record TestDomainEvent : DomainEventBase, IDomainEvent, Legion.MessageBus.Messages.IEvent
{
	public string TestValue { get; set; }

	public TestDomainEvent(string testValue)
		: base()
	{
		TestValue = testValue;
	}

	public bool IsTheSame(object obj)
	{
		if (obj is not TestDomainEvent testDomainEvent)
			return false;

		return testDomainEvent.Id == Id
			&& testDomainEvent.TestValue == TestValue;
	}
}
