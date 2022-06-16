using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkProjectOwnerChanged : EventStoreBaseEvent
    {
        public Guid WorkProjectId { get;}
        public string? Owner { get; }

        public WorkProjectOwnerChanged(Guid workProjectId, string? owner)
        {
            WorkProjectId = workProjectId;
            Owner = owner;
        }
    }
}
