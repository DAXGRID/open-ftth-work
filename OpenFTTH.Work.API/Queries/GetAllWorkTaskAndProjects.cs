using OpenFTTH.Results;
using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.Work.API.Queries
{
    public class GetAllWorkTaskAndProjects : IQuery<Result<List<WorkTaskAndProject>>>
    {
        public GetAllWorkTaskAndProjects()
        {
        }
    }
}
