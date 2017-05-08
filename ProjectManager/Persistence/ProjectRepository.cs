using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectManager.Domain;
using ProjectManager.Infrastructure;

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
            foreach (var file in Directory.GetFiles(_storageFolder, "project-*"))
            {
                var fileContent = File.ReadAllText(file);
                var projectState = JsonConvert.DeserializeObject<ProjectState>(fileContent);
                projects.Add(new Project(projectState));
            }
            return projects;
        }

        public async Task Save(Project project)
        {
            var serializedProject = JsonConvert.SerializeObject(project.State);
            var fileName = string.Concat("project-", project.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            File.WriteAllText(path, serializedProject);
            foreach (var @event in project.Events)
            {
                await _eventBus.PublishAsync(@event);
            }
        }
    }
}