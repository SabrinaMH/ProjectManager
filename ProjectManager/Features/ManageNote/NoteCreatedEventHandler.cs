﻿using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectManager.Features.ViewNote;

namespace ProjectManager.Features.AddNote
{
    public class NoteCreatedEventHandler
    {
        private readonly string _storageFolder;

        public NoteCreatedEventHandler()
        {
            _storageFolder = ConfigurationManager.AppSettings["storage.folder"];
        }

        public async Task Handle(NoteCreated @event)
        {
            var noteViewModel = new NoteViewModel(@event.Id, @event.TaskId, @event.Text);
            var serializedViewModel = JsonConvert.SerializeObject(noteViewModel);
            var fileName = string.Concat("noteViewModel-", @event.Id, ".json");
            var path = Path.Combine(_storageFolder, fileName);
            File.WriteAllText(path, serializedViewModel);
        }
    }
}