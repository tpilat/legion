using Legion.EntityFrameworkCore.Extensions;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Legion.EntityFrameworkCore.Expressions.MemberAccess.Tokenizer;

internal static class MemberAccessTokenExtensions
{
	public static Expression CreateMemberAccessExpression(this IMemberAccessToken token, Expression instance)
	{
		var memberInfo = token.GetMemberInfoForType(instance.Type) ?? throw new ArgumentException(FormatInvalidTokenErrorMessage(token, instance.Type));

		if (token is IndexerToken indexerToken)
		{
			var arguments = indexerToken.GetIndexerArguments();
			return Expression.Call(instance, (MethodInfo)memberInfo, arguments);
		}

		// Property or field
		return Expression.MakeMemberAccess(instance, memberInfo);
	}

	private static string FormatInvalidTokenErrorMessage(IMemberAccessToken token, Type type)
	{
		string memberName;
		string memberType;

		if (token is PropertyToken propertyToken)
		{
			memberType = "property or field";
			memberName = propertyToken.PropertyName;
		}
		else
		{
			memberType = "indexer with arguments";

			var argumentsAsString = ((IndexerToken)token).Arguments.Where(a => a != null).Select(a => a.ToString());
			memberName = string.Join(",", argumentsAsString.ToArray());
		}

		return string.Format(CultureInfo.CurrentCulture, "Invalid {0} - '{1}' for type: {2}", memberType, memberName, type.GetTypeName());
	}

	private static IEnumerable<Expression> GetIndexerArguments(this IndexerToken indexerToken)
	{
		return indexerToken.Arguments.Select(a => (Expression)Expression.Constant(a));
	}

	/// <exception cref="InvalidOperationException"><c>InvalidOperationException</c>.</exception>
	private static MemberInfo? GetMemberInfoForType(this IMemberAccessToken token, Type targetType)
	{
		if (token is PropertyToken propertyToken)
			return GetMemberInfoFromPropertyToken(propertyToken, targetType);

		if (token is IndexerToken indexerToken)
			return GetMemberInfoFromIndexerToken(indexerToken, targetType);

		throw new InvalidOperationException(token.GetType().GetTypeName() + " is not supported");
	}

	private static MemberInfo? GetMemberInfoFromPropertyToken(PropertyToken token, Type targetType)
	{
		return targetType.FindPropertyOrField(token.PropertyName);
	}

	private static MemberInfo? GetMemberInfoFromIndexerToken(IndexerToken token, Type targetType)
		=> targetType.GetIndexerPropertyInfo(token.Arguments.Select(a => a.GetType()).ToArray())?.GetGetMethod();
}