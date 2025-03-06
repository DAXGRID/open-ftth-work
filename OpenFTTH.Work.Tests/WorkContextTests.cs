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
using System.Threading.Tasks;

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
        public async Task GetUserCurrentWorkTaskOnUserThatHasNoWorkTaskSet_ShouldSucceed()
        {
            var result = await _queryDispatcher.HandleAsync<GetUserWorkContext, Result<UserWorkContext>>(new GetUserWorkContext("hans"));

            result.Value.CurrentWorkTask.Should().BeNull();
        }

        [Fact, Order(2)]
        public async Task SetUserCurrentToWorkTaskThatDoesNotExists_ShouldReturnNullWorkTask()
        {
            var workTaskId = Guid.NewGuid();

            var result = await _commandDispatcher.HandleAsync<SetUserCurrentWorkTask, Result<UserWorkContext>>(new SetUserCurrentWorkTask("hans", workTaskId));

            result.Value.CurrentWorkTask.Should().BeNull();
        }


        [Fact, Order(3)]
        public async Task SetUserCurrentToWorkTaskThatExists_ShouldSucceed()
        {
            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            workState.TryGetWorkTaskByNumber("W00001", out var existingWorkTask).Should().BeTrue();

            var result = await _commandDispatcher.HandleAsync<SetUserCurrentWorkTask, Result<UserWorkContext>>(new SetUserCurrentWorkTask("hans", existingWorkTask.Id));

            result.Value.CurrentWorkTask.Should().NotBeNull();

            _workContextManager.GetUsersAssignedToWorkTask(existingWorkTask.Id).Any(u => u == "hans").Should().BeTrue();
        }
    }
}
