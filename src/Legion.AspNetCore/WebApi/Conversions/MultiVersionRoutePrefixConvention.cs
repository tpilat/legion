using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Legion.AspNetCore.WebApi.Conversions;

public class MultiVersionRoutePrefixConvention : IApplicationModelConvention
{
	private readonly string _prefix;

	public MultiVersionRoutePrefixConvention(string prefix)
	{
		Throw.IfArgumentNullOrWhiteSpace(prefix);

		_prefix = prefix;
	}

	public void Apply(ApplicationModel application)
	{
		foreach (var controller in application.Controllers)
		{
			var versionAttribute = controller.Attributes.OfType<ApiRoutePrefixAttribute>().FirstOrDefault();
			if (versionAttribute == null)
				continue;

			var prefix = $"{_prefix}/{versionAttribute.Version}";

			foreach (var selector in controller.Selectors)
			{
				var original = selector.AttributeRouteModel;
				if (original == null)
					continue;

				selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
					new AttributeRouteModel(new RouteAttribute(prefix)),
					original);
			}
		}
	}
}
