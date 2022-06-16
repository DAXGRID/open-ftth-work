using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.Work.Business
{
    public class WorkState
    {
        private ConcurrentDictionary<Guid, WorkProject> _workProjectsById = new();
        private ConcurrentDictionary<Guid, WorkTask> _workTasksById = new();

        public IEnumerable<WorkProject> WorkProjects => _workProjectsById.Values;

        public IEnumerable<WorkTask> WorkTasks => _workTasksById.Values;

        public void Add(WorkProject workProject)
        {
            _workProjectsById.TryAdd(workProject.Id, workProject);
        }

        public void Add(WorkTask workTask)
        {
            _workTasksById.TryAdd(workTask.Id, workTask);
        }

        public void Update(WorkProject workProject)
        {
            if (_workProjectsById.ContainsKey(workProject.Id))
                _workProjectsById[workProject.Id] = workProject;
        }

        public void Update(WorkTask workTask)
        {
            if (_workTasksById.ContainsKey(workTask.Id))
                _workTasksById[workTask.Id] = workTask;
        }

        public bool TryGet<T>(Guid id, out T obj) where T : IWorkObject
        {
            if (_workProjectsById.TryGetValue(id, out WorkProject? workProject))
            {
                if (workProject is T)
                {
                    obj = (T)(object)workProject;
                    return true;
                }
            }
            else if (_workTasksById.TryGetValue(id, out WorkTask? workTask))
            {
                if (workTask is T)
                {
                    obj = (T)(object)workTask;
                    return true;
                }
            }

            #pragma warning disable CS8601 // Possible null reference assignment.
            obj = default(T);
            #pragma warning restore CS8601 // Possible null reference assignment.

            return false;
        }

        public bool TryGetProjectByNumber(string number, out WorkProject? workProject)
        {
            var foundWorkProject = _workProjectsById.Values.FirstOrDefault(w => w.Number == number);

            if (foundWorkProject != null)
            {
                workProject = foundWorkProject;
                return true;
            }

            #pragma warning disable CS8601 // Possible null reference assignment.
            workProject = default;
            #pragma warning restore CS8601 // Possible null reference assignment.

            return false;
        }

        public bool TryGetWorkTaskByNumber(string number, out WorkTask? workTask)
        {
            var foundWorkTask = _workTasksById.Values.FirstOrDefault(w => w.Number == number);

            if (foundWorkTask != null)
            {
                workTask = foundWorkTask;
                return true;
            }

            #pragma warning disable CS8601 // Possible null reference assignment.
            workTask = default;
            #pragma warning restore CS8601 // Possible null reference assignment.

            return false;
        }

        public bool CheckIfProjectNumberAlreadyUsed(string projectNumber)
        {
            var projectNumberLower = projectNumber.Trim().ToLower();

            if (_workProjectsById.Values.Any(w => w.Number.ToLower().Trim() == projectNumberLower))
                return true;

            return false;
        }

        public bool CheckIfWorkTaskNumberAlreadyUsed(string number)
        {
            var taskNumberLower = number.Trim().ToLower();

            if (_workTasksById.Values.Any(w => w.Number.ToLower().Trim() == taskNumberLower))
                return true;

            return false;
        }

       
    }
}
