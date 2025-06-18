using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.ESB.MBox;

public partial interface IMBoxUnitOfWork : Legion.Model.Repositories.IUnitOfWork
{

	Legion.ADF.ESB.MBox.Model.Repositories.IMessageRepository MessageRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IMessageContentRepository MessageContentRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IMessageProcessingLogRepository MessageProcessingLogRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IMessageProcessingStatusRepository MessageProcessingStatusRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IMessagePublishingRepository MessagePublishingRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IMessageStatusRepository MessageStatusRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IMessageTypeRepository MessageTypeRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IQueueRepository QueueRepository { get; }

	Legion.ADF.ESB.MBox.Model.Repositories.IQueuedMessageRepository QueuedMessageRepository { get; }
}
