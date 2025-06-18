using Legion.EntityFrameworkCore.Queries.Includes;
using Legion.Queries;
using Legion.Queries.Includes;

namespace Legion.EntityFrameworkCore.Queries;

public class QueryableBuilder<T> : Legion.Queries.QueryableBuilder<T>, IQueryableBuilder<T>, IQueryModifier<T>
	where T : class
{
	public QueryableBuilder()
		: base()
	{
	}

	public override Legion.Queries.QueryableBuilder<T> Includes(Action<IIncludeBaseDescriptorBuilder<T>> include)
	{
		Throw.IfArgumentNull(include);

		var builder = new IncludeBaseDescriptorBuilder<T>();
		include.Invoke(builder);
		Modify(builder);

		return this;
	}
}
