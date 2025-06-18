namespace Legion.Queries.Paging;

public interface IPagingDescriptorBuilder<T> : IQueryModifier<T>
{
	IPagingDescriptorBuilder<T> Page(int pageIndex, int pageSize);
}
