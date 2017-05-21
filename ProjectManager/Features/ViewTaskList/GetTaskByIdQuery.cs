using System;

namespace ProjectManager.Features.ViewProjectList
{
    public class GetTaskByIdQuery 
    {
        public Guid Id { get; }
        public GetTaskByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}