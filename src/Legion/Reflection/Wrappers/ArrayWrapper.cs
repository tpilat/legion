namespace Legion.Reflection;

public class ArrayWrapper
{
	private readonly ArrayTypeWrapper _arrayTypeWrapper;
	private readonly Array _arrayInstance;

	public ArrayWrapper(object arrayInstance)
	{
		_arrayTypeWrapper = ArrayTypeWrapper.Create(arrayInstance);
		_arrayInstance = (arrayInstance as Array)!;
	}

	public ArrayWrapper(ArrayTypeWrapper arrayTypeWrapper, object arrayInstance)
	{
		_arrayTypeWrapper = arrayTypeWrapper;
		_arrayInstance = (arrayInstance as Array)!;
	}

	public int Length()
		=> _arrayInstance.Length;

	public object? GetValue(int index)
		=> _arrayTypeWrapper.Getter(_arrayInstance, index);

	public void SetValue(int index, object? value)
		=> _arrayTypeWrapper.Setter(_arrayInstance, index, value);
}
