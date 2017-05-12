    using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.AddProject
{
    public class ProjectCreated : Event
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public DateTime? Deadline { get; set; }

        public ProjectCreated(Guid id, string title, DateTime? deadline)
        {
            Id = id;
            Title = title;
            Deadline = deadline;
        }
    }
}