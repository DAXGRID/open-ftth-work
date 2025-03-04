using OpenFTTH.Results;
using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.Work.API.Commands
{

    public class SetUserCurrentWorkTask : ICommand<Result<UserWorkContext>>
    {
        public string RequestName => typeof(SetUserCurrentWorkTask).Name;

        public string UserName { get; }
        public Guid WorkTaskId { get; }

        public SetUserCurrentWorkTask(string userName, Guid workTaksId)
        {
            UserName = userName;
            WorkTaskId = workTaksId;
        }
    }
}
