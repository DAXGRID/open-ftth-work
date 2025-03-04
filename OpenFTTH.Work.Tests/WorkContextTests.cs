using FluentAssertions;
using OpenFTTH.Results;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API.Commands;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.API.Queries;
using OpenFTTH.Work.Business;
using OpenFTTH.Work.Business.Projections;
using System;
using System.Linq;
using Xunit;
using Xunit.Extensions.Ordering;

namespace OpenFTTH.Work.Tests
{
    [Order(500)]
    public class WorkContextTests
    {
        private readonly IEventStore _eventStore;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly WorkContextManager _workContextManager;


        public WorkContextTests(IEventStore eventStore, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, WorkContextManager workContextManager)
        {
            _eventStore = eventStore;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
            _workContextManager = workContextManager;
        }

        [Fact, Order(1)]
        public void GetUserCurrentWorkTaskOnUserThatHasNoWorkTaskSet_ShouldSucceed()
        {
            var result = _queryDispatcher.HandleAsync<GetUserWorkContext, Result<UserWorkContext>>(new GetUserWorkContext("hans")).Result;

            result.Value.CurrentWorkTask.Should().BeNull();

        }


        [Fact, Order(2)]
        public void SetUserCurrentToWorkTaskThatDoesNotExists_ShouldReturnNullWorkTask()
        {
            var workTaskId = Guid.NewGuid();

            var result = _commandDispatcher.HandleAsync<SetUserCurrentWorkTask, Result<UserWorkContext>>(new SetUserCurrentWorkTask("hans", workTaskId)).Result;

            result.Value.CurrentWorkTask.Should().BeNull();
        }


        [Fact, Order(3)]
        public void SetUserCurrentToWorkTaskThatExists_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var result = _commandDispatcher.HandleAsync<SetUserCurrentWorkTask, Result<UserWorkContext>>(new SetUserCurrentWorkTask("hans", existingWorkTask.Id)).Result;

            result.Value.CurrentWorkTask.Should().NotBeNull();

            _workContextManager.GetUsersAssignedToWorkTask(existingWorkTask.Id).Any(u => u == "hans").Should().BeTrue();
        }
    }
}
