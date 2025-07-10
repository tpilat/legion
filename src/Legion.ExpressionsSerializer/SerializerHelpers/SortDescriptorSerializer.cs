using Legion.ExpressionsSerializer.Extensions;
using Legion.ExpressionsSerializer.Serializers;
using Legion.Queries.Sorting;
using System.Linq.Expressions;

namespace Legion.ExpressionsSerializer.SerializerHelpers;

public static class SortDescriptorSerializer
{
	private static readonly Lazy<ExpressionSerializer> _serializer = new(() => new ExpressionSerializer(new JsonSerializer()));

	public static SortDescriptorDto Serialize<T>(SortDescriptor<T> sortDescriptor)
	{
		Throw.IfArgumentNull(sortDescriptor);
		Throw.IfArgumentNull(sortDescriptor.MemberSelector);

		return new SortDescriptorDto
		{
			MemberSelector = sortDescriptor.MemberSelector.ToJson(),
			SortDirection = sortDescriptor.SortDirection
		};
	}

	public static SortDescriptor<T> Deserialize<T>(SortDescriptorDto dto)
	{
		Throw.IfArgumentNull(dto);
		Throw.IfArgumentNullOrWhiteSpace(dto.MemberSelector);

		var memberSelectorExpression = _serializer.Value.DeserializeText(dto.MemberSelector) as Expression<Func<T, object>>;
		Throw.IfNull(memberSelectorExpression);

		return new SortDescriptor<T>
		{
			MemberSelector = memberSelectorExpression,
			SortDirection = dto.SortDirection
		};
	}

	public static SortDescriptorBuilder<T> Deserialize<T>(
		IScopeContext scopeContext,
		List<SortDescriptorDto>? descriptors,
		SortDescriptorBuilder<T> defaultSortDescriptorBuilder)
	{
		scopeContext = scopeContext.CreateNew();

		if (0 < descriptors?.Count)
		{
			int i = 0;
			var sortDescriptors = new List<SortDescriptor<T>>();
			foreach (var sortDescriptorDto in descriptors)
			{
				var sortDescriptor = Deserialize<T>(sortDescriptorDto);

				Throw.IfNull(sortDescriptors, errorCode: null, $"SortDescriptor[{i}] '{sortDescriptorDto}' could not be deserialized.", scopeContext);

				sortDescriptors.Add(sortDescriptor);
				i++;
			}

			return new SortDescriptorBuilder<T>(sortDescriptors);
		}
		else
		{
			return defaultSortDescriptorBuilder;
		}
	}
}
