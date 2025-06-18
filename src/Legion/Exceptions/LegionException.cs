using Legion.Exceptions.Internal;
using Legion.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Legion.Exceptions;

public class LegionException : ApplicationException, ILegionException
{
	public IErrorCode? ErrorCode { get; }

	public IScopeContext? ScopeContext { get; }

	public string? Detail { get; }

	public override string Message
	{
		get
		{
			if (ErrorCode != null)
			{
				var sb = new StringBuilder();
				var empty = true;
				if (!string.IsNullOrWhiteSpace(ErrorCode.Code))
				{
					sb.Append(ErrorCode.Code);
					empty = false;
				}

				if (!string.IsNullOrWhiteSpace(ErrorCode.Message))
				{
					if (!empty)
						sb.Append(": ");

					sb.Append(ErrorCode.Message);
					empty = false;
				}

				if (!string.IsNullOrWhiteSpace(ErrorCode.Description))
				{
					if (!empty)
						sb.AppendLine();

					sb.Append(ErrorCode.Description);
					empty = false;
				}

				if (!string.IsNullOrWhiteSpace(Detail))
				{
					if (!empty)
						sb.AppendLine();

					sb.Append(Detail);
				}

				return sb.ToString();
			}
			else
			{
				if (string.IsNullOrWhiteSpace(Detail))
				{
					return base.Message ?? ErrorCodes.LegionException.Default.Message;
				}
				else
				{
					var sb = new StringBuilder();
					var empty = true;

					var message = base.Message ?? ErrorCodes.LegionException.Default.Message;
					if (!string.IsNullOrWhiteSpace(message))
					{
						sb.Append(message);
						empty = false;
					}

					if (!string.IsNullOrWhiteSpace(Detail))
					{
						if (!empty)
							sb.AppendLine();

						sb.Append(Detail);
					}

					return sb.ToString();
				}
			}
		}
	}

	public LegionException(IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
		: base(errorCode?.Message ?? ErrorCodes.LegionException.Default.Message)
	{
		ErrorCode = errorCode ?? ErrorCodes.LegionException.Default;
		ScopeContext = scopeContext;
		Detail = detail;
	}

	public LegionException(IErrorCode? errorCode, string? detail, Exception? innerException, IScopeContext? scopeContext = null)
		: base(errorCode?.Message, innerException)
	{
		ErrorCode = errorCode ?? ErrorCodes.LegionException.Default;
		ScopeContext = scopeContext;
		Detail = detail;
	}

	public override string ToString()
	{
		var sb = new StringBuilder(base.ToString());

		sb.AppendLine();
		sb.AppendLine();

		if (ScopeContext == null)
		{
			sb.Append(nameof(EnvironmentInfo.RUNTIME_UNIQUE_KEY)).Append(": ").Append(EnvironmentInfo.RUNTIME_UNIQUE_KEY);
		}
		else
		{
			sb.AppendLine("--- ScopeContext ---");
			sb.Append(ScopeContext.ToStringTrace());
		}

		return sb.ToString();
	}

	[System.Diagnostics.StackTraceHidden]
	public static void ThrowIf(bool condition, IErrorCode? errorCode = null, string? detail = null, IScopeContext? scopeContext = null)
	{
		if (condition)
			Throw(errorCode, detail, scopeContext);
	}

	[System.Diagnostics.StackTraceHidden]
	[DoesNotReturn]
	public static void Throw(IErrorCode? errorCode, string? detail, IScopeContext? scopeContext, Exception? innerException = null)
		=> throw new LegionException(errorCode, detail, innerException, scopeContext);
}
