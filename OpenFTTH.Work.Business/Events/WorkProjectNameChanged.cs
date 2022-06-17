using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkProjectNameChanged : EventStoreBaseEvent
    {
        public Guid WorkProjectId { get;}
        public string? Name { get; }

        public WorkProjectNameChanged(Guid workProjectId, string? name)
        {
            WorkProjectId = workProjectId;
            Name = name;
        }
    }
}
