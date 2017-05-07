using System;

namespace ProjectManager.ViewProjectsFeature
{
    public class ProjectViewModel
    {
        public string Title { get; }
        public Guid Id { get; }

        public ProjectViewModel(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}