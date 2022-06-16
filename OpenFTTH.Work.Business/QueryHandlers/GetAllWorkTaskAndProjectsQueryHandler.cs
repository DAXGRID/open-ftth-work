using FluentResults;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.API.Queries;
using OpenFTTH.Work.Business.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFTTH.Work.Business.QueryHandlers
{
    public class GetAllWorkTaskAndProjectsQueryHandler
      : IQueryHandler<GetAllWorkTaskAndProjects, Result<List<WorkTaskAndProject>>>
    {
        private readonly IEventStore _eventStore;

        public GetAllWorkTaskAndProjectsQueryHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<Result<List<WorkTaskAndProject>>> HandleAsync(GetAllWorkTaskAndProjects query)
        {
            List<WorkTaskAndProject> workTaskAndProjects = new();

            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            foreach (var workTask in workState.WorkTasks)
            {
                WorkProject? workProject = null;

                if (workTask.WorkProjectId != null)
                {
                    if (workState.TryGet<WorkProject>(workTask.WorkProjectId.Value, out var workProjectFound))
                        workProject = workProjectFound;
                }

                workTaskAndProjects.Add(new WorkTaskAndProject(workTask, workProject));
            }

            return Task.FromResult(Result.Ok(workTaskAndProjects));
        }
    }
}
