using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskOwnerChanged : EventStoreBaseEvent
    {
        public string? Owner { get; }

        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;

        public WorkTaskOwnerChanged(Guid workTaskId, string? owner)
        {
            base.WorkTaskId = workTaskId;
            Owner = owner;
        }
    }
}
