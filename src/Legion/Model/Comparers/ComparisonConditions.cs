using Legion.Extensions;
using System.Linq.Expressions;

namespace Legion.Model.Comparers;

public class ComparisonConditions<T>
{
	private Func<T, bool> _default = x => true;

	private readonly Dictionary<string, Func<T, bool>> _properties = new Dictionary<string, Func<T, bool>>();
	private readonly Dictionary<string, object> _nestedConditionActions = new Dictionary<string, object>();

	/// <summary>
	/// Set default predicate to all properties.
	/// </summary>
	public ComparisonConditions<T> SetDefault(Func<T, bool> predicate)
	{
		_default = predicate ?? throw new ArgumentNullException(nameof(predicate));

		return this;
	}

	/// <summary>
	/// Set default predicate to all properties.
	/// </summary>
	public ComparisonConditions<T> EnableAll()
	{
		_properties.Clear();
		_default = x => true;
		return this;
	}

	/// <summary>
	/// Set default predicate to all properties.
	/// </summary>
	public ComparisonConditions<T> DisableAll()
	{
		_properties.Clear();
		_default = x => false;
		return this;
	}

	/// <summary>
	/// Enable property comparison.
	/// </summary>
	public ComparisonConditions<T> Enable(params Expression<Func<T, object?>>[] properties)
	{
		if (properties == null)
			throw new ArgumentNullException(nameof(properties));

		foreach (var property in properties)
		{
			var propertyName = property?.GetMemberName() ?? throw new ArgumentException(null, nameof(property));
			_properties[propertyName] = x => true;
		}

		return this;
	}

	/// <summary>
	/// Disable property comparison.
	/// </summary>
	public ComparisonConditions<T> Ignore(params Expression<Func<T, object?>>[] properties)
	{
		if (properties == null)
			throw new ArgumentNullException(nameof(properties));

		foreach (var property in properties)
		{
			var propertyName = property?.GetMemberName() ?? throw new ArgumentException(null, nameof(property));
			_properties[propertyName] = x => false;
		}

		return this;
	}

	/// <summary>
	/// Property can be mapped only if the condition will be evaluated as true.
	/// </summary>
	public ComparisonConditions<T> MapIf(Expression<Func<T, object?>> property, Func<T, bool> condition)
	{
		var propertyName = property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property));
		_properties[propertyName] = condition ?? throw new ArgumentNullException(nameof(condition));
		return this;
	}

	public ComparisonConditions<T> For<TNested>(Expression<Func<T, TNested>> property, Action<ComparisonConditions<TNested>> conditions)
		where TNested : class
	{
		var propertyName = property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property));
		_nestedConditionActions[propertyName] = conditions;
		return this;
	}

	public ComparisonConditions<T> For<TNested>(Expression<Func<T, IEnumerable<TNested>>> property, Action<ComparisonConditions<TNested>> conditions)
		where TNested : class
	{
		var propertyName = property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property));
		_nestedConditionActions[propertyName] = conditions;
		return this;
	}

	public ComparisonConditions<T> For<TNested>(Expression<Func<T, ICollection<TNested>>> property, Action<ComparisonConditions<TNested>> conditions)
		where TNested : class
	{
		var propertyName = property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property));
		_nestedConditionActions[propertyName] = conditions;
		return this;
	}

	public ComparisonConditions<T> For<TNested>(Expression<Func<T, IList<TNested>>> property, Action<ComparisonConditions<TNested>> conditions)
		where TNested : class
	{
		var propertyName = property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property));
		_nestedConditionActions[propertyName] = conditions;
		return this;
	}

	public ComparisonConditions<T> For<TNested>(Expression<Func<T, TNested[]>> property, Action<ComparisonConditions<TNested>> conditions)
		where TNested : class
	{
		var propertyName = property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property));
		_nestedConditionActions[propertyName] = conditions;
		return this;
	}

	public ComparisonConditions<T> Clear()
	{
		_properties.Clear();
		return this;
	}

	public bool CanCompare(T obj, string property)
		=> _properties.TryGetValue(property, out Func<T, bool>? condition)
			? condition.Invoke(obj)
			: _default.Invoke(obj);

	public Action<ComparisonConditions<TNested>>? GetConditions<TNested>(Expression<Func<T, TNested>> property)
		=> _nestedConditionActions.TryGetValue(property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property)), out object? nested)
			? (Action<ComparisonConditions<TNested>>)nested
			: null;

	public Action<ComparisonConditions<TNested>>? GetConditions<TNested>(Expression<Func<T, List<TNested>>> property)
		=> _nestedConditionActions.TryGetValue(property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property)), out object? nested)
			? (Action<ComparisonConditions<TNested>>)nested
			: null;

	public Action<ComparisonConditions<TNested>>? GetConditions<TNested>(Expression<Func<T, IReadOnlyList<TNested>>> property)
		=> _nestedConditionActions.TryGetValue(property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property)), out object? nested)
			? (Action<ComparisonConditions<TNested>>)nested
			: null;

	public Action<ComparisonConditions<TNested>>? GetConditions<TNested>(Expression<Func<T, IList<TNested>>> property)
		=> _nestedConditionActions.TryGetValue(property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property)), out object? nested)
			? (Action<ComparisonConditions<TNested>>)nested
			: null;

	public Action<ComparisonConditions<TNested>>? GetConditions<TNested>(Expression<Func<T, ICollection<TNested>>> property)
		=> _nestedConditionActions.TryGetValue(property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property)), out object? nested)
			? (Action<ComparisonConditions<TNested>>)nested
			: null;

	public Action<ComparisonConditions<TNested>>? GetConditions<TNested>(Expression<Func<T, IEnumerable<TNested>>> property)
		=> _nestedConditionActions.TryGetValue(property?.GetMemberName() ?? throw new ArgumentNullException(nameof(property)), out object? nested)
			? (Action<ComparisonConditions<TNested>>)nested
			: null;
}
