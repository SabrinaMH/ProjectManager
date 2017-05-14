using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ProjectManager.Features.ViewProjectList
{
    public class ProjectQueryService
    {
        private readonly string _storageFolder;

        public ProjectQueryService()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public List<ProjectViewModel> Execute(GetProjectsQuery query)
        {
            var viewModels = new List<ProjectViewModel>();
            foreach (var directory in Directory.GetDirectories(_storageFolder, "project-*"))
            {
                var fileName = directory.Substring(directory.LastIndexOf('\\') + 1);
                var fileContent = File.ReadAllText(Path.Combine(directory, fileName + ".json"));
                var viewModel = JsonConvert.DeserializeObject<ProjectViewModel>(fileContent);
                viewModels.Add(viewModel);
            }
            return viewModels;
        }

        public ProjectViewModel Execute(GetProjectByIdQuery query)
        {
            var directory = Path.Combine(_storageFolder, "project-" + query.Id);
            var file = Directory.GetFiles(directory, "projectViewModel-" + query.Id + ".json").First();
            var fileContent = File.ReadAllText(file);
            var viewModel = JsonConvert.DeserializeObject<ProjectViewModel>(fileContent);
            return viewModel;
        }
    }
}