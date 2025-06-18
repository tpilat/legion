using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ESB.Components;

public partial interface IComponentsUnitOfWork : Legion.Model.Repositories.IUnitOfWork
{

	Legion.ADF.ESB.Components.Model.Repositories.IAdapterRepository AdapterRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IAdapterLogRepository AdapterLogRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IAdapterRequestRepository AdapterRequestRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IAdapterRequestPayloadRepository AdapterRequestPayloadRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IAdapterResponseRepository AdapterResponseRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IAdapterResponsePayloadRepository AdapterResponsePayloadRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IAdapterStatusRepository AdapterStatusRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IJobRepository JobRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IJobDataRepository JobDataRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IJobLogRepository JobLogRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IJobStatusRepository JobStatusRepository { get; }

	Legion.ADF.ESB.Components.Model.Repositories.IJobTypeRepository JobTypeRepository { get; }
}
