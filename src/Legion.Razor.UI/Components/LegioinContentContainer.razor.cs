using Microsoft.AspNetCore.Components;

namespace Legion.Razor.UI;

/// <summary>
/// LegionContentContainer component.
/// </summary>
public partial class LegionContentContainer : LegionComponentWithChildren
{
	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>The name.</value>
	[Parameter]
	public string Name { get; set; }
}
