using Legion.ExpressionsSerializer.Nodes;
using System.Linq.Expressions;
using System.Reflection;

namespace Legion.ExpressionsSerializer.Interfaces;

public interface IExpressionContext
{
	BindingFlags? GetBindingFlags();

	ParameterExpression GetParameterExpression(ParameterExpressionNode node);

	Type ResolveType(TypeNode node);
}
