using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using ProjectManager.Domain;
using ProjectManager.Infrastructure;

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

        public async System.Threading.Tasks.Task SaveAsync(Note note)
        {
            var serializedTask = JsonConvert.SerializeObject(note.State);
            var fileName = string.Concat("note-", note.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            File.WriteAllText(path, serializedTask);
            foreach (var @event in note.Events)
            {
                await _eventBus.PublishAsync(@event);
            }
        }
    }
}