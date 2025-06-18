namespace TestEnterpriseServiceBus.Adapters.SocPoist.Http.Requests;

public class JsonSerializer
{
	public static readonly Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings =
		new()
		{
			Formatting = Newtonsoft.Json.Formatting.None,
			DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,
			NullValueHandling = Newtonsoft.Json.NullValueHandling.Include
		};
}