namespace SmartTaskManager.Core.Models
{
    /// <summary>
    /// Represents a single task belonging to a project.
    /// </summary>
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}