using FluentAssertions;
using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.Business;
using OpenFTTH.Work.Business.Projections;
using System;
using System.Linq;
using Xunit;
using Xunit.Extensions.Ordering;

namespace OpenFTTH.Work.Tests
{
    [Order(100)]
    public class WorkProjectTests
    {
        private readonly IEventStore _eventStore;

        public WorkProjectTests(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [Fact, Order(1)]
        public void Create_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            var newWorkProject = new WorkProject(
                id: Guid.NewGuid(),
                createdDate: DateTime.Now,
                number: "P00001",
                name: "Super project 1",
                type: "Dig down some stuff",
                status: "In use",
                owner: "Grumme Hans"
            );

            var workProjectAR = new WorkProjectAR();

            var createWorkProjectResult = workProjectAR.Create(newWorkProject, workState);

            _eventStore.Aggregates.Store(workProjectAR);

            // Assert
            createWorkProjectResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkProject>(newWorkProject.Id, out var workProject);

            workProject.Should().NotBeNull();
            workProject.Should().Be(newWorkProject);
        }


        [Fact, Order(2)]
        public void CreateWithNumberThatAlreadyExists_ShouldFail()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            var newWorkProject = new WorkProject(
                id: Guid.NewGuid(),
                createdDate: DateTime.Now,
                number: "P00001",
                name: "Super project 1",
                type: "Dig down some stuff",
                status: "In use",
                owner: "Grumme Hans"
            );

            var workProjectAR = new WorkProjectAR();

            var createWorkProjectResult = workProjectAR.Create(newWorkProject, workState);

            // Assert
            createWorkProjectResult.IsSuccess.Should().BeFalse();

            ((WorkError)createWorkProjectResult.Errors.First()).Code.Should().Be(WorkErrorCodes.WORK_PROJECT_NUMBER_ALREADY_EXISTS);
        }

        [Fact, Order(3)]
        public void UpdateNAme_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetProjectByNumber("P00001", out var existingWorkProject).Should().BeTrue();

            var workProjectAR = _eventStore.Aggregates.Load<WorkProjectAR>(existingWorkProject.Id);

            var updateTypeResult = workProjectAR.UpdateName("Hi");

            _eventStore.Aggregates.Store(workProjectAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkProject>(existingWorkProject.Id, out var updatedWorkProject);
            updatedWorkProject.Name.Should().Be("Hi");
        }

        [Fact, Order(3)]
        public void UpdateType_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetProjectByNumber("P00001", out var existingWorkProject).Should().BeTrue();

            var workProjectAR = _eventStore.Aggregates.Load<WorkProjectAR>(existingWorkProject.Id);

            var updateTypeResult = workProjectAR.UpdateType("Dig some other stuff");

            _eventStore.Aggregates.Store(workProjectAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkProject>(existingWorkProject.Id, out var updatedWorkProject);
            updatedWorkProject.Type.Should().Be("Dig some other stuff");
        }

        [Fact, Order(4)]
        public void UpdateStatus_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetProjectByNumber("P00001", out var existingWorkProject).Should().BeTrue();

            var workProjectAR = _eventStore.Aggregates.Load<WorkProjectAR>(existingWorkProject.Id);

            var updateTypeResult = workProjectAR.UpdateStatus("Closed");

            _eventStore.Aggregates.Store(workProjectAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkProject>(existingWorkProject.Id, out var updatedWorkProject);
            updatedWorkProject.Status.Should().Be("Closed");
        }

        [Fact, Order(5)]
        public void UpdateOwner_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetProjectByNumber("P00001", out var existingWorkProject).Should().BeTrue();

            var workProjectAR = _eventStore.Aggregates.Load<WorkProjectAR>(existingWorkProject.Id);

            var updateTypeResult = workProjectAR.UpdateOwner("Bent");

            _eventStore.Aggregates.Store(workProjectAR);

            // Assert
            updateTypeResult.IsSuccess.Should().BeTrue();

            workState.TryGet<WorkProject>(existingWorkProject.Id, out var updatedWorkProject);
            updatedWorkProject.Owner.Should().Be("Bent");
        }
    }
}