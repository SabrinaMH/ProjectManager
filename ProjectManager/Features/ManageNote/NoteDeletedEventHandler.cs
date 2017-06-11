using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using ProjectManager.Domain;
using Task = System.Threading.Tasks.Task;

namespace ProjectManager.Features.ManageNote
{
    public class NoteDeletedEventHandler
    {
        private readonly string _storageFolder;

        public NoteDeletedEventHandler()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public async Task Handle(NoteDeleted @event)
        {
            var fileName = string.Concat("task-", @event.TaskId, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            var serializedTask = File.ReadAllText(path);
            var taskState = JsonConvert.DeserializeObject<TaskState>(serializedTask);
            taskState.HasNote = false;
            File.WriteAllText(path, JsonConvert.SerializeObject(taskState));
        }
    }
}