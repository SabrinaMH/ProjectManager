using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace ProjectManager.Features.ViewNote
{
    public class NoteQueryService
    {
        private readonly string _storageFolder;

        public NoteQueryService()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }
        public NoteViewModel Execute(GetNoteForTaskQuery query)
        {
            foreach (var file in Directory.GetFiles(_storageFolder, "note-*"))
            {
                var fileContent = File.ReadAllText(file);
                var viewModel = JsonConvert.DeserializeObject<NoteViewModel>(fileContent);
                if (viewModel.TaskId.Equals(query.TaskId))
                {
                    return viewModel;
                }
            }
            return null; // Todo: Deal with this more appropriately
        }

        public NoteViewModel Execute(GetNoteByIdQuery query)
        {
            string file = Path.Combine(_storageFolder, "note-" + query.Id + ".json");
            string fileContent = File.ReadAllText(file);
            var viewModel = JsonConvert.DeserializeObject<NoteViewModel>(fileContent);
            return viewModel;
        }
    }
}