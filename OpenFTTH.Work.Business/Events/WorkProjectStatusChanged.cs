using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkProjectStatusChanged : EventStoreBaseEvent
    {
        public Guid WorkProjectId { get;}
        public string? Status { get; }

        public WorkProjectStatusChanged(Guid workProjectId, string? status)
        {
            WorkProjectId = workProjectId;
            Status = status;
        }
    }
}
