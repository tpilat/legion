using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Legion.Localization;

public class ResourceManagerWithCultureStringLocalizer : ResourceManagerStringLocalizer, IStringLocalizer
{
	private readonly CultureInfo? _culture;
	private readonly string _resourceBaseName;

	public ResourceManagerWithCultureStringLocalizer(
		ResourceManager resourceManager,
		CultureInfo culture,
		Assembly resourceAssembly,
		string baseName,
		IResourceNamesCache resourceNamesCache,
		ILogger logger)
		: base(
			resourceManager,
			resourceAssembly,
			baseName,
			resourceNamesCache,
			logger)
	{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
		Throw.IfArgumentNull(baseName);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

		_culture = culture;
		_resourceBaseName = baseName;
	}

	/// <inheritdoc />
	public override LocalizedString this[string name]
	{
		get
		{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
			Throw.IfArgumentNull(name);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

			var value = GetStringSafely(name, _culture);

			return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: _resourceBaseName);
		}
	}

	/// <inheritdoc />
	public override LocalizedString this[string name, params object[] arguments]
	{
		get
		{
#pragma warning disable L_ArgNullEx_Str // Type or member is obsolete
			Throw.IfArgumentNull(name);
#pragma warning restore L_ArgNullEx_Str // Type or member is obsolete

			var format = GetStringSafely(name, _culture);
			var value = string.Format(_culture ?? CultureInfo.CurrentCulture, format ?? name, arguments);

			return new LocalizedString(name, value, resourceNotFound: format == null, searchedLocation: _resourceBaseName);
		}
	}

	/// <inheritdoc />
	public override IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
		GetAllStrings(includeParentCultures, _culture ?? CultureInfo.CurrentUICulture);
}
