using System.Text.RegularExpressions;

namespace Legion.Expressions;

public static class ExpressionPrinter
{
	private static readonly Lazy<Type> _queryableType = new(() => typeof(IQueryable<>));

	public static string Print<T>(IQueryable<T> query)
	{
		if (query == null || query.Expression == null)
			return "null";

		string expressionString = query.Expression.ToString();

		var queryType = query.GetType().GetInterfaces()
			.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == _queryableType.Value)
			?.GetGenericArguments()[0];

		if (queryType != null)
		{
			string replacementString = $"IQueryable<{queryType.FullName}>.";

			string pattern = @"^\[Microsoft\.EntityFrameworkCore\.Query\..*?\].";

			expressionString = Regex.Replace(expressionString, pattern, replacementString);
		}

		return expressionString;
	}
}

