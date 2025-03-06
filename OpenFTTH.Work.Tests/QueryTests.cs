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
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions.Ordering;
using System.Threading.Tasks;

namespace OpenFTTH.Work.Tests
{
    [Order(300)]
    public class QueryTests
    {
        private readonly IEventStore _eventStore;
        private readonly IQueryDispatcher _queryDispatcher;

        public QueryTests(IEventStore eventStore, IQueryDispatcher queryDispatcher)
        {
            _eventStore = eventStore;
            _queryDispatcher = queryDispatcher;
        }

        [Fact, Order(1)]
        public async Task QueryWorkTaskAndProjects_ShouldSucceed()
        {
            var workTaskAndProjectsList = (await _queryDispatcher.HandleAsync<GetAllWorkTaskAndProjects, Result<List<WorkTaskAndProject>>>(new GetAllWorkTaskAndProjects())).Value.ToList();

            workTaskAndProjectsList.Count(w => w.WorkTask != null).Should().BeGreaterThan(0);
            workTaskAndProjectsList.Count(w => w.WorkProject != null).Should().BeGreaterThan(0);
        }
    }
}
