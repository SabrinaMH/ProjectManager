using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectManager.Features.ManageNote;
using ProjectManager.Infrastructure;

namespace ProjectManager.Domain
{
    public class Note
    {
        public Guid Id => State.Id;
        public Guid TaskId => State.TaskId;
        private readonly List<Event> _events = new List<Event>();
        public NoteState State { get; }
        public ReadOnlyCollection<Event> Events => _events.AsReadOnly();

        public Note(Guid id, Guid taskId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException(nameof(text));
            if (id.Equals(Guid.Empty))
                throw new ArgumentException(nameof(id));
            if (taskId.Equals(Guid.Empty))
                throw new ArgumentException(nameof(taskId));
            
            State = new NoteState(id, taskId, text);
            var noteCreated = new NoteCreated(id, taskId, text);
            _events.Add(noteCreated);
        }

        public Note(NoteState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            State = state;
        }

        public void Rewrite(string newText)
        {
            State.Text = newText;
            var noteRewritten = new NoteRewritten(Id, State.Text);
            _events.Add(noteRewritten);
        }
    }

    public class NoteState
    {
        public Guid Id { get; }
        public Guid TaskId { get; }
        public string Text { get; set; }

        public NoteState(Guid id, Guid taskId, string text)
        {
            Id = id;
            TaskId = taskId;
            Text = text;
        }
    }
}