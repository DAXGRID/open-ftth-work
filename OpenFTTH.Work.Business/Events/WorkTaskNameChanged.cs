using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskNameChanged : EventStoreBaseEvent
    {
        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;
        public string? Name { get; }

        public WorkTaskNameChanged(Guid workTaskId, string? name)
        {
            base.WorkTaskId = workTaskId;
            Name = name;
        }
    }
}
