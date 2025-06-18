using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;
using System.Text;

namespace Legion.Generators.AppGen.Descriptors.MetaDescriptors;

public abstract class TypeDescriptor
{
	public GeneratorContext Context { get; }
	public ModelResult ModelResult { get; }
	public string FileName { get; set; }
	public List<string> Imports { get; private set; }
	public string Name { get; set; }
	public string AsPropertyName => Name;
	public string AsPrivateFieldName => GeneratorHelper.AsPrivateFieldName(Name);
	public string AsFieldName => GeneratorHelper.AsFieldName(Name);
	public string AsParameterName => GeneratorHelper.AsFieldName(Name);
	public List<BasePropertyDescriptor> BaseProperties { get; }
	public Dictionary<string, MethodDescriptor> Methods { get; }

	public abstract void Initialize(string modelName, string contextName);

	public abstract string TargetFolder(string modelName, string contextName);

	public string FilePath(string modelName, string contextName)
		=> Path.Combine(TargetFolder(modelName, contextName), FileName);

	public abstract string Namespace(string modelName, string contextName);

	public abstract string BaseNamespace(string modelName, string contextName);

	public string BaseName(string modelName, string contextName)
		=> $"{BaseNamespace(modelName, contextName)}.{Name}";

	public string FullName(string modelName, string contextName)
		=> $"{Namespace(modelName, contextName)}.{Name}";

	public ModelBase Model { get; set; }
	public CodeGeneratorSettings Settings => Model?.Settings;

	public TypeDescriptor(GeneratorContext context)
	{
		Context = context ?? throw new ArgumentNullException(nameof(context));
		ModelResult = new ModelResult();
		Imports = new List<string>();
		BaseProperties = new List<BasePropertyDescriptor>();
		Methods = new Dictionary<string, MethodDescriptor>();
	}

	public abstract ModelResult Generate(string modelName, string contextName);

	//public string ToActivityName(string token)
	//{
	//	if ("Activity".Equals(token, StringComparison.OrdinalIgnoreCase))
	//		token = token + "_";

	//	return token;
	//}

	public string GetActivityTokenName(EntityBase entity)
		=> entity.ActivityToken;
		//=> ToActivityName(entity.UniqueName);

	public string GetActivityTokenName(QueryEntityBase queryEntity)
		=> queryEntity.ActivityToken;
		//=> ToActivityName(queryEntity.AssociatedTable != null ? queryEntity.AssociatedTable.UniqueName : queryEntity.UniqueName);
}

public abstract class TypeDescriptor<T> : TypeDescriptor
{
	public T Data { get; }

	public TypeDescriptor(T data, GeneratorContext context)
		: base(context)
	{
		Data = data;
	}
}

public static class TypeDescriptorFluentApi
{
	public static T AddImport<T>(this T typeDescriptor, string import)
		where T : TypeDescriptor
	{
		if (typeDescriptor == null)
			throw new ArgumentNullException(nameof(typeDescriptor));
		if (string.IsNullOrWhiteSpace(import))
			throw new ArgumentNullException(nameof(import));

		typeDescriptor.Imports.Add(import);
		return typeDescriptor;
	}

	public static T AddImports<T>(this T typeDescriptor, IEnumerable<string> imports)
		where T : TypeDescriptor
	{
		if (typeDescriptor == null)
			throw new ArgumentNullException(nameof(typeDescriptor));
		if (imports == null)
			throw new ArgumentNullException(nameof(imports));

		foreach (var import in imports)
		{
			if (string.IsNullOrWhiteSpace(import))
				throw new ArgumentException(nameof(import));

			typeDescriptor.Imports.Add(import);
		}

		return typeDescriptor;
	}

	/// <summary>
	/// Build import spustaj az po nastaveni Namespace
	/// </summary>
	/// <returns></returns>
	public static T BuildImports<T>(this T typeDescriptor, string modelName, string contextName)
		where T : TypeDescriptor
	{
		if (typeDescriptor == null)
			throw new ArgumentNullException(nameof(typeDescriptor));

		var imports = typeDescriptor.Imports.Distinct().ToList();

		if (!string.IsNullOrWhiteSpace(typeDescriptor.Namespace(modelName, contextName)))
		{
			var namespaceParts = typeDescriptor.Namespace(modelName, contextName).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder sb = new StringBuilder();
			bool first = true;
			foreach (var nsPart in namespaceParts)
			{
				if (!first)
					sb.Append(".");

				sb.Append(nsPart);
				first = false;
				imports.Remove(sb.ToString());
			}
		}

		imports = imports.OrderBy(x => x).ToList();

		typeDescriptor.Imports.Clear();
		typeDescriptor.Imports.AddRange(imports);

		return typeDescriptor;
	}

	public static T AddBaseProperty<T>(this T typeDescriptor, BasePropertyDescriptor property)
		where T : TypeDescriptor
	{
		if (typeDescriptor == null)
			throw new ArgumentNullException(nameof(typeDescriptor));
		if (property == null)
			throw new ArgumentNullException(nameof(property));

		typeDescriptor.BaseProperties.Add(property);
		return typeDescriptor;
	}

	public static T AddBaseProperty<T>(this T typeDescriptor, Action<BasePropertyDescriptor> configurator)
		where T : TypeDescriptor
	{
		if (typeDescriptor == null)
			throw new ArgumentNullException(nameof(typeDescriptor));
		if (configurator == null)
			throw new ArgumentNullException(nameof(configurator));

		var property = new BasePropertyDescriptor(typeDescriptor);
		configurator.Invoke(property);
		typeDescriptor.BaseProperties.Add(property);
		return typeDescriptor;
	}

	public static T AddMethod<T>(this T typeDescriptor, string methodKey, MethodDescriptor method)
		where T : TypeDescriptor
	{
		if (typeDescriptor == null)
			throw new ArgumentNullException(nameof(typeDescriptor));
		if (method == null)
			throw new ArgumentNullException(nameof(method));

		typeDescriptor.Methods.Add(methodKey, method);
		return typeDescriptor;
	}

	public static T AddMethod<T>(this T typeDescriptor, string methodKey, Action<MethodDescriptor> configurator)
		where T : TypeDescriptor
	{
		if (typeDescriptor == null)
			throw new ArgumentNullException(nameof(typeDescriptor));
		if (configurator == null)
			throw new ArgumentNullException(nameof(configurator));

		var method = new MethodDescriptor(typeDescriptor);
		configurator.Invoke(method);
		typeDescriptor.Methods.Add(methodKey, method);
		return typeDescriptor;
	}
}
