using Legion.Queries;
using Legion.Queries.Includes;
using System.Linq.Expressions;

namespace Legion.EntityFrameworkCore.Queries.Includes;

public class ThenIncludeDescriptorBuilder<TEntity, TProperty, TNextProperty> : IThenIncludeDescriptorBuilder<TEntity, TProperty, TNextProperty>, IThenIncludeDescriptorBuilder<TEntity, TProperty>, IQueryModifier<TEntity>
	where TEntity : class
{
	private readonly IncludeBaseDescriptorBuilder<TEntity> _baseDescriptorBuilder;
	private readonly ThenIncludeDescriptor<TEntity, TProperty, TNextProperty> _thenIncludeDescriptor;
	private readonly List<IThenIncludeDescriptorBuilder<TEntity, TNextProperty>> _thenIncludeDescriptorBuilders;

	public ThenIncludeDescriptorBuilder(IncludeBaseDescriptorBuilder<TEntity> baseDescriptorBuilder, ThenIncludeDescriptor<TEntity, TProperty, TNextProperty> thenIncludeDescriptor)
	{
		Throw.IfArgumentNull(baseDescriptorBuilder);
		Throw.IfArgumentNull(thenIncludeDescriptor);
		_baseDescriptorBuilder = baseDescriptorBuilder;
		_thenIncludeDescriptor = thenIncludeDescriptor;
		_thenIncludeDescriptorBuilders = [];
	}

	public IIncludeDescriptorBuilder<TEntity, T> IncludeEnumerable<T>(Expression<Func<TEntity, IEnumerable<T>>> memberSelector)
		=> _baseDescriptorBuilder.IncludeEnumerable(memberSelector);

	public IIncludeDescriptorBuilder<TEntity, T> Include<T>(Expression<Func<TEntity, T>> memberSelector)
		=> _baseDescriptorBuilder.Include(memberSelector);

	public IThenIncludeDescriptorBuilder<TEntity, TNextProperty, TNextNestedProperty> ThenIncludeEnumerable<TNextNestedProperty>(Expression<Func<TNextProperty, IEnumerable<TNextNestedProperty>>> memberSelector)
	{
		Throw.IfArgumentNull(memberSelector);

		var thenIncludeDescriptor = _thenIncludeDescriptor.SetThenNavigation(memberSelector);
		var thenIncludeDescriptorBuilder = new ThenIncludeDescriptorBuilder<TEntity, TNextProperty, TNextNestedProperty>(_baseDescriptorBuilder, thenIncludeDescriptor);
		_thenIncludeDescriptorBuilders.Add(thenIncludeDescriptorBuilder);
		return thenIncludeDescriptorBuilder;
	}

	public IThenIncludeDescriptorBuilder<TEntity, TNextProperty, TNextNestedProperty> ThenInclude<TNextNestedProperty>(Expression<Func<TNextProperty, TNextNestedProperty>> memberSelector)
	{
		Throw.IfArgumentNull(memberSelector);

		var thenIncludeDescriptor = _thenIncludeDescriptor.SetThenNavigation(memberSelector);
		var thenIncludeDescriptorBuilder = new ThenIncludeDescriptorBuilder<TEntity, TNextProperty, TNextNestedProperty>(_baseDescriptorBuilder, thenIncludeDescriptor);
		_thenIncludeDescriptorBuilders.Add(thenIncludeDescriptorBuilder);
		return thenIncludeDescriptorBuilder;
	}

	IEnumerable<TEntity> IQueryModifier<TEntity>.ApplyIncludes(IEnumerable<TEntity> enumerable)
		=> enumerable;

	IQueryable<TEntity> IQueryModifier<TEntity>.ApplyIncludes(IQueryable<TEntity> queryable)
		=> queryable;

	IEnumerable<TEntity> IQueryModifier<TEntity>.ApplySort(IEnumerable<TEntity> enumerable)
		=> enumerable;

	IQueryable<TEntity> IQueryModifier<TEntity>.ApplySort(IQueryable<TEntity> queryable)
		=> queryable;

	IEnumerable<TEntity> IQueryModifier<TEntity>.ApplyPaging(IEnumerable<TEntity> enumerable)
		=> enumerable;

	IQueryable<TEntity> IQueryModifier<TEntity>.ApplyPaging(IQueryable<TEntity> queryable)
		=> queryable;

	IEnumerable<TEntity> IQueryModifier<TEntity>.Apply(IEnumerable<TEntity> enumerable)
		=> enumerable;

	IQueryable<TEntity> IQueryModifier<TEntity>.Apply(IQueryable<TEntity> queryable)
		=> queryable;
}
