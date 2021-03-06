using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            foreach (var file in Directory.GetFiles(_storageFolder, "project-*"))
            {
                var fileContent = File.ReadAllText(file);
                var projectState = JsonConvert.DeserializeObject<ProjectState>(fileContent);
                projects.Add(new Project(projectState));
            }
            return projects;
        }

        public async Task SaveAsync(Project project)
        {
            var serializedProject = JsonConvert.SerializeObject(project.State);
            var fileName = string.Concat("project-", project.Id);
            var path = Path.Combine(_storageFolder, fileName + ".json");
            File.WriteAllText(path, serializedProject);
            foreach (var @event in project.Events)
            {
                await _eventBus.PublishAsync(@event);
            }
        }

        public Project Get(Guid id)
        {
            Project project = null;
            foreach (var file in Directory.GetFiles(_storageFolder, "project-*"))
            {
                var fileContent = File.ReadAllText(file);
                var projectState = JsonConvert.DeserializeObject<ProjectState>(fileContent);
                if (projectState.Id.Equals(id))
                {
                    project = new Project(projectState);
                }
            }

            if (project != null)
                return project;
            else
                throw new NotFoundException(id.ToString());
        }
    }
}