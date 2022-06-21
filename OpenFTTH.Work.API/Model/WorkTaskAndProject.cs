namespace OpenFTTH.Work.API.Model
{
    public record WorkTaskAndProject
    {
        public WorkTask WorkTask { get; }
        public WorkProject? WorkProject { get; }
        public string AddressString { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public WorkTaskAndProject(WorkTask workTask, WorkProject? workProject)
        {
            WorkTask = workTask;
            WorkProject = workProject;
        }
    }
}
