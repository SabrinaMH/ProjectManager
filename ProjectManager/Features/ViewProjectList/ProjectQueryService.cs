using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            foreach (var file in Directory.GetFiles(_storageFolder, "projectViewModel-*"))
            {
                var fileContent = File.ReadAllText(file);
                var viewModel = JsonConvert.DeserializeObject<ProjectViewModel>(fileContent);
                viewModels.Add(viewModel);
            }
            return viewModels;
        }
    }
}