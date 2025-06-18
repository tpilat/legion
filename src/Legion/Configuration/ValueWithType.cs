namespace Legion.Configuration;

public class ValueWithType
{
	public string? Value { get; set; }
	public Type? Type { get; set; }
	public int Order { get; set; }

	public ValueWithType()
	{
	}

	public ValueWithType(string? value, Type? type, int order)
	{
		Value = value;
		Type = type;
		Order = order;
	}

	public override string? ToString()
		=> Value;
}
