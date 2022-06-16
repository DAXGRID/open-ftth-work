using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskStatusChanged : EventStoreBaseEvent
    {
        public string? Status { get; }

        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;

        public WorkTaskStatusChanged(Guid workTaskId, string? status)
        {
            base.WorkTaskId = workTaskId;
            Status = status;
        }
    }
}
