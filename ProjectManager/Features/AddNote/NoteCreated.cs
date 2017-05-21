using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.AddNote
{
    public class NoteCreated : Event
    {
        public Guid Id { get; }
        public Guid TaskId { get; }

        public NoteCreated(Guid id, Guid taskId)
        {
            Id = id;
            TaskId = taskId;
        }
    }
}