using OpenFTTH.Events;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkProjectCreated : EventStoreBaseEvent
    {
        public Guid WorkProjectId { get;}
        public WorkProject WorkProject { get; }
        public WorkProjectCreated(Guid workProjectId, WorkProject workProject)
        {
            WorkProjectId = workProjectId;
            WorkProject = workProject;
        }
    }
}
