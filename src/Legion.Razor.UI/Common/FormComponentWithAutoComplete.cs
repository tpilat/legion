using Microsoft.AspNetCore.Components;

namespace Legion.Razor.UI;

/// <summary>
/// Class FormComponentWithAutoComplete.
/// </summary>
public class FormComponentWithAutoComplete<T> : FormComponent<T>
{
	/// <summary>
	/// Gets or sets a value indicating the type of built-in autocomplete
	/// the browser should use.
	/// <see cref="Blazor.AutoCompleteType" />
	/// </summary>
	/// <value>
	/// The type of built-in autocomplete.
	/// </value>
	[Parameter]
	public virtual AutoCompleteType AutoCompleteType { get; set; } = AutoCompleteType.On;

	/// <summary>
	/// Gets the autocomplete attribute's string value.
	/// </summary>
	/// <value>
	/// <c>off</c> if the AutoComplete parameter is false or the
	/// AutoCompleteType parameter is "off". When the AutoComplete
	/// parameter is true, the value is <c>on</c> or, if set, the value of
	/// AutoCompleteType.</value>
	public virtual string AutoCompleteAttribute
	{
		get => Attributes != null && Attributes.ContainsKey("AutoComplete") && $"{Attributes["AutoComplete"]}".ToLower() == "false" ? DefaultAutoCompleteAttribute :
			Attributes != null && Attributes.ContainsKey("AutoComplete") ? Attributes["AutoComplete"] as string ?? AutoCompleteType.GetAutoCompleteValue() : AutoCompleteType.GetAutoCompleteValue();
	}

	/// <summary>
	/// Gets or sets the default autocomplete attribute's string value.
	/// </summary>
	public virtual string DefaultAutoCompleteAttribute { get; set; } = "off";

	object ariaAutoComplete;

	/// <inheritdoc />
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		parameters = parameters.TryGetValue("aria-autocomplete", out ariaAutoComplete) ?
			ParameterView.FromDictionary(parameters
				.ToDictionary().Where(i => i.Key != "aria-autocomplete").ToDictionary(i => i.Key, i => i.Value)
				.ToDictionary(i => i.Key, i => i.Value))
			: parameters;

		await base.SetParametersAsync(parameters);
	}

	/// <summary>
	/// Gets or sets the default aria-autocomplete attribute's string value.
	/// </summary>
	public virtual string DefaultAriaAutoCompleteAttribute { get; set; } = "none";

	/// <summary>
	/// Gets the aria-autocomplete attribute's string value.
	/// </summary>
	public virtual string AriaAutoCompleteAttribute
	{
		get => AutoCompleteAttribute == DefaultAutoCompleteAttribute ? DefaultAriaAutoCompleteAttribute : ariaAutoComplete as string;
	}

}

