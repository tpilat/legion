using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Legion.Validation.Options;

public class OptionsValidator<TOptions> : IValidateOptions<TOptions>
	where TOptions : class
{
	private readonly IServiceProvider _serviceProvider;
	private readonly string? _name;
	private readonly string? _basePath;

	public OptionsValidator(IServiceProvider serviceProvider, string? basePath, string? name)
	{
		Throw.IfArgumentNull(serviceProvider);

		_serviceProvider = serviceProvider;
		_name = name;

		if (!string.IsNullOrWhiteSpace(basePath))
			_basePath = basePath;
	}

	public ValidateOptionsResult Validate(string? name, TOptions options)
	{
		if (_name != null && _name != name)
			return ValidateOptionsResult.Skip;

		Throw.IfArgumentNull(options);

		using var serviceScope = _serviceProvider.CreateScope();
		var validator = serviceScope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

		var result = validator.Validate(options);

		if (result.Failures.Count == 0)
		{
			return ValidateOptionsResult.Success;
		}
		else
		{
			string? basePath = _basePath;
			var errors = new List<string>();
			foreach (var failure in result.Failures)
			{
				basePath ??= typeof(TOptions).Name;
				var error = failure.ToFullPathString(basePath);
				if (!string.IsNullOrWhiteSpace(error))
					errors.Add(error);
			}

			return ValidateOptionsResult.Fail(errors);
		}
	}
}
