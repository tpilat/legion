using Legion.EntityFrameworkCore.Internals;
using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.EntityFrameworkCore.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddUnitOfWork<TUnitOfWork>(this IServiceCollection services, Func<IEFConnectionProvider, TUnitOfWork> unitOfWorkFactory)
		where TUnitOfWork : IUnitOfWork
	{
		Throw.IfArgumentNull(unitOfWorkFactory);

		UnitOfWorkProvider.RegisterUnitOfWorkFactory(unitOfWorkFactory);

		return services;
	}

	public static IServiceCollection AddQueryUnitOfWork<TQueryUnitOfWork>(this IServiceCollection services, Func<IEFConnectionProvider, TQueryUnitOfWork> queryUnitOfWorkFactory)
		where TQueryUnitOfWork : IQueryUnitOfWork
	{
		Throw.IfArgumentNull(queryUnitOfWorkFactory);

		UnitOfWorkProvider.RegisterQueryUnitOfWorkFactory(queryUnitOfWorkFactory);

		return services;
	}
}
