using System;

namespace ProjectManager.Features.ViewProjectList
{
    public class GetTaskByIdQuery 
    {
        public Guid Id { get; }
        public Guid ProjectId { get; set; }
        public GetTaskByIdQuery(Guid id, Guid projectId)
        {
            Id = id;
            ProjectId = projectId;
        }
    }
}