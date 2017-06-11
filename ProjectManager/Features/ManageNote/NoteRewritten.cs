using System;
using ProjectManager.Infrastructure;

namespace ProjectManager.Features.ManageNote
{
    public class NoteRewritten : Event
    {
        public Guid Id { get; }
        public string NewText { get; }

        public NoteRewritten(Guid id, string newText)
        {
            Id = id;
            NewText = newText;
        }
    }
}