using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Shared;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;

namespace Legion.Generators.AppGen.Descriptors;

public abstract class EnumDescriptor : TypeDescriptor
{
	protected readonly Dictionary<int, string> _enumData;

	public int LastId { get; protected set; }

	public ConvertEntity ConvertEntity { get; set; }

	public string BaseEntityname { get; protected set; }

	public EnumDescriptor(GeneratorContext context)
		: base(context)
	{
		_enumData = new Dictionary<int, string>();
	}

	public EnumDescriptor AddValue(int intValue, string value)
	{
		if (_enumData.ContainsKey(intValue))
			throw new InvalidOperationException($"An element with the same key '{intValue}' already exists. Associated value is {value}");

		if (_enumData.ContainsValue(value))
			throw new InvalidOperationException($"An element with the same value '{value}' already exists. Associated key is {intValue}");

		_enumData.Add(intValue, value);
		return this;
	}

	public List<KeyValuePair<int, string>> GetAllValues()
		=> _enumData.OrderBy(kvp => kvp.Key).ToList();

	public abstract void SetEnumData();

	public override ModelResult Generate(string modelName, string contextName)
	{
		SetEnumData();

		if (_enumData.Count == 0)
			return new ModelResult(); //TODO TOM: minimal settings - v settingsoch je zadane, ze sa ma napr. generovat iba zakladny EF model
									  //ModelResult.AddError($"{Namespace}.{Name}: {nameof(EnumDescriptor)}", "Enum has no values.");

		var result =
			GeneratorInvoker
				.Generate<EnumGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EnumDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
