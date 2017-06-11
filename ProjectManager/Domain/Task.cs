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

        public Task(Guid id, Guid projectId, string title, string priority, DateTime? deadline, int sendEmailNumberOfDaysBeforeDeadline)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(nameof(title));

            State = new TaskState(id, projectId, title, priority, deadline, sendEmailNumberOfDaysBeforeDeadline);
            var taskCreated = new TaskCreated(id, projectId, title, priority, deadline, sendEmailNumberOfDaysBeforeDeadline);
            _events.Add(taskCreated);
        }

        public Task(TaskState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            State = state;
        }

        public void Update(string title, string priority, DateTime? deadline, int sendEmailNumberOfDaysBeforeDeadline)
        {
            State.Title = title;
            State.Priority = priority;
            State.Deadline = deadline;
            State.SendEmailNumberOfDaysBeforeDeadline = sendEmailNumberOfDaysBeforeDeadline;
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
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsDone { get; }
        public bool HasNote { get; set; }
        public string Priority { get; set; }
        public int SendEmailNumberOfDaysBeforeDeadline { get; set; }
        public bool IsEmailSent { get; set; }

        public TaskState(Guid id, Guid projectId, string title, string priority, DateTime? deadline, int sendEmailNumberOfDaysBeforeDeadline)
        {
            Id = id;
            ProjectId = projectId;
            Title = title;
            Deadline = deadline;
            SendEmailNumberOfDaysBeforeDeadline = sendEmailNumberOfDaysBeforeDeadline;
            Priority = priority;
            IsEmailSent = false;
        }

    }
}