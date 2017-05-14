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
        public bool HasNote { get; }
        public bool Done { get; }

        public TaskViewModel(Guid id, Guid projectId, string title, DateTime? deadline, string priority, bool hasNote, bool done)
        {
            Id = id;
            ProjectId = projectId;
            Title = title;
            Deadline = deadline;
            Priority = priority;
            HasNote = hasNote;
            Done = done;
        }
    }
}