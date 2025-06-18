//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using System.Data.Common;

//namespace Legion.Interceptors;

//public abstract class ExceptionProcessorInterceptor<T> : SaveChangesInterceptor
//	where T : DbException
//{
//	protected internal enum DatabaseError
//	{
//		UniqueConstraint,
//		CannotInsertNull,
//		MaxLength,
//		NumericOverflow,
//		ReferenceConstraint
//	}

//	protected abstract DatabaseError? GetDatabaseError(T dbException);

//	/// <inheritdoc />
//	public override void SaveChangesFailed(DbContextErrorEventData eventData)
//	{
//		if (eventData.Exception is DbUpdateException dbUpdateException
//			&& eventData.Exception.GetBaseException() is T providerException)
//		{
//			var error = GetDatabaseError(providerException);

//			if (error != null && dbUpdateException != null)
//			{
//				var exception = ExceptionFactory.Create(error.Value, dbUpdateException, dbUpdateException.Entries);
//				throw exception;
//			}
//		}

//		base.SaveChangesFailed(eventData);
//	}

//	/// <inheritdoc />
//	public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = new CancellationToken())
//	{
//		if (eventData.Exception is DbUpdateException dbUpdateException
//			&& eventData.Exception.GetBaseException() is T providerException)
//		{
//			var error = GetDatabaseError(providerException);

//			if (error != null && dbUpdateException != null)
//			{
//				var exception = ExceptionFactory.Create(error.Value, dbUpdateException, dbUpdateException.Entries);
//				throw exception;
//			}
//		}

//		return base.SaveChangesFailedAsync(eventData, cancellationToken);
//	}
//}