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
using ProjectManager.Features.ViewTaskList;

namespace Test
{
    [TestFixture]
    public class TaskControllerTests
    {
        private IDisposable _webApp;
        private HttpClient _httpClient;
        private Fixture _fixture;
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
            _httpClient = new HttpClient();
            _fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            _webApp.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task Given_A_Task_Then_It_Can_Be_Fetched_Through_The_Api()
        {
            var taskInputModel = _fixture.Create<TaskInputModel>();
            var postTaskResponse = await _httpClient.PostAsJsonAsync(_taskEndpoint, taskInputModel);
            var postTaskResponseContent = await postTaskResponse.Content.ReadAsStringAsync();
            var taskId = JsonConvert.DeserializeObject<Guid>(postTaskResponseContent);

            var getTaskEndpoint = string.Format("{0}/{1}", _taskEndpoint, taskId);
            var getTaskResponse = await _httpClient.GetAsync(getTaskEndpoint);
            var getTasksResponseContent = await getTaskResponse.Content.ReadAsStringAsync();
            var taskViewModel = JsonConvert.DeserializeObject<TaskViewModel>(getTasksResponseContent);
            taskViewModel.Id.Should().Be(taskId);
        }
    }
}