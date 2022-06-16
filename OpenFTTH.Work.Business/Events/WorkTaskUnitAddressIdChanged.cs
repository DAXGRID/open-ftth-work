using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskUnitAddressIdChanged : EventStoreBaseEvent
    {
        public Guid? UnitAddressId { get; }

        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;

        public WorkTaskUnitAddressIdChanged(Guid workTaskId, Guid? unitAddressId)
        {
            base.WorkTaskId = workTaskId;
            UnitAddressId = unitAddressId;
        }
    }
}
