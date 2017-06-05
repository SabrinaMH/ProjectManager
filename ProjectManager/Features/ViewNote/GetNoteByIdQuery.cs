using System;

namespace ProjectManager.Features.ViewNote
{
    public class GetNoteByIdQuery
    {
        public Guid Id { get; }

        public GetNoteByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}