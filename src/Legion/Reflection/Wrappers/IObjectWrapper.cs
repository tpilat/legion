namespace Legion.Reflection;

public interface IObjectWrapper
{
	ITypeWrapper TypeWrapper { get; }

	object? this[string propertyFieldName] { get; set; }

	object? GetValue(string memberName);

	object? GetNonStaticValue(string memberName);

	object? GetStaticValue(string memberName);

	void SetValue(string memberName, object? value);

	void SetNonStaticValue(string memberName, object? value);

	void SetStaticValue(string memberName, object? value);
}
