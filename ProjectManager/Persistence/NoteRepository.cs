using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using ProjectManager.Domain;
using ProjectManager.Features.AddNote;
using ProjectManager.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace ProjectManager.Persistence
{
    public class NoteRepository
    {
        private readonly EventBus _eventBus;
        private readonly string _storageFolder;

        public NoteRepository(EventBus eventBus)
        {
            _eventBus = eventBus;
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
            if (!Directory.Exists(_storageFolder))
            {
                Directory.CreateDirectory(_storageFolder);
            }
        }

        public async Task SaveAsync(Note note)
        {
            var serializedNote = JsonConvert.SerializeObject(note.State);
            var fileName = string.Concat("note-", note.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            File.WriteAllText(path, serializedNote);
            foreach (var @event in note.Events)
            {
                await _eventBus.PublishAsync(@event);
            }
        }

        public async Task DeleteAsync(Note note)
        {
            var fileName = string.Concat("note-", note.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            File.Delete(path);
            var @event = new NoteDeleted(note.Id, note.TaskId);
            await _eventBus.PublishAsync(@event);
        }

        public Note Get(Guid id)
        {
            Note note = null;
            foreach (var file in Directory.GetFiles(_storageFolder, "note-*"))
            {
                var fileContent = File.ReadAllText(file);
                var noteState = JsonConvert.DeserializeObject<NoteState>(fileContent);
                if (noteState.Id.Equals(id))
                {
                    note = new Note(noteState);
                }
            }

            if (note != null)
                return note;
            else 
                throw new NotFoundException(id.ToString());
        }
    }
}