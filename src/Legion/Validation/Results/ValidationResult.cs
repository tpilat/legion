using Legion.Exceptions;
using Legion.Logging;
using System.Text;

namespace Legion.Validation.Results;

public class ValidationResult : IValidationResult
{
	private readonly List<IValidationFailure> _failures = [];

	public IReadOnlyList<IValidationFailure> Failures => _failures;

	public bool Interrupted { get; set; }
	public bool SkipNestedValidation { get; set; }

	IReadOnlyList<IValidationFailure> IValidationResult.Failures => _failures;

	public bool HasFailure => Failures.Any();

	public bool HasError => Failures.Any(x => x.Severity == ValidationSeverity.Error);

	public bool HasWarning => Failures.Any(x => x.Severity == ValidationSeverity.Warning);

	public ValidationResult()
	{
	}

	public ValidationResult(IValidationFailure failure)
	{
		Throw.IfArgumentNull(failure);

		_failures.Add(failure);
	}

	internal ValidationResult AddFailure(IValidationFailure? failure)
	{
		if (failure != null)
			_failures.Add(failure);

		return this;
	}

	internal void Merge(IValidationResult? result)
	{
		if (result == null)
			return;

		foreach (var error in result.Failures)
			AddFailure(error);
	}

	public override string? ToString()
	{
		var sb = new StringBuilder();

		foreach (var failure in _failures)
			sb.AppendLine(failure.ToString());
		
		var result = sb.ToString();

		return string.IsNullOrWhiteSpace(result)
			? null
			: result;
	}

	public IResult ToResult(IScopeContext scopeContext, bool clientMessageWithPropertyName = true)
	{
		var result = new ResultBuilder();
		result.MergeHasError(scopeContext, this, clientMessageWithPropertyName);
		return result.Build();
	}

	public string? ToText(IScopeContext scopeContext, string? delimiter, bool withDetail = true, bool withSeverity = true)
	{
		var sb = new StringBuilder();

		if (delimiter == null)
			delimiter = Environment.NewLine;

		int i = 0;
		foreach (var failure in Failures)
		{
			i++;
			sb.Append(failure.ToString(withDetail, withSeverity));

			if (i < Failures.Count)
				sb.Append(delimiter);
		}

		var result = sb.ToString();
		return string.IsNullOrWhiteSpace(result)
			? null
			: result;
	}

	public ValidationException? ToException(IScopeContext scopeContext, IErrorCode? errorCode = null, bool clientMessageWithPropertyName = true, bool withErrorMessageDetails = false)
	{
		var result = ToResult(scopeContext, clientMessageWithPropertyName);
		if (!result.HasError)
			return null;

		var validationException = ExceptionHelper.ToException(
			result.ErrorMessages.Cast<ILogMessage>().ToList(),
			msg => new ValidationException(errorCode, msg, (Exception?)null),
			withErrorMessageDetails);

		return validationException;
	}
}
