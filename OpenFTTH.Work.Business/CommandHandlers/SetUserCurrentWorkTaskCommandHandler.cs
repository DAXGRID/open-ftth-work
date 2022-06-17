using FluentResults;
using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Commands;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.Work.Business.CommandHandlers
{
    public class SetUserCurrentWorkTaskCommandHandler :
       ICommandHandler<SetUserCurrentWorkTask, Result<UserWorkContext>>
    {
        private readonly WorkContextManager _workContextManager;

        public SetUserCurrentWorkTaskCommandHandler(WorkContextManager manager)
        {
            _workContextManager = manager;
        }

        public Task<Result<UserWorkContext>> HandleAsync(SetUserCurrentWorkTask command)
        {
            return Task.FromResult(
                Result.Ok(_workContextManager.SetUserCurrentWorkTask(command.UserName, command.WorkTaskId))
            );
        }
    }
}
