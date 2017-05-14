using System;

namespace ProjectManager.Features.ViewProjectList
{
    public class GetNoteForTaskQuery
    {
        public Guid TaskId { get; }
        public GetNoteForTaskQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}