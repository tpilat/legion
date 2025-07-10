using Legion.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Legion.AspNetCore.WebApi.Configurations;

public class ExampleSchemaFilter : ISchemaFilter
{
	private static readonly HashSet<Type> _unsupportedTypes = [];

	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (_unsupportedTypes.Contains(context.Type))
			return;

		if (context.Type.GetGenericTypeDefinitionIfExists() == typeof(Results.ResultDto<>))
		{
			if (schema.Properties.ContainsKey("successMessages"))
			{
				schema.Properties["successMessages"].Example = new Microsoft.OpenApi.Any.OpenApiNull(); //new OpenApiArray();
			}

			if (schema.Properties.ContainsKey("warningMessages"))
			{
				schema.Properties["warningMessages"].Example = new Microsoft.OpenApi.Any.OpenApiNull(); //new OpenApiArray();
			}

			if (schema.Properties.ContainsKey("errorMessages"))
			{
				schema.Properties["errorMessages"].Example = new Microsoft.OpenApi.Any.OpenApiNull(); //new OpenApiArray();
			}
		}
		else
		{
			_unsupportedTypes.Add(context.Type);
		}
	}
}
