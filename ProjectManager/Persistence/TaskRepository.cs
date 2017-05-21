using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using ProjectManager.Domain;
using ProjectManager.Infrastructure;

namespace ProjectManager.Persistence
{
    public class TaskRepository
    {
        private readonly EventBus _eventBus;
        private readonly string _storageFolder;

        public TaskRepository(EventBus eventBus)
        {
            _eventBus = eventBus;
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
            if (!Directory.Exists(_storageFolder))
            {
                Directory.CreateDirectory(_storageFolder);
            }
        }
        //public List<Project> Get()
        //{
        //    var projects = new List<Project>();
        //    foreach (var file in Directory.GetFiles(_storageFolder, "project-*"))
        //    {
        //        var fileContent = File.ReadAllText(file);
        //        var projectState = JsonConvert.DeserializeObject<ProjectState>(fileContent);
        //        projects.Add(new Project(projectState));
        //    }
        //    return projects;
        //}

        public async System.Threading.Tasks.Task SaveAsync(Task task)
        {
            var serializedTask = JsonConvert.SerializeObject(task.State);
            var fileName = string.Concat("task-", task.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            File.WriteAllText(path, serializedTask);
            foreach (var @event in task.Events)
            {
                await _eventBus.PublishAsync(@event);
            }
        }
    }
}