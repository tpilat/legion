using Legion.Model;

namespace Legion.ADF.Messaging.DomainEvents.IntegrationTests.SqlServer;

public record TestDomainEventWithNoHandler : DomainEventBase, IDomainEvent, Legion.MessageBus.Messages.IEvent
{
	public string TestValue { get; set; }

	public TestDomainEventWithNoHandler(string testValue)
		: base()
	{
		TestValue = testValue;
	}

	public bool IsTheSame(object obj)
	{
		if (obj is not TestDomainEventWithNoHandler testDomainEventWithNoHandler)
			return false;

		return testDomainEventWithNoHandler.Id == Id
			&& testDomainEventWithNoHandler.TestValue == TestValue;
	}
}
