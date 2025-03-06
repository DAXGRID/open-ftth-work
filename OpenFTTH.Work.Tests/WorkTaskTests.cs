using FluentAssertions;
using OpenFTTH.Results;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.API.Queries;
using OpenFTTH.Work.Business;
using OpenFTTH.Work.Business.Projections;
using System;
using System.Linq;
using Xunit;
using Xunit.Extensions.Ordering;
using System.Threading.Tasks;

namespace OpenFTTH.Work.Tests
{
    [Order(200)]
    public class WorkTaskTests
    {
        private readonly IEventStore _eventStore;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public WorkTaskTests(IEventStore eventStore, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _eventStore = eventStore;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;

        }

        [Fact, Order(0)]
        public async Task GetUserContextWithoutAnyWorkTasks_ShouldSucceed()
        {
            var result = await _queryDispatcher.HandleAsync<GetUserWorkContext, Result<UserWorkContext>>(new GetUserWorkContext("unknown"));

            result.Value.CurrentWorkTask.Should().NotBeNull();
            result.Value.CurrentWorkTask.Id.Should().Be(Guid.Empty);
            result.Value.CurrentWorkTask.Number.Should().Be("DEFAULT");
        }

        [Fact, Order(1)]
        public void CreateWithoutProjectRef_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            var newWorkTask = new WorkTask(
                id: Guid.NewGuid(),
                createdDate: DateTime.Now,
                workProjectId: null,
                number: "W00001",
                name: "Work task without a project reference",
                subtaskName: null,
                type: "Do some usefull stuff",
                status: "Created",
                owner: "Big boss",
                installationId: null,
                areaId: null,
                unitAddressId: null
            );

            var workTaskAR = new WorkTaskAR();

            var createWorkProjectResult = workTaskAR.Create(newWorkTask, workState);

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            createWorkProjectResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(newWorkTask.Id, out var workProject);

            workProject.Should().NotBeNull();
            workProject.Should().Be(newWorkTask);
        }


        [Fact, Order(2)]
        public void CreateWithProjectRef_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetProjectByNumber("P00001", out var existingWorkProject).Should().BeTrue();

            var newWorkTask = new WorkTask(
                id: Guid.NewGuid(),
                createdDate: DateTime.Now,
                workProjectId: existingWorkProject.Id,
                number: "W00002",
                name: "Work task with a project reference",
                subtaskName: null,
                type: "Do some usefull stuff",
                status: "Created",
                owner: "Big boss",
                installationId: null,
                areaId: null,
                unitAddressId: null
            );

            var workTaskAR = new WorkTaskAR();

            var createWorkProjectResult = workTaskAR.Create(newWorkTask, workState);

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            createWorkProjectResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(newWorkTask.Id, out var workProject);

            workProject.Should().NotBeNull();
            workProject.Should().Be(newWorkTask);
        }


        [Fact, Order(3)]
        public void CreateWithNumberThatAlreadyExists_ShouldFail()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetProjectByNumber("P00001", out var existingWorkProject).Should().BeTrue();

            var newWorkTask = new WorkTask(
                id: Guid.NewGuid(),
                createdDate: DateTime.Now,
                workProjectId: existingWorkProject.Id,
                number: "W00002",
                name: "Work task with a project reference",
                subtaskName: null,
                type: "Do some usefull stuff",
                status: "Created",
                owner: "Big boss",
                installationId: null,
                areaId: null,
                unitAddressId: null
            );

            var workTaskAR = new WorkTaskAR();

            var createWorkTaskResult = workTaskAR.Create(newWorkTask, workState);

            // Assert
            createWorkTaskResult.IsSuccess.Should().BeFalse();

            ((WorkError)createWorkTaskResult.Errors.First()).Code.Should().Be(WorkErrorCodes.WORK_TASK_NUMBER_ALREADY_EXISTS);
        }

        [Fact, Order(10)]
        public void UpdateName_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateName("Jesper");

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.Name.Should().Be("Jesper");
        }

        [Fact, Order(10)]
        public void UpdateSubtaskName_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateSubtaskName("Subtask 3");

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.SubtaskName.Should().Be("Subtask 3");
        }

        [Fact, Order(10)]
        public void UpdateType_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateType("Do some other usefull stuff");

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.Type.Should().Be("Do some other usefull stuff");
        }

        [Fact, Order(11)]
        public void UpdateStatus_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateStatus("In Progress");

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.Status.Should().Be("In Progress");
        }

        [Fact, Order(11)]
        public void UpdateOwner_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateOwner("In Progress");

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.Owner.Should().Be("In Progress");
        }

        [Fact, Order(12)]
        public void UpdateInstallationIdToNonNullValue_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateInstallationsId("1234");

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.InstallationId.Should().Be("1234");
        }

        [Fact, Order(12)]
        public void UpdateInstallationIdToNullValue_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateInstallationsId(null);

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.InstallationId.Should().BeNull();
        }

        [Fact, Order(12)]
        public void UpdateAreaId_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var updateTypeResult = workTaskAR.UpdateAreaId("FK1234");

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.AreaId.Should().Be("FK1234");
        }

        [Fact, Order(13)]
        public void UpdateUnitAddressId_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var workTaskAR = _eventStore.Aggregates.Load<WorkTaskAR>(existingWorkTask.Id);

            var unitAddressId = Guid.NewGuid();

            var updateTypeResult = workTaskAR.UpdateUnitAddressId(unitAddressId);

            _eventStore.Aggregates.Store(workTaskAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkTask>(existingWorkTask.Id, out var updatedWorkTask);
            updatedWorkTask.UnitAddressId.Should().Be(unitAddressId);
        }

    }
}
