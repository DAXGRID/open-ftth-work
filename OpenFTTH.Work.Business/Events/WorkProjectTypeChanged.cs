using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkProjectTypeChanged : EventStoreBaseEvent
    {
        public Guid WorkProjectId { get;}
        public string? Type { get; }

        public WorkProjectTypeChanged(Guid workProjectId, string? type)
        {
            WorkProjectId = workProjectId;
            Type = type;
        }
    }
}
