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

            ProjectEvent<WorkProjectCreated>(
                (@event) => {
                    _state.Add(((WorkProjectCreated)@event.Data).WorkProject);
                }
            );

            ProjectEvent<WorkProjectTypeChanged>(
                (@event) => {
                    var workProjectTypeChangedEvent = (WorkProjectTypeChanged)@event.Data;

                    if (_state.TryGet<WorkProject>(workProjectTypeChangedEvent.WorkProjectId, out var existingWorkProject))
                    {
                        _state.Update(existingWorkProject with { Type = workProjectTypeChangedEvent.Type });
                    }
                }
            );

            ProjectEvent<WorkProjectStatusChanged>(
               (@event) => {
                   var workProjectTypeChangedEvent = (WorkProjectStatusChanged)@event.Data;

                   if (_state.TryGet<WorkProject>(workProjectTypeChangedEvent.WorkProjectId, out var existingWorkProject))
                   {
                       _state.Update(existingWorkProject with { Status = workProjectTypeChangedEvent.Status });
                   }
               }
            );

            ProjectEvent<WorkProjectOwnerChanged>(
              (@event) => {
                  var workProjectOwnerChangedEvent = (WorkProjectOwnerChanged)@event.Data;

                  if (_state.TryGet<WorkProject>(workProjectOwnerChangedEvent.WorkProjectId, out var existingWorkProject))
                  {
                      _state.Update(existingWorkProject with { Owner = workProjectOwnerChangedEvent.Owner });
                  }
              }
            );


            // Work task stuff

            ProjectEvent<WorkTaskCreated>(
                (@event) => {
                    _state.Add(((WorkTaskCreated)@event.Data).WorkTask);
                }
            );

            ProjectEvent<WorkTaskTypeChanged>(
                (@event) => {
                    var workTaskTypeChangedEvent = (WorkTaskTypeChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskTypeChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { Type = workTaskTypeChangedEvent.Type });
                    }
                }
            );

            ProjectEvent<WorkTaskStatusChanged>(
                (@event) => {
                    var workTaskTypeChangedEvent = (WorkTaskStatusChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskTypeChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { Status = workTaskTypeChangedEvent.Status });
                    }
                }
            );

            ProjectEvent<WorkTaskOwnerChanged>(
                (@event) => {
                    var workTaskOwnerChangedEvent = (WorkTaskOwnerChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskOwnerChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { Owner = workTaskOwnerChangedEvent.Owner });
                    }
                }
            );

            ProjectEvent<WorkTaskInstallationIdChanged>(
                (@event) => {
                    var workTaskInstallationIdChangedEvent = (WorkTaskInstallationIdChanged)@event.Data;

                    if (_state.TryGet<WorkTask>(workTaskInstallationIdChangedEvent.WorkTaskId, out var existingWorkTask))
                    {
                        _state.Update(existingWorkTask with { InstallationId = workTaskInstallationIdChangedEvent.InstallationId });
                    }
                }
            );

            ProjectEvent<WorkTaskAreaIdChanged>(
               (@event) => {
                   var workTaskAreaIdChangedEvent = (WorkTaskAreaIdChanged)@event.Data;

                   if (_state.TryGet<WorkTask>(workTaskAreaIdChangedEvent.WorkTaskId, out var existingWorkTask))
                   {
                       _state.Update(existingWorkTask with { AreaId = workTaskAreaIdChangedEvent.AreaId });
                   }
               }
            );

            ProjectEvent<WorkTaskUnitAddressIdChanged>(
              (@event) => {
                  var workTaskUnitAddressIdChangedEvent = (WorkTaskUnitAddressIdChanged)@event.Data;

                  if (_state.TryGet<WorkTask>(workTaskUnitAddressIdChangedEvent.WorkTaskId, out var existingWorkTask))
                  {
                      _state.Update(existingWorkTask with { UnitAddressId = workTaskUnitAddressIdChangedEvent.UnitAddressId });
                  }
              }
           );

        }
    }
}
