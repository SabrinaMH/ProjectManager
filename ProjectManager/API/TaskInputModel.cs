using System;
using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    public class TaskInputModel
    {
        [Required]
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        [Required]
        public string Priority { get; set; }
    }
}