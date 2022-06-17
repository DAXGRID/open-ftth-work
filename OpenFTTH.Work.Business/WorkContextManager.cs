﻿using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.Business.Projections;

namespace OpenFTTH.Work.Business
{
    public class WorkContextManager
    {
        private readonly IEventStore _eventStore;
        private readonly WorkState _workState;

        private Dictionary<string, Guid> _assignedWorkTaskIdByUserName = new();

        private Dictionary<Guid, string[]> _userNamesByWorkTaskId = new();

        public WorkContextManager(IEventStore eventStore)
        {
            _eventStore = eventStore;
            _workState = _eventStore.Projections.Get<WorkProjection>().State;
        }

        public UserWorkContext GetUserWorkContext(string userName)
        {
            if (_assignedWorkTaskIdByUserName.TryGetValue(userName, out var workTaskId))
            {
                if (_workState.TryGet<WorkTask>(workTaskId, out var workTask))
                {
                    return new UserWorkContext(userName, workTask);
                }
            }

            return new UserWorkContext(userName, null);
        }

        public List<string> GetUsersAssignedToWorkTask(Guid workTaskId)
        {
            if (_userNamesByWorkTaskId.TryGetValue(workTaskId, out var users))
            {
                return users.ToList();
            }
            else
            {
                return new List<string>();
            }
        }


        public UserWorkContext SetUserCurrentWorkTask(string userName, Guid newTaskIdToSet)
        {
            if (_assignedWorkTaskIdByUserName.TryGetValue(userName, out var currentAssignedWorkTaskId))
            {
                if (currentAssignedWorkTaskId != newTaskIdToSet)
                {
                    _assignedWorkTaskIdByUserName[userName] = newTaskIdToSet;

                    UpdateUserNameIndex(userName, currentAssignedWorkTaskId, newTaskIdToSet);
                }
            }
            else
            {
                _assignedWorkTaskIdByUserName.Add(userName, newTaskIdToSet);
            }

            return GetUserWorkContext(userName);
        }

        private void UpdateUserNameIndex(string userName, Guid currentAssignedWorkTaskId, Guid newTaskIdToSet)
        {
            // Remove from current assinged
            if (_userNamesByWorkTaskId.TryGetValue(currentAssignedWorkTaskId, out var userNamesWhereToRemoveWorkTaskId))
            {
                _userNamesByWorkTaskId[currentAssignedWorkTaskId] =  userNamesWhereToRemoveWorkTaskId.Where(val => val != userName).ToArray();
            }

            // Add to new assigned work task id
            if (_userNamesByWorkTaskId.TryGetValue(newTaskIdToSet, out var newTaskIdUserNames))
            {
                if (!newTaskIdUserNames.Contains(userName))
                {
                    var newTaskIdUserNamesList = newTaskIdUserNames.ToList();
                    newTaskIdUserNamesList.Add(userName);

                    _userNamesByWorkTaskId[newTaskIdToSet] = newTaskIdUserNamesList.ToArray();
                }
            }
            else
            {
                _userNamesByWorkTaskId[newTaskIdToSet] = new string[] { userName };
            }
        }
    }
}
