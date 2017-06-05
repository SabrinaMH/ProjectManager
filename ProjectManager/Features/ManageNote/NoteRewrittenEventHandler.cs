using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectManager.Features.ViewNote;

namespace ProjectManager.Features.AddNote
{
    public class NoteRewrittenEventHandler
    {
        private readonly string _storageFolder;

        public NoteRewrittenEventHandler()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public async Task Handle(NoteRewritten @event)
        {
            var fileName = string.Concat("noteViewModel-", @event.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            var fileContent = File.ReadAllText(path);
            var noteViewModel = JsonConvert.DeserializeObject<NoteViewModel>(fileContent);
            noteViewModel.Text = @event.NewText;
            var serializedViewModel = JsonConvert.SerializeObject(noteViewModel);
            File.WriteAllText(path, serializedViewModel);
        }
    }
}