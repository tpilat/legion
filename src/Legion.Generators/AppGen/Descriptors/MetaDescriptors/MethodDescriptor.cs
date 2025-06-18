using System.Text;

namespace Legion.Generators.AppGen.Descriptors.MetaDescriptors;

public class MethodDescriptor
{
	public TypeDescriptor DeclaringTypeDescriptor { get; }
	public string CSharpReturnType { get; set; }
	public Type ReturnType { get; set; }
	public string Modifiers { get; set; } //public private protected virtual override new abstract
	public bool IsAsync { get; set; }
	public string Name { get; set; }
	public string FullName => $"{DeclaringTypeDescriptor.Name}.{Name}";
	public List<MethodArgumentDescriptor> Arguments { get; }

	public MethodDescriptor(TypeDescriptor typeDescriptor)
	{
		DeclaringTypeDescriptor = typeDescriptor ?? throw new ArgumentNullException(nameof(typeDescriptor));
		Arguments = new List<MethodArgumentDescriptor>();
	}

	public MethodDescriptor AddArgument(MethodArgumentDescriptor argument)
	{
		if (argument == null)
			throw new ArgumentNullException(nameof(argument));

		Arguments.Add(argument);
		return this;
	}

	public MethodDescriptor AddArgument(Action<MethodArgumentDescriptor> configurator)
	{
		if (configurator == null)
			throw new ArgumentNullException(nameof(configurator));

		var argument = new MethodArgumentDescriptor(this);
		configurator.Invoke(argument);
		Arguments.Add(argument);
		return this;
	}

	public string ToDefinition(string ident = null)
	{
		StringBuilder sb = new StringBuilder();

		sb.Append($"{Modifiers}{(IsAsync ? " async" : "")} {CSharpReturnType} {Name}(");

		if (0 < Arguments.Count)
		{
			if (ident != null)
				sb.Append($"{Environment.NewLine}{ident}");

			sb.Append(string.Join($",{(ident == null ? " " : $"{Environment.NewLine}{ident}")}", Arguments.Select(a => a.ToDefinition())));
		}

		sb.Append(")");
		return sb.ToString();
	}

	public string Call(params string[] args)
		=> Call(null, null, MethodCallEnum.Default, args);

	public string Call(MethodCallEnum methodCall, params string[] args)
		=> Call(null, null, methodCall, args);

	public string Call(string prefix, string ident, MethodCallEnum methodCall, params string[] args)
	{
		StringBuilder sb = new StringBuilder();

		if ((methodCall == MethodCallEnum.Default && IsAsync)
			|| methodCall == MethodCallEnum.ForceAwait)
			sb.Append("await ");

		if (!string.IsNullOrWhiteSpace(prefix))
			sb.Append(prefix);

		sb.Append($"{Name}(");

		if (0 < args?.Length)
		{
			if (ident != null)
				sb.Append($"{Environment.NewLine}{ident}");

			sb.Append(string.Join($",{(ident == null ? " " : $"{Environment.NewLine}{ident}")}", args));
		}

		sb.Append(")");
		return sb.ToString();
	}
}
