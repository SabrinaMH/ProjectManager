﻿using System;
using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    public class UpdateTaskInputModel
    {
        [Required]
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        [Required]
        public string Priority { get; set; }
        [Required]
        public int SendEmailNumberOfDaysBeforeDeadline { get; set; }

        public UpdateTaskInputModel(string title, DateTime? deadline, string priority, int sendEmailNumberOfDaysBeforeDeadline)
        {
            Title = title;
            Deadline = deadline;
            Priority = priority;
            SendEmailNumberOfDaysBeforeDeadline = sendEmailNumberOfDaysBeforeDeadline;
        }
    }
}