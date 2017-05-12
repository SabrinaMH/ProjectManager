using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectManager.Features.AddTask;
using ProjectManager.Infrastructure;

namespace ProjectManager.Domain
{
    public class Task
    {
        public Guid Id => State.Id;
        public Guid ProjectId => State.ProjectId;

        private List<Event> _events = new List<Event>();
        public TaskState State { get; }
        public ReadOnlyCollection<Event> Events => _events.AsReadOnly();

        public Task(Guid id, Guid projectId, string title, string priority, DateTime? deadline)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(nameof(title));

            State = new TaskState(id, projectId, title, priority, deadline);
            var taskCreated = new TaskCreated(id, projectId, title, priority, deadline);
            _events.Add(taskCreated);
        }

        public Task(TaskState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            State = state;
        }

        protected bool Equals(Task other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Project)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class TaskState
    {
        public Guid Id { get; }
        public Guid ProjectId { get; }
        public string Title { get; }
        public DateTime? Deadline { get; }
        public bool IsDone { get; }
        public bool HasNote { get; }
        public string Priority { get; }

        public TaskState(Guid id, Guid projectId, string title, string priority, DateTime? deadline)
        {
            Id = id;
            ProjectId = projectId;
            Title = title;
            Deadline = deadline;
            Priority = priority;
        }
    }
}