namespace OpenFTTH.Work.API.Model
{
    public record WorkTask : IWorkObject
    {
        public Guid Id { get; }
        public DateTime CreatedDate { get; }
        public Guid? WorkProjectId { get; }
        public string Number { get; }
        public string? Name { get; }
        public string? SubtaskName { get; }
        public string? Type { get; init; }
        public string? Status { get; init; }
        public string? Owner { get; init; }
        public string? InstallationId { get; init; }
        public string? AreaId { get; init; }
        public Guid? UnitAddressId { get; init; }

        public WorkTask(Guid id, DateTime createdDate, Guid? workProjectId, string number, string? name, string? subtaskName, string? type, string? status, string? owner, string? installationId, string? areaId, Guid? unitAddressId)
        {
            Id = id;
            CreatedDate = createdDate;
            WorkProjectId = workProjectId;
            Number = number;
            Name = name;
            SubtaskName = subtaskName;
            Type = type;
            Status = status;
            Owner = owner;
            InstallationId = installationId;
            AreaId = areaId;
            UnitAddressId = unitAddressId;
        }
    }
}
