using Legion.Configuration;
using Legion.Extensions;
using Legion.Serializer;
using System.Text;

namespace TestEnterpriseServiceBus;

internal static class Test
{
	public static void Run()
	{
		var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testjson.json"));
		var jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
		var jsonFormatted = jObject.ToString();
		File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testjson_FORMATTED.json"), jsonFormatted, new System.Text.UTF8Encoding(false));

		var data_net8 = Legion.Configuration.JsonConfigurationParser.Parse(json, true, null, null, true);
		SaveData(data_net8, "data_net8");

		var data_newtonsoft = Legion.Configuration.JsonConfigurationParser_Newtonsoft.Parse(json, true, null, Microsoft.Extensions.Configuration.ConfigurationPath.KeyDelimiter, true);
		SaveData(data_newtonsoft, "data_newtonsoft");
		var json2 = Legion.Configuration.ConfigurationToJsonConverter.ToJsonString(data_newtonsoft, true, null, true, false);
		File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testjson_CONVERTED.json"), json2, new System.Text.UTF8Encoding(false));

	}

	private static void SaveData(Dictionary<string, Legion.Configuration.ValueWithType> data, string name)
	{
		var aa = JsonSerializerHelper.Serialize(data, new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented });
		File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{name}.json_dict"), aa, new System.Text.UTF8Encoding(false));

		var sb = new StringBuilder();
		foreach (var key in data.Keys.OrderBy(x => x))
		{
			var value = data[key];
			sb.AppendLine($"{key} = {(value.Value == null ? "null" : $"\"{value.Value}\"")}{(value.Type == null ? "" : $" | {value.Type.Name}")}");
		}

		File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{name}.txt"), sb.ToString(), new System.Text.UTF8Encoding(false));
	}
}
