using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectManager.Domain;
using ProjectManager.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace ProjectManager.Persistence
{
    public class ProjectRepository
    {
        private readonly EventBus _eventBus;
        private readonly string _storageFolder;

        public ProjectRepository(EventBus eventBus)
        {
            _eventBus = eventBus;
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
            if (!Directory.Exists(_storageFolder))
            {
                Directory.CreateDirectory(_storageFolder);
            }
        }

        public List<Project> Get()
        {
            var projects = new List<Project>();
            foreach (var directory in Directory.GetDirectories(_storageFolder, "project-*"))
            {
                var fileName = directory.Substring(directory.LastIndexOf('/') + 1);
                var fileContent = File.ReadAllText(Path.Combine(directory, fileName + ".json"));
                var projectState = JsonConvert.DeserializeObject<ProjectState>(fileContent);
                projects.Add(new Project(projectState));
            }
            return projects;
        }

        public async Task SaveAsync(Project project)
        {
            var serializedProject = JsonConvert.SerializeObject(project.State);
            var fileName = string.Concat("project-", project.Id);
            string projectFolder = Path.Combine(_storageFolder, fileName);
            Directory.CreateDirectory(projectFolder);
            var path = Path.Combine(projectFolder, fileName + ".json");
            File.WriteAllText(path, serializedProject);
            foreach (var @event in project.Events)
            {
                await _eventBus.PublishAsync(@event);
            }
        }
    }
}