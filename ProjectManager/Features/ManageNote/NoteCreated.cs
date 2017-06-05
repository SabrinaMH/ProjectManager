using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.AddNote
{
    public class NoteCreated : Event
    {
        public Guid Id { get; }
        public Guid TaskId { get; }
        public string Text { get; }

        public NoteCreated(Guid id, Guid taskId, string text)
        {
            Id = id;
            TaskId = taskId;
            Text = text;
        }
    }
}