using FluentResults;
using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.Work.API.Queries
{
    public class GetUserWorkContext : IQuery<Result<UserWorkContext>>
    {
        public string RequestName => typeof(GetUserWorkContext).Name;

        public string UserName { get; }

        public GetUserWorkContext(string userName)
        {
            UserName = userName;
        }
    }
}
