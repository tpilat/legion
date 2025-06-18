using Legion.Validation;

namespace Legion.Queries;

public record BaseQuery<T>(
	bool AsNoTracking,
	bool DisableCahce,
	Action<Legion.Queries.IQueryableBuilder<T>>? QueryableBuilder)
	where T : class
{
	public static IValidator<BaseQuery<T>> DefaultBaseValidator { get; }

	static BaseQuery()
	{
		DefaultBaseValidator = SetBaseValidatorRules(new ValidatorBuilder<BaseQuery<T>>()).Build();
	}

	public static ValidatorBuilder<BaseQuery<T>> SetBaseValidatorRules(ValidatorBuilder<BaseQuery<T>> builder)
		=> builder
			//.WithError(
			//		(x, parent) => x.AsNoTracking == false && x.DisableCahce == false,
			//		x => Exceptions.Internal.ErrorCodes.ChangeTrackingVsCachingException.Default)
		;

	public virtual IValidationResult Validate()
		=> DefaultBaseValidator.Validate(this);
}
