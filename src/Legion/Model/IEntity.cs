namespace Legion.Model;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IBaseEntity
{
}

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IEntity : IBaseEntity
{
	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	bool __IsNewObject { get; set; }

	string? GetPrimaryKeyValue();

	IReadOnlyList<IDomainEvent> GetDomainEvents();

	void ClearDomainEvents();
}

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IQueryEntity : IBaseEntity
{
}
