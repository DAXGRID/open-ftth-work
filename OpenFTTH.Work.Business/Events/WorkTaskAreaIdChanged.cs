using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskAreaIdChanged : EventStoreBaseEvent
    {
        public string? AreaId { get; }

        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;

        public WorkTaskAreaIdChanged(Guid workTaskId, string? areaId)
        {
            base.WorkTaskId = workTaskId;
            AreaId = areaId;
        }
    }
}
