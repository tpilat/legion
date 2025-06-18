namespace TestEnterpriseServiceBus.Adapters.SocPoist.Messages;

public record SocPoistResponse
{
	public byte[] CSV { get; set; }
	public int Week { get; set; }
	public int Year { get; set; }
}
