using OpenFTTH.Results;
using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.Business.Events;

namespace OpenFTTH.Work.Business
{
    /// <summary>
    /// Represents a project or case, that one or more work tasks can be related to
    /// </summary>
    public class WorkProjectAR : AggregateBase
    {
        private WorkProject? _workProjectState;

        public WorkProjectAR()
        {
            Register<WorkProjectCreated>(Apply);
            Register<WorkProjectNameChanged>(Apply);
            Register<WorkProjectTypeChanged>(Apply);
            Register<WorkProjectStatusChanged>(Apply);
            Register<WorkProjectOwnerChanged>(Apply);
        }

        #region Create

        public Result Create(WorkProject newWorkObject, WorkState state)
        {
            if (newWorkObject.Id == Guid.Empty)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_PROJECT_ID_CANNOT_BE_NULL_OR_EMPTY, "Invalid work project id. Cannot be null or empty."));

            if (state.TryGet<WorkProject>(newWorkObject.Id, out var _))
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_PROJECT_ID_ALREADY_EXISTS, $"The work project id: {newWorkObject.Id} already exists."));

            if (String.IsNullOrEmpty(newWorkObject.Number))
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_PROJECT_NUMBER_CANNOT_BE_NULL_OR_EMPTY, "Invalid work project number. Cannot be null or empty."));

            if (state.CheckIfProjectNumberAlreadyUsed(newWorkObject.Number))
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_PROJECT_NUMBER_ALREADY_EXISTS, $"Work project number: {newWorkObject.Number} already used. Must be unique."));

            this.Id = newWorkObject.Id;

            RaiseEvent(new WorkProjectCreated(newWorkObject.Id, newWorkObject));

            return Result.Ok();
        }

        private void Apply(WorkProjectCreated @event)
        {
            _workProjectState = @event.WorkProject;
        }

        #endregion

        #region Update Name
        public Result UpdateName(string? name)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workProjectState.Name == name)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_PROJECT_NAME_NOT_CHANGED, $"The work project name is already set to: {name}. Will not change anything."));

            RaiseEvent(new WorkProjectNameChanged(this.Id, name));

            return Result.Ok();
        }

        private void Apply(WorkProjectNameChanged @event)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workProjectState = _workProjectState with { Name = @event.Name };
        }

        #endregion


        #region Update Type
        public Result UpdateType(string? type)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workProjectState.Type == type)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_PROJECT_TYPE_NOT_CHANGED, $"The work project type is already set to: {type}. Will not change anything."));

            RaiseEvent(new WorkProjectTypeChanged(this.Id, type));

            return Result.Ok();
        }

        private void Apply(WorkProjectTypeChanged @event)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workProjectState = _workProjectState with { Type = @event.Type };
        }

        #endregion

        #region Update Status
        public Result UpdateStatus(string? status)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workProjectState.Status == status)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_STATUS_NOT_CHANGED, $"The work project status is already set to: {status}. Will not change anything."));

            RaiseEvent(new WorkProjectStatusChanged(this.Id, status));

            return Result.Ok();
        }

        private void Apply(WorkProjectStatusChanged @event)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workProjectState = _workProjectState with { Status = @event.Status };
        }

        #endregion

        #region Update Owner
        public Result UpdateOwner(string? owner)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workProjectState.Owner == owner)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_PROJECT_OWNER_NOT_CHANGED, $"The work project owner is already set to: {owner}. Will not change anything."));

            RaiseEvent(new WorkProjectOwnerChanged(this.Id, owner));

            return Result.Ok();
        }

        private void Apply(WorkProjectOwnerChanged @event)
        {
            if (_workProjectState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workProjectState = _workProjectState with { Owner = @event.Owner };
        }

        #endregion
    }
}
