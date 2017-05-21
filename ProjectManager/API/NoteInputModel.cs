using System;
using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    public class NoteInputModel
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public string Text { get; set; }
        
    }
}