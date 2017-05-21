using System;

namespace ProjectManager.Features.ViewNote
{
    public class NoteViewModel
    {
        public Guid Id { get; }
        public Guid TaskId { get; }
        public string Text { get; }

        public NoteViewModel(Guid id, Guid taskId, string text)
        {
            TaskId = taskId;
            Text = text;
            Id = id;
        }
    }
}