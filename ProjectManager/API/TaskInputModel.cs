using System;

namespace ProjectManager.API
{
    public class TaskInputModel
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        public string Priority { get; set; }
    }
}