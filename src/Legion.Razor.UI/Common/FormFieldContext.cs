namespace Legion.Razor.UI;

/// <summary>
/// Represents the context of the form field.
/// </summary>
public class FormFieldContext : IFormFieldContext
{
	/// <summary>
	/// Notifies the form field that the disabled state of the component has changed.
	/// </summary>
	public Action<bool> DisabledChanged { get; set; }
	/// <summary>
	/// Gets or sets a value indicating whether the label is floating or fixed on top.
	/// </summary>
	public bool AllowFloatingLabel { get; set; }
}