using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskTypeChanged : EventStoreBaseEvent
    {
        public string? Type { get; }

        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;

        public WorkTaskTypeChanged(Guid workTaskId, string? type)
        {
            base.WorkTaskId = workTaskId;
            Type = type;
        }
    }
}
