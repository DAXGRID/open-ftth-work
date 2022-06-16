using OpenFTTH.Events;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskCreated : EventStoreBaseEvent
    {
        public WorkTask WorkTask { get; }
        public WorkTaskCreated(Guid workTaskId, WorkTask workTask)
        {
            WorkTaskId = workTaskId;
            WorkTask = workTask;
        }
    }
}
