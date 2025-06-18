using System.Collections;

namespace Legion.Text;

public static class ToStringHelper
{
	public static string? ToString<TState>(TState state, string delimiter = "|")
	{
		if (state == null)
			return null;

		//var type = typeof(TState);

		//if (type.IsSimpleType())
		//	return state.ToString();

		if (state is IEnumerable enumerable)
		{
			var items = new List<string?>();
			
			var enumerator = enumerable.GetEnumerator();
			if (enumerator != null)
			{
				while (enumerator.MoveNext())
				{
					var item = enumerator.Current;
					items.Add(ToString(item, delimiter));
				}
			}

			return string.Join(delimiter, items);
		}

		return state.ToString();
	}
}
