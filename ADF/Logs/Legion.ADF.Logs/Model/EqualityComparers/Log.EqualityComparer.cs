using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class Log : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.Log? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.Log>>? conditions = null)
		=> LogEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class LogEqualityComparer : IEqualityComparer<Log>
	{
		public static bool EqualsTo(
			Logs.Model.Log? obj1,
			Logs.Model.Log? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.Log>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			if (obj1 == null && obj2 == null)
				return true;

			if (obj1 == null || obj2 == null)
				return false;

			if (ReferenceEquals(obj1, obj2))
				return true;

			cache ??= [];

			cache.TryGetValue(obj1, out HashSet<object>? cachedHashSet);
			if (cachedHashSet?.Contains(obj2) == true)
				return true;
			
			ComparisonConditions<Logs.Model.Log>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.Log>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdLog)) && obj1.IdLog != obj2.IdLog)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.InternalMessage)) && !string.Equals(obj1.InternalMessage, obj2.InternalMessage))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ClientMessage)) && !string.Equals(obj1.ClientMessage, obj2.ClientMessage))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Detail)) && !string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StackTrace)) && !string.Equals(obj1.StackTrace, obj2.StackTrace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Component)) && !string.Equals(obj1.Component, obj2.Component))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.OperationName)) && !string.Equals(obj1.OperationName, obj2.OperationName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AggregateName)) && !string.Equals(obj1.AggregateName, obj2.AggregateName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AggregateIdentifier)) && !string.Equals(obj1.AggregateIdentifier, obj2.AggregateIdentifier))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CustomCorrelationId)) && !string.Equals(obj1.CustomCorrelationId, obj2.CustomCorrelationId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdApplicationEntry)) && obj1.IdApplicationEntry != obj2.IdApplicationEntry)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CorrelationId)) && obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExternalCorrelationId)) && !string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ContextProperties)) && !string.Equals(obj1.ContextProperties, obj2.ContextProperties))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdUser)) && obj1.IdUser != obj2.IdUser)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TenantIdentifier)) && obj1.TenantIdentifier != obj2.TenantIdentifier)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogLevel)) && obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LogCode)) && !string.Equals(obj1.LogCode, obj2.LogCode))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SourceSystemName)) && !string.Equals(obj1.SourceSystemName, obj2.SourceSystemName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceFrame)) && !string.Equals(obj1.TraceFrame, obj2.TraceFrame))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SourceContext)) && !string.Equals(obj1.SourceContext, obj2.SourceContext))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RuntimeUniqueKey)) && obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsValidationError)) && obj1.IsValidationError != obj2.IsValidationError)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.PropertyName)) && !string.Equals(obj1.PropertyName, obj2.PropertyName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DisplayPropertyName)) && !string.Equals(obj1.DisplayPropertyName, obj2.DisplayPropertyName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ValidationFailure)) && !string.Equals(obj1.ValidationFailure, obj2.ValidationFailure))
						return false;
				}
				else
				{
					if (obj1.IdLog != obj2.IdLog)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (!string.Equals(obj1.InternalMessage, obj2.InternalMessage))
						return false;
					if (!string.Equals(obj1.ClientMessage, obj2.ClientMessage))
						return false;
					if (!string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (!string.Equals(obj1.StackTrace, obj2.StackTrace))
						return false;
					if (!string.Equals(obj1.Component, obj2.Component))
						return false;
					if (!string.Equals(obj1.OperationName, obj2.OperationName))
						return false;
					if (!string.Equals(obj1.AggregateName, obj2.AggregateName))
						return false;
					if (!string.Equals(obj1.AggregateIdentifier, obj2.AggregateIdentifier))
						return false;
					if (!string.Equals(obj1.CustomCorrelationId, obj2.CustomCorrelationId))
						return false;
					if (obj1.IdApplicationEntry != obj2.IdApplicationEntry)
						return false;
					if (obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (!string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (!string.Equals(obj1.ContextProperties, obj2.ContextProperties))
						return false;
					if (obj1.IdUser != obj2.IdUser)
						return false;
					if (obj1.TenantIdentifier != obj2.TenantIdentifier)
						return false;
					if (obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (!string.Equals(obj1.LogCode, obj2.LogCode))
						return false;
					if (!string.Equals(obj1.SourceSystemName, obj2.SourceSystemName))
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (!string.Equals(obj1.TraceFrame, obj2.TraceFrame))
						return false;
					if (!string.Equals(obj1.SourceContext, obj2.SourceContext))
						return false;
					if (obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (obj1.IsValidationError != obj2.IsValidationError)
						return false;
					if (!string.Equals(obj1.PropertyName, obj2.PropertyName))
						return false;
					if (!string.Equals(obj1.DisplayPropertyName, obj2.DisplayPropertyName))
						return false;
					if (!string.Equals(obj1.ValidationFailure, obj2.ValidationFailure))
						return false;
				}
			}

			if (cachedHashSet == null)
			{
				cachedHashSet = [];
				cache[obj1] = cachedHashSet;
			}

			cachedHashSet.Add(obj2);

			return true;
		}

		public static int GetHashCode(
			Logs.Model.Log? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.Log>>? conditions = null,
			HashSet<object>? cache = null)
		{
			if (obj == null)
				return 0;

			cache ??= [];

			if (cache.Contains(obj))
				return 0;

				var hash = 1;
			return hash;
		}

		public ComparisonOptions ComparisonOptions { get; }
		public Action<ComparisonConditions<Logs.Model.Log>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public LogEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.Log>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.Log? obj1,
			Logs.Model.Log? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.Log? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
