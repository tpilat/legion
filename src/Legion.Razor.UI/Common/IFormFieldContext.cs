namespace Legion.Razor.UI;

/// <summary>
/// Interface that represents the context of the form field.
/// </summary>
public interface IFormFieldContext
{
	/// <summary>
	/// Notifies the form field that the disabled state of the component has changed.
	/// </summary>
	Action<bool> DisabledChanged { get; set; }
	/// <summary>
	/// Gets or sets a value indicating whether the label is floating or fixed on top.
	/// </summary>
	bool AllowFloatingLabel { get; set; }
}
