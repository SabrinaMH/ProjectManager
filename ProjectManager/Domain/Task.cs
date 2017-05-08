using System;

namespace ProjectManager.Domain
{
    public class TaskState
    {
        public string Title { get; }
        public DateTime Deadline { get; }
        public bool IsDone { get; }
        public bool HasNote { get; }
        public string Priority { get; }

        public TaskState(string title, DateTime deadline, bool isDone, bool hasNote, string priority)
        {
            Title = title;
            Deadline = deadline;
            IsDone = isDone;
            HasNote = hasNote;
            Priority = priority;
        }

        protected bool Equals(TaskState other)
        {
            return string.Equals(Title, other.Title) && Deadline.Equals(other.Deadline) && IsDone == other.IsDone && HasNote == other.HasNote && string.Equals(Priority, other.Priority);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskState) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Deadline.GetHashCode();
                hashCode = (hashCode * 397) ^ IsDone.GetHashCode();
                hashCode = (hashCode * 397) ^ HasNote.GetHashCode();
                hashCode = (hashCode * 397) ^ (Priority != null ? Priority.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}