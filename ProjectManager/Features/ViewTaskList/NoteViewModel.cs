using System;

namespace ProjectManager.Features.ViewTaskList
{
    public class NoteViewModel
    {
        public Guid TaskId { get; }
        public string Text { get; }

        public NoteViewModel(Guid taskId, string text)
        {
            TaskId = taskId;
            Text = text;
        }
    }
}