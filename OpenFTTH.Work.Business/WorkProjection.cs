using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.Business.Events;

namespace OpenFTTH.Work.Business.Projections
{
    public class WorkProjection : ProjectionBase
    {
        WorkState _state = new WorkState();
        public WorkState State => _state;

        public WorkProjection()
        {
            // Work project stuff

            ProjectEventAsync<WorkProjectCreated>(
                (@event) => {
                    _state.Add(((WorkProjectCreated)@event.Data).WorkProject);
                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkProjectNameChanged>(
                (@event) => {
                    var workProjectNameChangedEvent = (WorkProjectNameChanged)@event.Data;

                     if (_state.TryGet<WorkProject>(workProjectNameChangedEvent.WorkProjectId, out var existingWorkProject))
                     {
                         _state.Update(existingWorkProject with { Name = workProjectNameChangedEvent.Name });
                     }

                     return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkProjectTypeChanged>(
                (@event) => {
                    var workProjectTypeChangedEvent = (WorkProjectTypeChanged)@event.Data;

                    if (_state.TryGet<WorkProject>(workProjectTypeChangedEvent.WorkProjectId, out var existingWorkProject))
                    {
                        _state.Update(existingWorkProject with { Type = workProjectTypeChangedEvent.Type });
                    }

                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkProjectStatusChanged>(
               (@event) => {
                   var workProjectTypeChangedEvent = (WorkProjectStatusChanged)@event.Data;

                   if (_state.TryGet<WorkProject>(workProjectTypeChangedEvent.WorkProjectId, out var existingWorkProject))
                   {
                       _state.Update(existingWorkProject with { Status = workProjectTypeChangedEvent.Status });
                   }

                   return Task.CompletedTask;
               }
            );

            ProjectEventAsync<WorkProjectOwnerChanged>(
              (@event) => {
                  var workProjectOwnerChangedEvent = (WorkProjectOwnerChanged)@event.Data;

                  if (_state.TryGet<WorkProject>(workProjectOwnerChangedEvent.WorkProjectId, out var existingWorkProject))
                  {
                      _state.Update(existingWorkProject with { Owner = workProjectOwnerChangedEvent.Owner });
                  }

                  return Task.CompletedTask;
              }
            );


            // Work task stuff

            ProjectEventAsync<WorkTaskCreated>(
                (@event) => {
                    _state.Add(((WorkTaskCreated)@event.Data).WorkTask);
                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkTaskNameChanged>(
                (@event) => {
                    var workTaskNameChangedEvent = (WorkTaskNameChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskNameChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { Name = workTaskNameChangedEvent.Name });
                    }

                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkTaskSubtaskNameChanged>(
                (@event) => {
                    var workTaskSubtaskNameChangedEvent = (WorkTaskSubtaskNameChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskSubtaskNameChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { SubtaskName = workTaskSubtaskNameChangedEvent.SubtaskName });
                    }

                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkTaskTypeChanged>(
                (@event) => {
                    var workTaskTypeChangedEvent = (WorkTaskTypeChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskTypeChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { Type = workTaskTypeChangedEvent.Type });
                    }

                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkTaskStatusChanged>(
                (@event) => {
                    var workTaskTypeChangedEvent = (WorkTaskStatusChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskTypeChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { Status = workTaskTypeChangedEvent.Status });
                    }

                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkTaskOwnerChanged>(
                (@event) => {
                    var workTaskOwnerChangedEvent = (WorkTaskOwnerChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskOwnerChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { Owner = workTaskOwnerChangedEvent.Owner });
                    }

                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkTaskInstallationIdChanged>(
                (@event) => {
                    var workTaskInstallationIdChangedEvent = (WorkTaskInstallationIdChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskInstallationIdChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { InstallationId = workTaskInstallationIdChangedEvent.InstallationId });
                    }

                    return Task.CompletedTask;
                }
            );

            ProjectEventAsync<WorkTaskAreaIdChanged>(
               (@event) => {
                   var workTaskAreaIdChangedEvent = (WorkTaskAreaIdChanged)@event.Data;

                   if (_state.TryGet<WorkTask>(workTaskAreaIdChangedEvent.WorkTaskId, out var existingWorkTask))
                   {
                       _state.Update(existingWorkTask with { AreaId = workTaskAreaIdChangedEvent.AreaId });
                   }

                   return Task.CompletedTask;
               }
            );

            ProjectEventAsync<WorkTaskUnitAddressIdChanged>(
              (@event) => {
                  var workTaskUnitAddressIdChangedEvent = (WorkTaskUnitAddressIdChanged)@event.Data;

                  if (_state.TryGet<WorkTask>(workTaskUnitAddressIdChangedEvent.WorkTaskId, out var existingWorkTask))
                  {
                      _state.Update(existingWorkTask with { UnitAddressId = workTaskUnitAddressIdChangedEvent.UnitAddressId });
                  }

                  return Task.CompletedTask;
              }
           );
        }
    }
}
