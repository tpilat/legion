namespace Legion.ADF.Cache.RestApi.Client.Requests;

public class JsonSerializer
{
	public static readonly Newtonsoft.Json.JsonSerializerSettings JsonSerializerOptions =
		new()
		{
			Formatting = Newtonsoft.Json.Formatting.None
		};
}
