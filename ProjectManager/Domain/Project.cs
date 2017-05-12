using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectManager.Features.AddProject;
using ProjectManager.Infrastructure;

namespace ProjectManager.Domain
{
    public class Project
    {
        public Guid Id { get { return State.Id; } }
        private List<Event> _events = new List<Event>();
        public ProjectState State { get; }
        public ReadOnlyCollection<Event> Events => _events.AsReadOnly();

        public Project(Guid id, string title, DateTime? deadline)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(nameof(title));
            
            State = new ProjectState(id, title);
            var projectCreated = new ProjectCreated(id, title, deadline);
            _events.Add(projectCreated);
        }

        public Project(ProjectState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            State = state;
        }

        protected bool Equals(Project other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Project) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ProjectState
    {
        public Guid Id { get; }
        public string Title { get; }

        public ProjectState(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}