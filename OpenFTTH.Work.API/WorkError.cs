using FluentResults;

namespace OpenFTTH.Work.API
{
    public class WorkError : Error
    {
        public WorkErrorCodes Code { get; }

        public WorkError(WorkErrorCodes errorCode, string errorMsg) : base(errorCode.ToString() + ": " + errorMsg)
        {
            this.Code = errorCode;
            Metadata.Add("ErrorCode", errorCode.ToString());
        }
    }
}
