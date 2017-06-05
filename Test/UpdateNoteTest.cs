using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class UpdateNoteTest
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
        public async Task Given_A_Note_Then_It_Can_Be_Updated()
        {
            var noteInputModel = _fixture.Create<NoteInputModel>();
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, noteInputModel);
            var postNoteResponseContent = await postNoteResponse.Content.ReadAsStringAsync();
            var noteId = JsonConvert.DeserializeObject<Guid>(postNoteResponseContent);

            var updateNoteInputModel = new UpdateNoteInputModel(_fixture.Create<string>());
            string specificNoteEndpoint = string.Format("{0}/{1}", _noteEndpoint, noteId);
            var result = await _httpClient.PostAsJsonAsync(specificNoteEndpoint, updateNoteInputModel);

            var noteResponse = await _httpClient.GetAsync(specificNoteEndpoint);
            string noteResponseContent = await noteResponse.Content.ReadAsStringAsync();
            var noteViewModel = JsonConvert.DeserializeObject<NoteViewModel>(noteResponseContent);
            noteViewModel.Text.Should().Be(updateNoteInputModel.Text);
        }
    }
}