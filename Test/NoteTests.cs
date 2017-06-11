using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ProjectManager;
using ProjectManager.API;
using ProjectManager.Domain;
using ProjectManager.Features.ViewNote;
using Task = System.Threading.Tasks.Task;

namespace Test
{
    [TestFixture]
    public class NoteTests
    {
        private IDisposable _webApp;
        private HttpClient _httpClient;
        private Fixture _fixture;
        private string _noteEndpoint;
        private string _taskEndpoint;

        [SetUp]
        public void SetUp()
        {
            var storageFolder = ConfigurationManager.AppSettings["storage.folder"];
            if (Directory.Exists(storageFolder))
            {
                foreach (var file in Directory.GetFiles(storageFolder))
                {
                    File.Delete(file);
                }
                Directory.Delete(storageFolder, true);
            }

            _webApp = WebApp.Start<Startup>("http://*:9000/");
            _taskEndpoint = "http://localhost:9000/task";
            _noteEndpoint = "http://localhost:9000/note";
            _httpClient = new HttpClient();
            _fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            _webApp.Dispose();
        }

        [Test]
        public void Cannot_Create_A_Note_That_Isnt_Associated_To_A_Task()
        {
            Action instantiateNote = () => new Note(Guid.NewGuid(), Guid.Empty, "test");
            instantiateNote.ShouldThrow<ArgumentException>();
        }

        [Test]
        public async Task Given_A_Task_Then_An_Associacated_Note_Can_Be_Created_Through_The_Api()
        {
            var taskInputModel = _fixture.Create<AddTaskInputModel>();
            var taskId = await PostTaskAsync(taskInputModel);
            var noteInputModel = new NoteInputModel(taskId, _fixture.Create<string>());
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, noteInputModel);
            postNoteResponse.IsSuccessStatusCode.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Task_With_A_Note_Then_The_Note_Can_Be_Fetched_Through_The_Api()
        {
            var taskInputModel = _fixture.Create<AddTaskInputModel>();
            var taskId = await PostTaskAsync(taskInputModel);
            var noteInputModel = new NoteInputModel(taskId, _fixture.Create<string>());
            var noteId = await PostNoteAsync(noteInputModel);

            var getNoteEndpoint = string.Format("http://localhost:9000/task/{0}/note", noteInputModel.TaskId);
            var getNoteResponse = await _httpClient.GetAsync(getNoteEndpoint);
            var getNoteResponseContent = await getNoteResponse.Content.ReadAsStringAsync();
            var noteViewModel = JsonConvert.DeserializeObject<NoteViewModel>(getNoteResponseContent);
            noteViewModel.Id.Should().Be(noteId);
        }

        [Test]
        public async Task When_Creating_A_Note_That_Is_Not_Associated_To_A_Task_Then_An_Error_Is_Returned()
        {
            var noteInputModel = new NoteInputModel(Guid.Empty, "Test");
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, noteInputModel);
            postNoteResponse.IsSuccessStatusCode.Should().BeFalse();
        }

        [Test]
        public async Task Given_A_Note_Then_It_Can_Be_Updated()
        {
            var taskInputModel = _fixture.Create<AddTaskInputModel>();
            var taskId = await PostTaskAsync(taskInputModel);
            var noteInputModel = new NoteInputModel(taskId, _fixture.Create<string>());
            var noteId = await PostNoteAsync(noteInputModel);

            var updateNoteInputModel = new UpdateNoteInputModel(_fixture.Create<string>());
            string specificNoteEndpoint = string.Format("{0}/{1}", _noteEndpoint, noteId);
            var result = await _httpClient.PostAsJsonAsync(specificNoteEndpoint, updateNoteInputModel);

            var noteResponse = await _httpClient.GetAsync(specificNoteEndpoint);
            string noteResponseContent = await noteResponse.Content.ReadAsStringAsync();
            var noteViewModel = JsonConvert.DeserializeObject<NoteViewModel>(noteResponseContent);
            noteViewModel.Text.Should().Be(updateNoteInputModel.Text);
        }

        [Test]
        public async Task Given_A_Note_When_It_Is_Deleted_Then_The_Note_Cannot_Be_Fetched()
        {
            var taskInputModel = _fixture.Create<AddTaskInputModel>();
            var taskId = await PostTaskAsync(taskInputModel);
            var noteInputModel = new NoteInputModel(taskId, _fixture.Create<string>());
            var noteId = await PostNoteAsync(noteInputModel);
            string specificNoteEndpoint = string.Format("{0}/{1}", _noteEndpoint, noteId);
            await _httpClient.DeleteAsync(specificNoteEndpoint);
            var noteResponse = await _httpClient.GetAsync(specificNoteEndpoint);
            noteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<Guid> PostTaskAsync(AddTaskInputModel inputModel)
        {
            var postTaskResponse = await _httpClient.PostAsJsonAsync(_taskEndpoint, inputModel);
            var postTaskResponseContent = await postTaskResponse.Content.ReadAsStringAsync();
            var taskId = JsonConvert.DeserializeObject<Guid>(postTaskResponseContent);
            return taskId;
        }

        private async Task<Guid> PostNoteAsync(NoteInputModel inputModel)
        {
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, inputModel);
            var postNoteResponseContent = await postNoteResponse.Content.ReadAsStringAsync();
            var noteId = JsonConvert.DeserializeObject<Guid>(postNoteResponseContent);
            return noteId;
        }
    }
}