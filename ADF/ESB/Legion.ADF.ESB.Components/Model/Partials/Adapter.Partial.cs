using Legion.Extensions;
using Legion.Validation;
using Legion.Validation.Results;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class Adapter : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static ValidatorBuilder<Adapter> DBValidatorRulesWithInOutBounds(ValidatorBuilder<Adapter> builder)
		=> SetDBValidatorRules(new ValidatorBuilder<Adapter>())
			.WithPropertyError(x => x.IsInbound, a =>
			{
				if (a == null)
					return null!;

				if (!a.IsInbound && !a.IsOutbound)
					return new ValidationResult().AddError<Adapter>(Legion.ADF.ESB.Components.Exceptions.Internal.ErrorCodes.AdapterException.NoInboundNoOutbound);

				return null!;
			});

	internal static IResult<Adapter> CreateAdapter(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Adapter>();

		if (result.IsArgumentNull(scopeContext, esbAdapter))
			return result.Build();

		var adapter = new Adapter
		{
			IdAdapter = esbAdapter.IdAdapter,
			Code = esbAdapter.Code,
			Name = esbAdapter.Name,
			Class = esbAdapter.GetType().ToFriendlyFullName(),
			Description = esbAdapter.Description,
			Properties = esbAdapter.Properties,
			IdAdapterStatus = esbAdapter.IdAdapterStatus,
			IsInbound = esbAdapter.IsInbound,
			IsOutbound = esbAdapter.IsOutbound
		};

		var validationResult =
			DBValidatorRulesWithInOutBounds(new ValidatorBuilder<Adapter>())
				.Build()
				.Validate(adapter);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(adapter).Build();
	}

	public static bool AreEquals(Adapter adapter1, Adapter adapter2)
		=> adapter1 != null
			&& adapter2 != null
			&& adapter1.Class == adapter2.Class
			&& adapter1.IdAdapter == adapter2.IdAdapter;

	public IResult Update(
		IScopeContext scopeContext,
		IESBAdapter esbAdapter)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNull(scopeContext.CreateNew(), esbAdapter))
			return result.Build();

		var mergeResult = esbAdapter.MergeProperties(scopeContext.CreateNew(), Properties);

		if (result.MergeHasError(mergeResult))
			return result.Build();

		Code = esbAdapter.Code;
		Name = esbAdapter.Name;
		Description = esbAdapter.Description;
		//Properties = esbAdapter.Properties; //we will not overwrite config in DB from code
		IsInbound = esbAdapter.IsInbound;
		IsOutbound = esbAdapter.IsOutbound;

		var validationResult =
			DBValidatorRulesWithInOutBounds(new ValidatorBuilder<Adapter>())
				.Build()
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}
}
