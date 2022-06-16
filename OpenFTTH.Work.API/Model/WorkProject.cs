namespace OpenFTTH.Work.API.Model
{
    public record WorkProject : IWorkObject
    {
        public Guid Id { get; }
        public DateTime CreatedDate { get; }
        public string Number { get; }
        public string? Name { get; init; }
        public string? Type { get; init; }
        public string? Status { get; init; }
        public string? Owner { get; init; }

        public WorkProject(Guid id, DateTime createdDate, string number, string? name, string? type, string? status, string? owner)
        {
            Id = id;
            CreatedDate = createdDate;
            Number = number;
            Name = name;
            Type = type;
            Status = status;
            Owner = owner;
        }
    }
}
