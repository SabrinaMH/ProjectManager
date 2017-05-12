using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace ProjectManager.Features.ViewTaskList
{
    public class TaskQueryService
    {
        private readonly string _storageFolder;

        public TaskQueryService()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public List<TaskViewModel> Execute(GetTasksQuery query)
        {
            var viewModels = new List<TaskViewModel>();
            foreach (var file in Directory.GetFiles(_storageFolder, "taskViewModel-*"))
            {
                var fileContent = File.ReadAllText(file);
                var viewModel = JsonConvert.DeserializeObject<TaskViewModel>(fileContent);
                viewModels.Add(viewModel);
            }
            return viewModels;
        }
    }
}