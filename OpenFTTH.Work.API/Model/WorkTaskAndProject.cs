namespace OpenFTTH.Work.API.Model
{
    public record WorkTaskAndProject
    {
        public WorkTask WorkTask { get; }
        public WorkProject? WorkProject { get;}
        public WorkTaskAndProject(WorkTask workTask, WorkProject? workProject)
        {
            WorkTask = workTask;
            WorkProject = workProject;
        }
    }
}
