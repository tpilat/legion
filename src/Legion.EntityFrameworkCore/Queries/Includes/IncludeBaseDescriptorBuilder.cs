using Legion.Queries;
using Legion.Queries.Includes;
using System.Linq.Expressions;

namespace Legion.EntityFrameworkCore.Queries.Includes;

public class IncludeBaseDescriptorBuilder<TEntity> : IIncludeBaseDescriptorBuilder<TEntity>, IQueryModifier<TEntity>
	where TEntity : class
{
	private readonly List<IIncludeDescriptorBuilder<TEntity>> _includeDescriptorBuilders;

	public IncludeBaseDescriptorBuilder()
	{
		_includeDescriptorBuilders = [];
	}

	public IIncludeDescriptorBuilder<TEntity, TProperty> IncludeEnumerable<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> memberSelector)
	{
		Throw.IfArgumentNull(memberSelector);

		var includeDescriptor = new IncludeDescriptor<TEntity, TProperty>(memberSelector);
		var includeDescriptorBuilder = new IncludeDescriptorBuilder<TEntity, TProperty>(this, includeDescriptor);
		_includeDescriptorBuilders.Add(includeDescriptorBuilder);
		return includeDescriptorBuilder;
	}

	public IIncludeDescriptorBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> memberSelector)
	{
		Throw.IfArgumentNull(memberSelector);

		var includeDescriptor = new IncludeDescriptor<TEntity, TProperty>(memberSelector);
		var includeDescriptorBuilder = new IncludeDescriptorBuilder<TEntity, TProperty>(this, includeDescriptor);
		_includeDescriptorBuilders.Add(includeDescriptorBuilder);
		return includeDescriptorBuilder;
	}

	public IEnumerable<TEntity> ApplyIncludes(IEnumerable<TEntity> enumerable)
	{
		Throw.IfArgumentNull(enumerable);
		return ((IQueryModifier<TEntity>)this).ApplyIncludes(enumerable.AsQueryable());
	}

	public IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> queryable)
	{
		Throw.IfArgumentNull(queryable);

		foreach (var includeDescriptorBuilder in _includeDescriptorBuilders)
			queryable = includeDescriptorBuilder.ApplyIncludes(queryable);

		return queryable;
	}

	public IEnumerable<TEntity> ApplySort(IEnumerable<TEntity> enumerable)
		=> enumerable;

	public IQueryable<TEntity> ApplySort(IQueryable<TEntity> queryable)
		=> queryable;

	public IEnumerable<TEntity> ApplyPaging(IEnumerable<TEntity> enumerable)
		=> enumerable;

	public IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> queryable)
		=> queryable;

	IEnumerable<TEntity> IQueryModifier<TEntity>.Apply(IEnumerable<TEntity> enumerable)
	{
		Throw.IfArgumentNull(enumerable);
		return ((IQueryModifier<TEntity>)this).Apply(enumerable.AsQueryable());
	}

	IQueryable<TEntity> IQueryModifier<TEntity>.Apply(IQueryable<TEntity> queryable)
	{
		Throw.IfArgumentNull(queryable);

		foreach (var includeDescriptorBuilder in _includeDescriptorBuilders)
			queryable = includeDescriptorBuilder.Apply(queryable);

		return queryable;
	}
}
