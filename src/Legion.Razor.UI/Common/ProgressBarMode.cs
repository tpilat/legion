namespace Legion.Razor.UI;

/// <summary>
/// Specifies the behavior of <see cref="LegionProgressBar" /> or <see cref="LegionProgressBarCircular" />.
/// </summary>
public enum ProgressBarMode
{
	/// <summary>
	/// LegionProgressBar displays its value as a percentage range (0 to 100).
	/// </summary>
	Determinate,
	/// <summary>
	/// LegionProgressBar displays continuous animation.
	/// </summary>
	Indeterminate
}
