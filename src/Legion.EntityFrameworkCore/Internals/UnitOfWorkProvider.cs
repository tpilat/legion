using Legion.Model.Repositories;
using System.Collections.Concurrent;

namespace Legion.EntityFrameworkCore.Internals;

internal class UnitOfWorkProvider : IUnitOfWorkProvider
{
	private readonly static ConcurrentDictionary<Type, Func<IEFConnectionProvider, IUnitOfWork>> _unitOfWorkFactories = [];
	private readonly static ConcurrentDictionary<Type, Func<IEFConnectionProvider, IQueryUnitOfWork>> _queryUnitOfWorkFactories = [];

	public static bool RegisterUnitOfWorkFactory<TUnitOfWork>(Func<IEFConnectionProvider, TUnitOfWork> factory)
		where TUnitOfWork : IUnitOfWork
		=> _unitOfWorkFactories.TryAdd(typeof(TUnitOfWork), efConnectionProvider => factory.Invoke(efConnectionProvider));

	public static bool RegisterQueryUnitOfWorkFactory<TQueryUnitOfWork>(Func<IEFConnectionProvider, TQueryUnitOfWork> factory)
		where TQueryUnitOfWork : IQueryUnitOfWork
		=> _queryUnitOfWorkFactories.TryAdd(typeof(TQueryUnitOfWork), efConnectionProvider => factory.Invoke(efConnectionProvider));

	private readonly IEFConnectionProvider _efConnectionProvider;

	public UnitOfWorkProvider(IEFConnectionProvider efConnectionProvider)
	{
		Throw.IfArgumentNull(efConnectionProvider);

		_efConnectionProvider = efConnectionProvider;
	}

	public IResult<TUnitOfWork> Create<TUnitOfWork>(IScopeContext scopeContext)
		where TUnitOfWork : IUnitOfWork
	{
		var result = new ResultBuilder<TUnitOfWork>();

		var unitOfWorkType = typeof(TUnitOfWork);
		if (!_unitOfWorkFactories.TryGetValue(unitOfWorkType, out var factory))
			return result.WithOutOfRangeException(scopeContext, unitOfWorkType);

		try
		{
			var uow = (TUnitOfWork)factory.Invoke(_efConnectionProvider);
			return result.WithData(uow).Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(
				scopeContext,
				Exceptions.Internal.ErrorCodes.UnitOfWorkException.UnhandledUnitOfWorkFactory(typeof(TUnitOfWork)),
				innerException: ex);
		}
	}

	public IResult<TQueryUnitOfWork> CreateQuery<TQueryUnitOfWork>(IScopeContext scopeContext)
		where TQueryUnitOfWork : IQueryUnitOfWork
	{
		var result = new ResultBuilder<TQueryUnitOfWork>();

		var unitOfWorkType = typeof(TQueryUnitOfWork);
		if (!_queryUnitOfWorkFactories.TryGetValue(unitOfWorkType, out var factory))
			return result.WithOutOfRangeException(scopeContext, unitOfWorkType);

		try
		{
			var uow = (TQueryUnitOfWork)factory.Invoke(_efConnectionProvider);
			return result.WithData(uow).Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(
				scopeContext,
				Exceptions.Internal.ErrorCodes.UnitOfWorkException.UnhandledUnitOfWorkFactory(typeof(TQueryUnitOfWork)),
				innerException: ex);
		}
	}
}
