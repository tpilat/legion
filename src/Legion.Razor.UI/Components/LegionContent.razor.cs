using Microsoft.AspNetCore.Components;

namespace Legion.Razor.UI;

/// <summary>
/// LegionContent component.
/// </summary>
public partial class LegionContent : LegionComponentWithChildren
{
	/// <summary>
	/// Gets or sets the container.
	/// </summary>
	/// <value>The container.</value>
	[Parameter]
	public string Container { get; set; }

	/// <inheritdoc />
	protected override string GetComponentCssClass()
	{
		return "content";
	}
}
