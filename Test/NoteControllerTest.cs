using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ProjectManager;
using ProjectManager.API;
using ProjectManager.Features.ViewNote;

namespace Test
{
    [TestFixture]
    public class NoteControllerTest
    {
        private IDisposable _webApp;
        private HttpClient _httpClient;
        private Fixture _fixture;
        private string _noteEndpoint;

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
        public async System.Threading.Tasks.Task Can_Create_A_Note_Through_The_Api()
        {
            var noteInputModel = _fixture.Create<NoteInputModel>();
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, noteInputModel);
            postNoteResponse.IsSuccessStatusCode.Should().BeTrue();
        }

        [Test]
        public async System.Threading.Tasks.Task Given_A_Note_Then_The_Note_Can_Be_Fetched_Through_The_Api()
        {
            var noteInputModel = _fixture.Create<NoteInputModel>();
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, noteInputModel);
            var postNoteResponseContent = await postNoteResponse.Content.ReadAsStringAsync();
            var noteId = JsonConvert.DeserializeObject<Guid>(postNoteResponseContent);

            var getNoteEndpoint = string.Format("http://localhost:9000/task/{0}/note", noteInputModel.TaskId);
            var getNoteResponse = await _httpClient.GetAsync(getNoteEndpoint);
            var getNoteResponseContent = await getNoteResponse.Content.ReadAsStringAsync();
            var noteViewModel = JsonConvert.DeserializeObject<NoteViewModel>(getNoteResponseContent);
            noteViewModel.Id.Should().Be(noteId);
        }

        [Test]
        public async System.Threading.Tasks.Task When_Creating_A_Note_That_Is_Not_Associated_To_A_Task_Then_An_Error_Is_Returned()
        {
            var noteInputModel = new NoteInputModel(Guid.Empty, "Test");
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, noteInputModel);
            postNoteResponse.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}