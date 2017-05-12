using System;

namespace ProjectManager.Features.ViewTaskList
{
    public class TaskViewModel
    {
        public string Title { get; }
        public DateTime? Deadline { get; }
        public string Priority { get; }
        public Guid Id { get; }
        public Guid ProjectId { get; }

        public TaskViewModel(Guid id, Guid projectId, string title, DateTime? deadline, string priority)
        {
            Id = id;
            ProjectId = projectId;
            Title = title;
            Deadline = deadline;
            Priority = priority;
        }
    }
}