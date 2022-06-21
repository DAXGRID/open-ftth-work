using FluentResults;
using OpenFTTH.Address.API.Model;
using OpenFTTH.Address.API.Queries;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.API.Queries;
using OpenFTTH.Work.Business.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFTTH.Work.Business.QueryHandlers
{
    public class GetAllWorkTaskAndProjectsQueryHandler
      : IQueryHandler<GetAllWorkTaskAndProjects, Result<List<WorkTaskAndProject>>>
    {
        private readonly IEventStore _eventStore;
        private readonly IQueryDispatcher _queryDispatcher;

        public GetAllWorkTaskAndProjectsQueryHandler(IEventStore eventStore, IQueryDispatcher queryDispatcher)
        {
            _eventStore = eventStore;
            _queryDispatcher = queryDispatcher;
        }

        public Task<Result<List<WorkTaskAndProject>>> HandleAsync(GetAllWorkTaskAndProjects query)
        {
            List<WorkTaskAndProject> workTaskAndProjects = new();

            var workState = _eventStore.Projections.Get<WorkProjection>().State;

            foreach (var workTask in workState.WorkTasks)
            {
                WorkProject? workProject = null;

                if (workTask.WorkProjectId != null)
                {
                    if (workState.TryGet<WorkProject>(workTask.WorkProjectId.Value, out var workProjectFound))
                        workProject = workProjectFound;
                }

                workTaskAndProjects.Add(new WorkTaskAndProject(workTask, workProject));
            }

            EnrichResultWithAddressAndCoordinateInformation(workTaskAndProjects);

            return Task.FromResult(Result.Ok(workTaskAndProjects));
        }

        private void EnrichResultWithAddressAndCoordinateInformation(List<WorkTaskAndProject> workTaskAndProjects)
        {
            List<Guid> addressIdsToQuery = new();

            foreach (var workTask in workTaskAndProjects)
            {
                if (workTask.WorkTask.UnitAddressId != null && workTask.WorkTask.UnitAddressId != Guid.Empty)
                {
                    addressIdsToQuery.Add(workTask.WorkTask.UnitAddressId.Value);
                }
            }

            var getAddressInfoQuery = new GetAddressInfo(addressIdsToQuery.ToArray());

            var addressResult = _queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQuery).Result;

            Dictionary<Guid, AddressAndCoordinateInfo> addressInfoById = new();

            if (addressResult.IsSuccess)
            {
                foreach (var addressHit in addressResult.Value.AddressHits)
                {
                    if (addressHit.RefClass == AddressEntityClass.UnitAddress)
                    {
                        var unitAddress = addressResult.Value.UnitAddresses[addressHit.RefId];
                        var accessAddress = addressResult.Value.AccessAddresses[unitAddress.AccessAddressId];

                        var addressStr = accessAddress.RoadName + " " + accessAddress.HouseNumber;

                        if (unitAddress.FloorName != null)
                            addressStr += (", " + unitAddress.FloorName);

                        if (unitAddress.SuitName != null)
                            addressStr += (" " + unitAddress.SuitName);

                        addressInfoById.Add(addressHit.Key, 
                            new AddressAndCoordinateInfo()
                            { 
                                AddressString = addressStr,
                                X = accessAddress.AddressPoint.X,
                                Y = accessAddress.AddressPoint.Y
                            }
                        );
                            
                    }
                    else
                    {
                        var accessAddress = addressResult.Value.AccessAddresses[addressHit.RefId];

                        var addressStr = accessAddress.RoadName + " " + accessAddress.HouseNumber;

                        addressInfoById.Add(addressHit.Key,
                            new AddressAndCoordinateInfo()
                            {
                                AddressString = addressStr,
                                X = accessAddress.AddressPoint.X,
                                Y = accessAddress.AddressPoint.Y
                            }
                        );
                    }
                }
            }

            foreach (var workTask in workTaskAndProjects)
            {
                if (workTask.WorkTask.UnitAddressId != null)
                {
                    if (addressInfoById.ContainsKey(workTask.WorkTask.UnitAddressId.Value))
                    {
                        var addressInfo = addressInfoById[workTask.WorkTask.UnitAddressId.Value];

                        workTask.AddressString = addressInfo.AddressString;
                        workTask.X = addressInfo.X;
                        workTask.Y = addressInfo.Y;
                    }
                }
            }
        }

        class AddressAndCoordinateInfo
        {
            public string AddressString { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
