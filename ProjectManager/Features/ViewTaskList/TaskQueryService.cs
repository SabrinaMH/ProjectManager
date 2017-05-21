using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProjectManager.Features.ViewProjectList;

namespace ProjectManager.Features.ViewTaskList
{
    public class TaskQueryService
    {
        private readonly string _storageFolder;

        public TaskQueryService()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public List<TaskViewModel> Execute(GetTasksForProjectQuery query)
        {
            var viewModels = new List<TaskViewModel>();
            foreach (var file in Directory.GetFiles(_storageFolder, "taskViewModel-*"))
            {
                var fileContent = File.ReadAllText(file);
                var viewModel = JsonConvert.DeserializeObject<TaskViewModel>(fileContent);
                if (viewModel.ProjectId.Equals(query.ProjectId))
                {
                    viewModels.Add(viewModel);
                }
            }
            return viewModels;
        }

        public TaskViewModel Execute(GetTaskByIdQuery query)
        {
            string file = Path.Combine(_storageFolder, "taskViewModel-" + query.Id + ".json");
            string fileContent = File.ReadAllText(file);
            var viewModel = JsonConvert.DeserializeObject<TaskViewModel>(fileContent);
            return viewModel;
        }

        public NoteViewModel Execute(GetNoteForTaskQuery query)
        {
            string noteFolder = Path.Combine(_storageFolder, "task-" + query.TaskId);
            string file = Path.Combine(noteFolder, "noteViewModel.json");
            string fileContent = File.ReadAllText(file);
            var viewModel = JsonConvert.DeserializeObject<NoteViewModel>(fileContent);
            return viewModel;
        }
    }
}