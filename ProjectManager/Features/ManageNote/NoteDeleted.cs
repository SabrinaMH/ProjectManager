using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.ManageNote
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