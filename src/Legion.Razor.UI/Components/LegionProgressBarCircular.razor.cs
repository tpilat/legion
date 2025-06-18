using Microsoft.AspNetCore.Components;

namespace Legion.Razor.UI;

/// <summary>
/// LegionProgressBarCircular component.
/// </summary>
/// <example>
/// <code>
/// &lt;LegionProgressBarCircular @bind-Value="@value" Max="200" /&gt;
/// </code>
/// </example>
public partial class LegionProgressBarCircular : LegionProgressBar
{
	/// <inheritdoc />
	protected override string GetComponentCssClass()
	{
		var classList = new List<string>()
			{
				"lui-progressbar-circular"
			};

		switch (Mode)
		{
			case ProgressBarMode.Determinate:
				classList.Add("lui-progressbar-determinate");
				break;
			case ProgressBarMode.Indeterminate:
				classList.Add("lui-progressbar-indeterminate");
				break;
		}

		classList.Add($"lui-progressbar-{ProgressBarStyle.ToString().ToLowerInvariant()}");
		classList.Add($"lui-progressbar-circular-{GetCircleSize()}");

		return string.Join(" ", classList);
	}

	/// <summary>
	/// Gets the circle size.
	/// </summary>
	protected string GetCircleSize()
	{
		switch (Size)
		{
			case ProgressBarCircularSize.Medium:
				return "md";
			case ProgressBarCircularSize.Large:
				return "lg";
			case ProgressBarCircularSize.Small:
				return "sm";
			case ProgressBarCircularSize.ExtraSmall:
				return "xs";
			default:
				return string.Empty;
		}
	}

	/// <summary>
	/// Gets or sets the size.
	/// </summary>
	/// <value>The size.</value>
	[Parameter]
	public ProgressBarCircularSize Size { get; set; } = ProgressBarCircularSize.Medium;
}
