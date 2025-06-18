namespace Legion.ADF.Messaging;

public class ADFMessagingBuilderContext
{
	private bool _addedDomainEvents;
	private bool _addedInbox;
	private bool _addedOutbox;
	private bool _addedMessageBox;

	public bool AddDomainEvents()
	{
		if (_addedDomainEvents)
			return false;

		_addedDomainEvents = true;
		return true;
	}

	public bool AddInbox()
	{
		if (_addedInbox)
			return false;

		_addedInbox = true;
		return true;
	}

	public bool AddOutbox()
	{
		if (_addedOutbox)
			return false;

		_addedOutbox = true;
		return true;
	}

	public bool AddMessageBox()
	{
		if (_addedMessageBox)
			return false;

		_addedMessageBox = true;
		return true;
	}
}
