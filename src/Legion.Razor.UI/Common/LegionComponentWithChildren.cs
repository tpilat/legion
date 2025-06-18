using Microsoft.AspNetCore.Components;

namespace Legion.Razor.UI;

/// <summary>
/// A base class of components that have child content.
/// </summary>
public class LegionComponentWithChildren : LegionComponentBase
{
	/// <summary>
	/// Gets or sets the child content
	/// </summary>
	/// <value>The content of the child.</value>
	[Parameter]
	public RenderFragment ChildContent { get; set; }
}