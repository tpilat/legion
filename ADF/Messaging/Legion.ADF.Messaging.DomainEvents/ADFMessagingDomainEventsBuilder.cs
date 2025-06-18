namespace Legion.ADF.Messaging.DomainEvents;

public class ADFMessagingDomainEventsBuilder
{
	public ADFMessagingBuilder ADFMessagingBuilder { get; }
	internal bool ProcessingServiceAllowed { get; set; }

	public ADFMessagingDomainEventsBuilder(ADFMessagingBuilder adfMessagingBuilder)
	{
		Throw.IfArgumentNull(adfMessagingBuilder);

		ADFMessagingBuilder = adfMessagingBuilder;
	}
}
