﻿using System;
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

namespace Test
{
    [TestFixture]
    public class AddTaskTests
    {
        private IDisposable _webApp;
        private HttpClient _httpClient;
        private Fixture _fixture;
        private string _projectEndpoint;

        [SetUp]
        public void SetUp()
        {
            var storageFolder = ConfigurationManager.AppSettings["storage.folder"];
            if (Directory.Exists(storageFolder))
            {
                Directory.Delete(storageFolder, true);
            }

            _webApp = WebApp.Start<Startup>("http://*:9000/");
            _projectEndpoint = "http://localhost:9000/project";
            _httpClient = new HttpClient();
            _fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            _webApp.Dispose();
        }

        [Test]
        public async Task When_Adding_A_Task_Then_It_Appears_In_The_Task_List()
        {
            var projectInputModel = _fixture.Create<ProjectInputModel>();
            var postProjectResponse = await _httpClient.PostAsJsonAsync(_projectEndpoint, projectInputModel);
            var postProjectResponseContent = await postProjectResponse.Content.ReadAsStringAsync();
            var projectId = JsonConvert.DeserializeObject<Guid>(postProjectResponseContent);

            var taskInputModel = _fixture.Create<TaskInputModel>();
            var endpoint = string.Format("{0}/{1}/task", _projectEndpoint, projectId);
            var postTaskResponse = await _httpClient.PostAsJsonAsync(endpoint, taskInputModel);
            var postTaskResponseContent = await postTaskResponse.Content.ReadAsStringAsync();
            var taskId = JsonConvert.DeserializeObject<Guid>(postTaskResponseContent);

            var getTasksResponse = await _httpClient.GetAsync(endpoint);
            var getTasksResponseContent = await getTasksResponse.Content.ReadAsStringAsync();
            var taskViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getTasksResponseContent);
            taskViewModels.Should().Contain(x => x.Id.Equals(taskId));
        }

        [Test]
        public async Task When_Adding_A_Task_With_A_Deadline_Then_It_Appears_In_The_Task_List()
        {
            var projectInputModel = _fixture.Create<ProjectInputModel>();
            var postProjectResponse = await _httpClient.PostAsJsonAsync(_projectEndpoint, projectInputModel);
            var postProjectResponseContent = await postProjectResponse.Content.ReadAsStringAsync();
            var projectId = JsonConvert.DeserializeObject<Guid>(postProjectResponseContent);

            var taskInputModel = _fixture.Create<TaskInputModel>();
            var endpoint = string.Format("{0}/{1}/task", _projectEndpoint, projectId);
            var postTaskResponse = await _httpClient.PostAsJsonAsync(endpoint, taskInputModel);
            var postTaskResponseContent = await postTaskResponse.Content.ReadAsStringAsync();
            var taskId = JsonConvert.DeserializeObject<Guid>(postTaskResponseContent);

            var getTasksResponse = await _httpClient.GetAsync(endpoint);
            var getTasksResponseContent = await getTasksResponse.Content.ReadAsStringAsync();
            var taskViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getTasksResponseContent);
            taskViewModels.Should().Contain(x => x.Id.Equals(taskId) && x.Deadline.Equals(taskInputModel.Deadline));
        }
    }
}