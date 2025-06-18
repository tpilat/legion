namespace Legion.ADF.Config.Model;

public sealed partial class ConfigurationClass : Config.ConfigBaseEntity, Legion.Model.IEntity
{
	internal static IResult<ConfigurationClass?> Create(
		IScopeContext scopeContext,
		string rootPath,
		string displayName,
		string? csharpClassTypeToDeserialize)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ConfigurationClass?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, rootPath))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, displayName))
			return result.Build();

		var id = Guid.NewGuid();
		var configurationClass = new ConfigurationClass
		{
			__IsNewObject = true,
			IdConfigurationClass = id,
			DisplayName = displayName,
			RootPath = rootPath,
			Class = csharpClassTypeToDeserialize
		};

		var validationResult =
			DefaultDBValidator
				.Validate(configurationClass);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(configurationClass).Build();
	}

	internal IResult UpdateValue(
		IScopeContext scopeContext,
		string displayName,
		string? csharpClassTypeToDeserialize)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		DisplayName = displayName;
		Class = csharpClassTypeToDeserialize;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}
}
