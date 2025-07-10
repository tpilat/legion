namespace Legion.ADF.ServiceBus;

public class ADFServiceBusBuilderContext
{
	private bool _configured;

	public bool Configured()
	{
		if (_configured)
			return false;

		_configured = true;
		return true;
	}
}
