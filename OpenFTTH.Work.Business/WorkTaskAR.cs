using OpenFTTH.Results;
using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.Business.Events;

namespace OpenFTTH.Work.Business
{
    /// <summary>
    /// Represents a work task/order
    /// </summary>
    public class WorkTaskAR : AggregateBase
    {
        private WorkTask? _workTaskState;

        public WorkTaskAR()
        {
            Register<WorkTaskCreated>(Apply);
            Register<WorkTaskNameChanged>(Apply);
            Register<WorkTaskSubtaskNameChanged>(Apply);
            Register<WorkTaskTypeChanged>(Apply);
            Register<WorkTaskStatusChanged>(Apply);
            Register<WorkTaskOwnerChanged>(Apply);
            Register<WorkTaskInstallationIdChanged>(Apply);
            Register<WorkTaskAreaIdChanged>(Apply);
            Register<WorkTaskUnitAddressIdChanged>(Apply);
        }

        #region Create

        public Result Create(WorkTask newWorkTask, WorkState state)
        {
            if (newWorkTask.Id == Guid.Empty)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_ID_CANNOT_BE_NULL_OR_EMPTY, "Invalid work task id. Cannot be null or empty."));

            if (state.TryGet<WorkTask>(newWorkTask.Id, out var _))
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_ID_ALREADY_EXISTS, $"The work task id: {newWorkTask.Id} already exists."));

            if (String.IsNullOrEmpty(newWorkTask.Number))
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_NUMBER_CANNOT_BE_NULL_OR_EMPTY, "Invalid work task number. Cannot be null or empty."));

            if (state.CheckIfWorkTaskNumberAlreadyUsed(newWorkTask.Number))
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_NUMBER_ALREADY_EXISTS, $"Work task number: {newWorkTask.Number} already used. Must be unique."));

            if (newWorkTask.WorkProjectId != null && !state.TryGet<WorkProject>(newWorkTask.WorkProjectId.Value, out var _))
                return Result.Fail(new WorkError(WorkErrorCodes.CANNOT_FIND_WORK_PROJECT_BY_ID, $"No work project with id: {newWorkTask.WorkProjectId.Value} found."));

            this.Id = newWorkTask.Id;

            RaiseEvent(new WorkTaskCreated(newWorkTask.Id, newWorkTask));

            return Result.Ok();
        }

        private void Apply(WorkTaskCreated @event)
        {
            _workTaskState = @event.WorkTask;
        }

        #endregion

        #region Update Name
        public Result UpdateName(string? name)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.SubtaskName == name)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_NAME_NOT_CHANGED, $"The work task name is already set to: {name}. Will not change anything."));

            RaiseEvent(new WorkTaskNameChanged(this.Id, name));

            return Result.Ok();
        }

        private void Apply(WorkTaskNameChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { Name = @event.Name };
        }

        #endregion

        #region Update Subtask Name
        public Result UpdateSubtaskName(string? name)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.Name == name)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_SUBTASK_NAME_NOT_CHANGED, $"The work task name is already set to: {name}. Will not change anything."));

            RaiseEvent(new WorkTaskSubtaskNameChanged(this.Id, name));

            return Result.Ok();
        }

        private void Apply(WorkTaskSubtaskNameChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { SubtaskName = @event.SubtaskName };
        }

        #endregion


        #region Update Type
        public Result UpdateType(string? type)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.Type == type)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_TYPE_NOT_CHANGED, $"The work task type is already set to: {type}. Will not change anything."));

            RaiseEvent(new WorkTaskTypeChanged(this.Id, type));

            return Result.Ok();
        }

        private void Apply(WorkTaskTypeChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { Type = @event.Type };
        }

        #endregion

        #region Update Status
        public Result UpdateStatus(string? status)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.Status == status)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_STATUS_NOT_CHANGED, $"The work task status is already set to: {status}. Will not change anything."));

            RaiseEvent(new WorkTaskStatusChanged(this.Id, status));

            return Result.Ok();
        }

        private void Apply(WorkTaskStatusChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { Status = @event.Status };
        }

        #endregion

        #region Update Owner
        public Result UpdateOwner(string? owner)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.Owner == owner)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_OWNER_NOT_CHANGED, $"The work task owner is already set to: {owner}. Will not change anything."));

            RaiseEvent(new WorkTaskOwnerChanged(this.Id, owner));

            return Result.Ok();
        }

        private void Apply(WorkTaskOwnerChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { Owner = @event.Owner };
        }

        #endregion

        #region Update Installation Id
        public Result UpdateInstallationsId(string? installationsId)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.InstallationId == installationsId)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_INSTALLATION_ID_NOT_CHANGED, $"The work task installation id is already set to: {installationsId}. Will not change anything."));

            RaiseEvent(new WorkTaskInstallationIdChanged(this.Id, installationsId));

            return Result.Ok();
        }

        private void Apply(WorkTaskInstallationIdChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { InstallationId = @event.InstallationId };
        }

        #endregion


        #region Update Area Id
        public Result UpdateAreaId(string? areaId)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.AreaId == areaId)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_AREA_ID_NOT_CHANGED, $"The work task area id is already set to: {areaId}. Will not change anything."));

            RaiseEvent(new WorkTaskAreaIdChanged(this.Id, areaId));

            return Result.Ok();
        }

        private void Apply(WorkTaskAreaIdChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { AreaId = @event.AreaId };
        }

        #endregion


        #region Update Unit Adress Id
        public Result UpdateUnitAddressId(Guid? unitAddressId)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            if (_workTaskState.UnitAddressId == unitAddressId)
                return Result.Fail(new WorkError(WorkErrorCodes.WORK_TASK_UNIT_ADDRESS_ID_NOT_CHANGED, $"The work task unit address id is already set to: {unitAddressId}. Will not change anything."));

            RaiseEvent(new WorkTaskUnitAddressIdChanged(this.Id, unitAddressId));

            return Result.Ok();
        }

        private void Apply(WorkTaskUnitAddressIdChanged @event)
        {
            if (_workTaskState == null)
                throw new ApplicationException($"Invalid internal state. State cannot be null. Seems that method is called on non yet created object.");

            _workTaskState = _workTaskState with { UnitAddressId = @event.UnitAddressId };
        }

        #endregion
    }
}
