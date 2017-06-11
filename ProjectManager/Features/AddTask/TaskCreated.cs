﻿using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.AddTask
{
    public class TaskCreated : Event
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Priority { get; }
        public DateTime? Deadline { get; set; }
        public int SendEmailNumberOfDaysBeforeDeadline { get; }
        public Guid ProjectId { get; set; }

        public TaskCreated(Guid id, Guid projectId, string title, string priority, DateTime? deadline, int sendEmailNumberOfDaysBeforeDeadline)
        {
            Id = id;
            ProjectId = projectId;
            Title = title;
            Priority = priority;
            Deadline = deadline;
            SendEmailNumberOfDaysBeforeDeadline = sendEmailNumberOfDaysBeforeDeadline;
        }
    }
}