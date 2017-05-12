using System;

namespace ProjectManager.Features.ViewTaskList
{
    public class GetTasksForProjectQuery
    {
        public Guid ProjectId { get; }

        public GetTasksForProjectQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}