using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.AddNote
{
    public class NoteDeleted : Event
    {
        public Guid Id { get; }
        public Guid TaskId { get; }

        public NoteDeleted(Guid id, Guid taskId)
        {
            Id = id;
            TaskId = taskId;
        }
    }
}