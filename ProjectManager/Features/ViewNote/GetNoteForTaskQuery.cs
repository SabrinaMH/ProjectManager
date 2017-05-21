using System;

namespace ProjectManager.Features.ViewNote
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