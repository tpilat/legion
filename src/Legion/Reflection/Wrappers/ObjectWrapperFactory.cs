using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Legion.Reflection;

public static class ObjectWrapperFactory
{
	private static readonly Type _bindingFlagsType = typeof(BindingFlags);
	private static readonly ConcurrentDictionary<Type, Func<object, object, BindingFlags, object>> _cache = [];

	public static IObjectWrapper Create<T>(T instance, BindingFlags bindingFlags)
	{
		Throw.IfArgumentNull(instance);

		var type = instance.GetType();

		var ctor = _cache.GetOrAdd(type, t =>
		{
			Type objectWrapperType = typeof(ObjectWrapper<>).MakeGenericType(type);
			Type typeWrapperType = typeof(TypeWrapper<>).MakeGenericType(type);

#if NET8_0_OR_GREATER
			var constructorInfo = objectWrapperType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, [typeWrapperType, type, _bindingFlagsType]);
#else
			var constructorInfo = objectWrapperType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, [typeWrapperType, type, _bindingFlagsType], null);
#endif

			Throw.IfNull(constructorInfo, (IErrorCode?)null, $"{nameof(type)} = {type.FullName}");

			var typeWrapperExpression = Expression.Parameter(typeof(object), "typeWrapper");
			var instanceExpression = Expression.Parameter(typeof(object), "instnace");
			var bindingFlagsExpression = Expression.Parameter(_bindingFlagsType, "bindingFlags");

			var convertedTypeWrapperParam = Expression.Convert(typeWrapperExpression, typeWrapperType);
			var convertedInstanceParam = Expression.Convert(instanceExpression, type);
			var bindingFlagsParam = Expression.Convert(bindingFlagsExpression, _bindingFlagsType);

			NewExpression newExpression = Expression.New(constructorInfo, convertedTypeWrapperParam, convertedInstanceParam, bindingFlagsParam);

			var lambda = Expression.Lambda<Func<object, object, BindingFlags, object>>(
				Expression.Convert(newExpression, typeof(object)),
				typeWrapperExpression,
				instanceExpression,
				bindingFlagsExpression
			).Compile();

			return lambda;
		});

		return (IObjectWrapper)ctor(
			null!,
			instance,
			bindingFlags);
	}

	public static IObjectWrapper Create(object instance, BindingFlags bindingFlags)
		=> Create(instance?.GetType()!, instance!, bindingFlags);

	public static IObjectWrapper Create(
		Type type,
		object instance,
		BindingFlags bindingFlags)
	{
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(instance);

		var ctor = _cache.GetOrAdd(type, t =>
		{
			Type objectWrapperType = typeof(ObjectWrapper<>).MakeGenericType(type);
			Type typeWrapperType = typeof(TypeWrapper<>).MakeGenericType(type);

#if NET8_0_OR_GREATER
			var constructorInfo = objectWrapperType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, [typeWrapperType, type, _bindingFlagsType]);
#else
			var constructorInfo = objectWrapperType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, [typeWrapperType, type, _bindingFlagsType], null);
#endif

			Throw.IfNull(constructorInfo, (IErrorCode?)null, $"{nameof(type)} = {type.FullName}");

			var typeWrapperExpression = Expression.Parameter(typeof(object), "typeWrapper");
			var instanceExpression = Expression.Parameter(typeof(object), "instnace");
			var bindingFlagsExpression = Expression.Parameter(_bindingFlagsType, "bindingFlags");

			var convertedTypeWrapperParam = Expression.Convert(typeWrapperExpression, typeWrapperType);
			var convertedInstanceParam = Expression.Convert(instanceExpression, type);
			var bindingFlagsParam = Expression.Convert(bindingFlagsExpression, _bindingFlagsType);

			NewExpression newExpression = Expression.New(constructorInfo, convertedTypeWrapperParam, convertedInstanceParam, bindingFlagsParam);

			var lambda = Expression.Lambda<Func<object, object, BindingFlags, object>>(
				Expression.Convert(newExpression, typeof(object)),
				typeWrapperExpression,
				instanceExpression,
				bindingFlagsExpression
			).Compile();

			return lambda;
		});

		return (IObjectWrapper)ctor(
			null!,
			instance,
			bindingFlags);
	}
}