using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskSubtaskNameChanged : EventStoreBaseEvent
    {
        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;
        public string? SubtaskName { get; }

        public WorkTaskSubtaskNameChanged(Guid workTaskId, string? subtaskName)
        {
            base.WorkTaskId = workTaskId;
            SubtaskName = subtaskName;
        }
    }
}
