using System;
using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    public class NoteInputModel
    {
        [Required]
        public Guid TaskId { get; set; }
        public string Text { get; set; }

        public NoteInputModel(Guid taskId, string text)
        {
            TaskId = taskId;
            Text = text;
        }
        
    }
}