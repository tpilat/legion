namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryToken : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	internal static IResult<ApplicationEntryToken> Create(
		IScopeContext scopeContext,
		string token,
		string sourceFilePath,
		string? methodInfo,
		string? aggregateName,
		string? description)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ApplicationEntryToken>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, token))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, sourceFilePath))
			return result.Build();

		var applicationEntryToken = new ApplicationEntryToken
		{
			__IsNewObject = true,
			IdApplicationEntryToken = Guid.NewGuid(),
			Token = token,
			SourceFilePath = sourceFilePath,
			MethodInfo = methodInfo,
			AggregateName = aggregateName,
			Description = description
		};

		var validationResult =
			DefaultDBValidator
				.Validate(applicationEntryToken);

		if (result.MergeHasError(scopeContext.CreateNew(), validationResult, true))
			return result.Build();

		return result.WithData(applicationEntryToken).Build();
	}

	internal IResult Update(
		IScopeContext scopeContext,
		string? methodInfo,
		string? aggregateName,
		string? description)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		MethodInfo = methodInfo;
		AggregateName = aggregateName;
		Description = description;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}
}
