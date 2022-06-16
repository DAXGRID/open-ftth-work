using OpenFTTH.Events;

namespace OpenFTTH.Work.Business.Events
{
    public record WorkTaskInstallationIdChanged : EventStoreBaseEvent
    {
        public string? InstallationId { get; }

        public new Guid WorkTaskId => base.WorkTaskId == null ? Guid.Empty : base.WorkTaskId.Value;

        public WorkTaskInstallationIdChanged(Guid workTaskId, string? installationId)
        {
            base.WorkTaskId = workTaskId;
            InstallationId = installationId;
        }
    }
}
