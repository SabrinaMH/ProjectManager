using System;
using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    public class AddTaskInputModel
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        [Required]
        public string Priority { get; set; }

        [Required]
        public int SendEmailNumberOfDaysBeforeDeadline { get; set; }
    }
}