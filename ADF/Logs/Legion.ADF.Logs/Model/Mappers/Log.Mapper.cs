using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class Log : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.Log? Map(
		Logs.Model.Log source,
		Logs.Model.Log? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.Log>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.Log? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.Log>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.Log? MapTo(
		Logs.Model.Log? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.Log>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.Log>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.Log();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.Log)cached;
			
		MappingConditions<Logs.Model.Log>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.Log>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdLog)))
				target.IdLog = IdLog;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(InternalMessage)))
				target.InternalMessage = InternalMessage;
			if (conds.CanMap(this, nameof(ClientMessage)))
				target.ClientMessage = ClientMessage;
			if (conds.CanMap(this, nameof(Detail)))
				target.Detail = Detail;
			if (conds.CanMap(this, nameof(StackTrace)))
				target.StackTrace = StackTrace;
			if (conds.CanMap(this, nameof(Component)))
				target.Component = Component;
			if (conds.CanMap(this, nameof(OperationName)))
				target.OperationName = OperationName;
			if (conds.CanMap(this, nameof(AggregateName)))
				target.AggregateName = AggregateName;
			if (conds.CanMap(this, nameof(AggregateIdentifier)))
				target.AggregateIdentifier = AggregateIdentifier;
			if (conds.CanMap(this, nameof(CustomCorrelationId)))
				target.CustomCorrelationId = CustomCorrelationId;
			if (conds.CanMap(this, nameof(IdApplicationEntry)))
				target.IdApplicationEntry = IdApplicationEntry;
			if (conds.CanMap(this, nameof(CorrelationId)))
				target.CorrelationId = CorrelationId;
			if (conds.CanMap(this, nameof(ExternalCorrelationId)))
				target.ExternalCorrelationId = ExternalCorrelationId;
			if (conds.CanMap(this, nameof(ContextProperties)))
				target.ContextProperties = ContextProperties;
			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(TenantIdentifier)))
				target.TenantIdentifier = TenantIdentifier;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
			if (conds.CanMap(this, nameof(LogCode)))
				target.LogCode = LogCode;
			if (conds.CanMap(this, nameof(SourceSystemName)))
				target.SourceSystemName = SourceSystemName;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(TraceFrame)))
				target.TraceFrame = TraceFrame;
			if (conds.CanMap(this, nameof(SourceContext)))
				target.SourceContext = SourceContext;
			if (conds.CanMap(this, nameof(RuntimeUniqueKey)))
				target.RuntimeUniqueKey = RuntimeUniqueKey;
			if (conds.CanMap(this, nameof(IsValidationError)))
				target.IsValidationError = IsValidationError;
			if (conds.CanMap(this, nameof(PropertyName)))
				target.PropertyName = PropertyName;
			if (conds.CanMap(this, nameof(DisplayPropertyName)))
				target.DisplayPropertyName = DisplayPropertyName;
			if (conds.CanMap(this, nameof(ValidationFailure)))
				target.ValidationFailure = ValidationFailure;
		}
		else
		{
			target.IdLog = IdLog;
			target.CreatedUtc = CreatedUtc;
			target.InternalMessage = InternalMessage;
			target.ClientMessage = ClientMessage;
			target.Detail = Detail;
			target.StackTrace = StackTrace;
			target.Component = Component;
			target.OperationName = OperationName;
			target.AggregateName = AggregateName;
			target.AggregateIdentifier = AggregateIdentifier;
			target.CustomCorrelationId = CustomCorrelationId;
			target.IdApplicationEntry = IdApplicationEntry;
			target.CorrelationId = CorrelationId;
			target.ExternalCorrelationId = ExternalCorrelationId;
			target.ContextProperties = ContextProperties;
			target.IdUser = IdUser;
			target.TenantIdentifier = TenantIdentifier;
			target.IdLogLevel = IdLogLevel;
			target.LogCode = LogCode;
			target.SourceSystemName = SourceSystemName;
			target.TraceCorrelationId = TraceCorrelationId;
			target.TraceFrame = TraceFrame;
			target.SourceContext = SourceContext;
			target.RuntimeUniqueKey = RuntimeUniqueKey;
			target.IsValidationError = IsValidationError;
			target.PropertyName = PropertyName;
			target.DisplayPropertyName = DisplayPropertyName;
			target.ValidationFailure = ValidationFailure;
		}

		cache.Add(this, target);

		return target;
	}
}
