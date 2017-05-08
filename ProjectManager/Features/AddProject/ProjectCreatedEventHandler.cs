using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectManager.Features.ViewProjectList;

namespace ProjectManager.Features.AddProject
{
    public class ProjectAddedEventHandler
    {
        private readonly string _storageFolder;

        public ProjectAddedEventHandler()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public async Task Handle(ProjectCreated @event)
        {
            var projectViewModel = new ProjectViewModel(@event.Id, @event.Title);
            var serializedViewModel = JsonConvert.SerializeObject(projectViewModel);
            var fileName = string.Concat("projectViewModel-", @event.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            File.WriteAllText(path, serializedViewModel);
        }
    }
}