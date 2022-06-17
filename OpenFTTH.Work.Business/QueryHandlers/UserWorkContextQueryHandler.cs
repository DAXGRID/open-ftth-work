using FluentResults;
using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Model;
using OpenFTTH.Work.API.Queries;

namespace OpenFTTH.Work.Business.QueryHandlers
{
    public class UserWorkContextQueryHandler :
         IQueryHandler<GetUserWorkContext, Result<UserWorkContext>>
    {
        private readonly WorkContextManager _workContextManager;

        public UserWorkContextQueryHandler(WorkContextManager manager)
        {
            _workContextManager = manager;
        }

        public Task<Result<UserWorkContext>> HandleAsync(GetUserWorkContext query)
        {
            return Task.FromResult(
               Result.Ok(_workContextManager.GetUserWorkContext(query.UserName))
            );
        }
    }
}
