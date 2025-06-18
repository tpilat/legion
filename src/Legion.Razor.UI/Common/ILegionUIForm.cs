namespace Legion.Razor.UI;

/// <summary>
/// Represents the common <see cref="LegionUITemplateForm{TItem}" /> API used by
/// its items. Injected as a cascading property in <see cref="ILegionFormComponent" />.
/// </summary>
public interface ILegionUIForm
{
	/// <summary>
	/// Adds the specified component to the form.
	/// </summary>
	/// <param name="component">The component to add to the form.</param>
	void AddComponent(ILegionFormComponent component);
	/// <summary>
	/// Removes the component from the form.
	/// </summary>
	/// <param name="component">The component to remove from the form.</param>
	void RemoveComponent(ILegionFormComponent component);
	/// <summary>
	/// Finds a form component by its name.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>The component whose <see cref="ILegionFormComponent.Name" /> equals to <paramref name="name" />; <c>null</c> if such a component is not found.</returns>
	ILegionFormComponent FindComponent(string name);
}
