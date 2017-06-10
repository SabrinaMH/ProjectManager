using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ProjectManager;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using FluentAssertions;
using ProjectManager.API;
using ProjectManager.Features.ViewProjectList;
using ProjectManager.Features.ViewTaskList;

namespace Test
{
    [TestFixture]
    public class TaskTests
    {
        private IDisposable _webApp;
        private HttpClient _httpClient;
        private Fixture _fixture;
        private string _taskEndpoint;
        private string _projectEndpoint;
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
            _taskEndpoint = "http://localhost:9000/task";
            _projectEndpoint = "http://localhost:9000/project";
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
        public async Task Given_A_Task_When_Adding_A_Note_Then_The_Task_Has_A_Note_Assigned_To_It()
        {
            var taskInputModel = _fixture.Create<TaskInputModel>();
            var taskId = await PostTask(taskInputModel);

            var noteInputModel = new NoteInputModel(taskId, _fixture.Create<string>());
            var postNoteResponse = await _httpClient.PostAsJsonAsync(_noteEndpoint, noteInputModel);
            await postNoteResponse.Content.ReadAsStringAsync();

            string taskEndpoint = string.Format("{0}/{1}", _taskEndpoint, taskId);
            var getTaskResponse = await _httpClient.GetAsync(taskEndpoint);
            string getTaskResponseContent = await getTaskResponse.Content.ReadAsStringAsync();
            var taskViewModel = JsonConvert.DeserializeObject<TaskViewModel>(getTaskResponseContent);
            taskViewModel.HasNote.Should().BeTrue();
        }

        [Test]
        public async Task When_Adding_A_Task_Then_It_Appears_In_The_Task_List()
        {
            var taskInputModel = _fixture.Create<TaskInputModel>();
            var taskId = await PostTask(taskInputModel);

            var tasksForAProjectEndpoint = string.Format("{0}/{1}/task", _projectEndpoint, taskInputModel.ProjectId);
            var getTasksResponse = await _httpClient.GetAsync(tasksForAProjectEndpoint);
            var getTasksResponseContent = await getTasksResponse.Content.ReadAsStringAsync();
            var taskViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getTasksResponseContent);
            taskViewModels.Should().Contain(x => x.Id.Equals(taskId));
        }

        [Test]
        public async Task When_Adding_A_Task_With_A_Deadline_Then_It_Appears_In_The_Task_List()
        {
            var taskInputModel = _fixture.Create<TaskInputModel>();
            var taskId = await PostTask(taskInputModel);

            var tasksForAProjectEndpoint = string.Format("{0}/{1}/task", _projectEndpoint, taskInputModel.ProjectId);
            var getTasksResponse = await _httpClient.GetAsync(tasksForAProjectEndpoint);
            var getTasksResponseContent = await getTasksResponse.Content.ReadAsStringAsync();
            var taskViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getTasksResponseContent);
            taskViewModels.Should().Contain(x => x.Id.Equals(taskId) && x.Deadline.Equals(taskInputModel.Deadline));
        }

        private async Task<Guid> PostTask(TaskInputModel inputModel)
        {
            var postTaskResponse = await _httpClient.PostAsJsonAsync(_taskEndpoint, inputModel);
            var postTaskResponseContent = await postTaskResponse.Content.ReadAsStringAsync();
            var taskId = JsonConvert.DeserializeObject<Guid>(postTaskResponseContent);
            return taskId;
        }
    }
}