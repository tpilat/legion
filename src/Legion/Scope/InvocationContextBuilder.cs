using Legion.Transactions;
using System.Runtime.CompilerServices;

namespace Legion;

public interface IInvocationContextBuilder<TBuilder, TObject>
	where TBuilder : IInvocationContextBuilder<TBuilder, TObject>
	where TObject : InvocationContext
{
	TBuilder Object(TObject invocationContext);

	TObject Build();

	TBuilder Initialize(
		IServiceProvider serviceProvider,
		string? storeId = null,
		bool forceStoreId = false,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	TBuilder WithProfilerAsyncCallback(Func<IInvocationResult, CancellationToken, Task> asyncCallback, bool force = false);

	TBuilder WithProfilerSyncCallback(Action<IInvocationResult> syncCallback, bool force = false);

	TBuilder WithUnhandledErrorCode(IErrorCode errorCode, bool force = false);

	TBuilder WithDefaultClientErrorMessage(string defaultClientErrorMessage, bool force = false);
}

public abstract class InvocationContextBuilderBase<TBuilder, TObject> : IInvocationContextBuilder<TBuilder, TObject>
	where TBuilder : InvocationContextBuilderBase<TBuilder, TObject>
	where TObject : InvocationContext
{
	protected readonly TBuilder _builder;
	protected TObject _invocationContext;

	protected InvocationContextBuilderBase(TObject invocationContext)
	{
		Throw.IfArgumentNull(invocationContext);

		_invocationContext = invocationContext;
		_builder = (TBuilder)this;
	}

	public virtual TBuilder Object(TObject invocationContext)
	{
		Throw.IfArgumentNull(invocationContext);

		_invocationContext = invocationContext;
		return _builder;
	}

	public TObject Build()
	{
		return _invocationContext;
	}

	public TBuilder Initialize(
		IServiceProvider serviceProvider,
		string? storeId = null,
		bool forceStoreId = false,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		Throw.IfArgumentNull(serviceProvider);

		_invocationContext.ServiceProvider = serviceProvider;

		_invocationContext.InvocationAppendTraceFrameWithTargetStoreId(
			storeId,
			forceStoreId,
			true,
			memberName,
			sourceFilePath,
			sourceLineNumber);

		return _builder;
	}

	public TBuilder WithProfilerAsyncCallback(Func<IInvocationResult, CancellationToken, Task> asyncCallback, bool force = false)
	{
		if (force || _invocationContext.InvocationResultAsyncCallback == null)
			_invocationContext.InvocationResultAsyncCallback = asyncCallback;

		return _builder;
	}

	public TBuilder WithProfilerSyncCallback(Action<IInvocationResult> syncCallback, bool force = false)
	{
		if (force || _invocationContext.InvocationResultSyncCallback == null)
			_invocationContext.InvocationResultSyncCallback = syncCallback;

		return _builder;
	}

	public TBuilder WithUnhandledErrorCode(IErrorCode errorCode, bool force = false)
	{
		if (force || _invocationContext.UnhandledErrorCode == null)
			_invocationContext.UnhandledErrorCode = errorCode;

		return _builder;
	}

	public TBuilder WithDefaultClientErrorMessage(string defaultClientErrorMessage, bool force = false)
	{
		if (force || string.IsNullOrWhiteSpace(_invocationContext.DefaultClientErrorMessage))
			_invocationContext.DefaultClientErrorMessage = defaultClientErrorMessage;

		return _builder;
	}
}

public class InvocationContextBuilder : InvocationContextBuilderBase<InvocationContextBuilder, InvocationContext>
{
	public InvocationContextBuilder(IScopeContext scopeContext, IErrorCode? unhandledErrorCode = null)
		: this(new InvocationContext(scopeContext, unhandledErrorCode))
	{
	}

	public InvocationContextBuilder(InvocationContext invocationContext)
		: base(invocationContext)
	{
	}

	public static implicit operator InvocationContext?(InvocationContextBuilder builder)
	{
		if (builder == null)
			return null;

		return builder._invocationContext as InvocationContext;
	}

	public static implicit operator InvocationContextBuilder?(InvocationContext invocationContext)
	{
		if (invocationContext == null)
			return null;

		return new InvocationContextBuilder(invocationContext);
	}
}
