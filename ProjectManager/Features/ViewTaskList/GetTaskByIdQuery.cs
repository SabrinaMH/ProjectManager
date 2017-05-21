using System;

namespace ProjectManager.Features.ViewTaskList
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