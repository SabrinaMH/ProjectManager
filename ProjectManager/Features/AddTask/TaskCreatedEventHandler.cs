using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectManager.Features.ViewTaskList;

namespace ProjectManager.Features.AddTask
{
    public class TaskCreatedEventHandler
    {
        private readonly string _storageFolder;

        public TaskCreatedEventHandler()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public async Task Handle(TaskCreated @event)
        {
            var taskViewModel = new TaskViewModel(@event.Id, @event.ProjectId, @event.Title, @event.Deadline, @event.Priority, false, false);
            var serializedViewModel = JsonConvert.SerializeObject(taskViewModel);
            var fileName = string.Concat("taskViewModel-", @event.Id, ".json");
            var path = Path.Combine(_storageFolder, "project-" + @event.ProjectId, fileName);
            File.WriteAllText(path, serializedViewModel);
        }
    }
}