using System;

namespace ProjectManager.Features.ViewProjectList
{
    public class ProjectViewModel
    {
        public string Title { get; }
        public DateTime? Deadline { get; }
        public Guid Id { get; }

        public ProjectViewModel(Guid id, string title, DateTime? deadline)
        {
            Id = id;
            Title = title;
            Deadline = deadline;
        }
    }
}