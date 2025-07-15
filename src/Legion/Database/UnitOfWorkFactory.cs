using Legion.Model.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.Database;

public static class UnitOfWorkFactory<TUoW, TCSP>
	where TUoW : Model.Repositories.IUnitOfWork
	where TCSP : class, IConnectionStringProvider
{
	public static  TUoW CreateUnitOfWorkWithoutTransaction(
		IServiceProvider serviceProvider,
		string? storeId = null,
		bool? allowLocking = null,
		bool createAuditEntryStore = false)
	{
		Throw.IfArgumentNull(serviceProvider);

		var connectionStringProvider = serviceProvider.GetRequiredService<TCSP>();
		var uowFactrory = serviceProvider.GetRequiredService<IUnitOfWorkFactory<TUoW>>();
		var uow = uowFactrory.CreateWithoutTransaction(
			serviceProvider,
			connectionStringProvider.GetConncetionString(storeId!),
			allowLocking,
			createAuditEntryStore);

		return uow;
	}

	public static TUoW CreateUnitOfWorkWithTransaction(
		IServiceProvider serviceProvider,
		string? storeId = null,
		System.Data.IsolationLevel? isolationLevel = null,
		bool? allowLocking = null,
		bool createAuditEntryStore = false)
	{
		Throw.IfArgumentNull(serviceProvider);

		var connectionStringProvider = serviceProvider.GetRequiredService<TCSP>();
		var uowFactrory = serviceProvider.GetRequiredService<IUnitOfWorkFactory<TUoW>>();
		var uow = uowFactrory.Create(
			serviceProvider,
			connectionStringProvider.GetConncetionString(storeId!),
			isolationLevel,
			allowLocking,
			createAuditEntryStore);

		return uow;
	}

	public static TQUoW GetQueryUnitOfWork<TQUoW>(IServiceProvider serviceProvider, TUoW uow)
		where TQUoW : Legion.Model.Repositories.IQueryUnitOfWork
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(uow);

		var quowFactrory = serviceProvider.GetRequiredService<IQueryUnitOfWorkFactory<TQUoW>>();
		var quow = quowFactrory.Create(uow);
		return quow;
	}
}
