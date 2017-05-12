using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProjectManager.API;

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
            foreach (var file in Directory.GetFiles(_storageFolder, "projectViewModel-*"))
            {
                var fileContent = File.ReadAllText(file);
                var viewModel = JsonConvert.DeserializeObject<ProjectViewModel>(fileContent);
                viewModels.Add(viewModel);
            }
            return viewModels;
        }

        public ProjectViewModel Execute(GetProjectByIdQuery query)
        {
            var file = Directory.GetFiles(_storageFolder, "projectViewModel-" + query.Id + ".json").First();
            var fileContent = File.ReadAllText(file);
            var viewModel = JsonConvert.DeserializeObject<ProjectViewModel>(fileContent);
            return viewModel;
        }
    }
}