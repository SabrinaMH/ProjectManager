    using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.AddProject
{
    public class ProjectCreated : Event
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }

        public ProjectCreated(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}