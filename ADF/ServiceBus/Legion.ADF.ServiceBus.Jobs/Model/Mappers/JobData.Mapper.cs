using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobData : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobData? Map(
		Jobs.Model.JobData source,
		Jobs.Model.JobData? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobData? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobData? MapTo(
		Jobs.Model.JobData? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobData>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Jobs.Model.JobData>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Jobs.Model.JobData();

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobData)cached;
			
		MappingConditions<Jobs.Model.JobData>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobData>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJobData)))
				target.IdJobData = IdJobData;
			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(JobDataIdentifier)))
				target.JobDataIdentifier = JobDataIdentifier;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(LastModifiedUtc)))
				target.LastModifiedUtc = LastModifiedUtc;
			if (conds.CanMap(this, nameof(MimeType)))
				target.MimeType = MimeType;
			if (conds.CanMap(this, nameof(ContentEncoding)))
				target.ContentEncoding = ContentEncoding;
			if (conds.CanMap(this, nameof(ByteArrayContent)))
				target.ByteArrayContent = ByteArrayContent?.ToArray();
			if (conds.CanMap(this, nameof(JsonContent)))
				target.JsonContent = JsonContent;
			if (conds.CanMap(this, nameof(StringContent)))
				target.StringContent = StringContent;
			if (conds.CanMap(this, nameof(DbOid)))
				target.DbOid = DbOid;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(RelativePath)))
				target.RelativePath = RelativePath;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(IsCompressed)))
				target.IsCompressed = IsCompressed;
			if (conds.CanMap(this, nameof(EncryptionKey)))
				target.EncryptionKey = EncryptionKey;
		}
		else
		{
			target.IdJobData = IdJobData;
			target.IdJob = IdJob;
			target.JobDataIdentifier = JobDataIdentifier;
			target.CreatedUtc = CreatedUtc;
			target.LastModifiedUtc = LastModifiedUtc;
			target.MimeType = MimeType;
			target.ContentEncoding = ContentEncoding;
			target.ByteArrayContent = ByteArrayContent?.ToArray();
			target.JsonContent = JsonContent;
			target.StringContent = StringContent;
			target.DbOid = DbOid;
			target.Name = Name;
			target.RelativePath = RelativePath;
			target.Metadata = Metadata;
			target.IsCompressed = IsCompressed;
			target.EncryptionKey = EncryptionKey;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Job = Job?.MapTo(target.Job, referenceModifier, conds?.GetConditions(x => x.Job), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Job = null!;
		}

		return target;
	}
}
