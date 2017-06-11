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

namespace Test
{
    [TestFixture]
    public class ProjectTests
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
                foreach (var file in Directory.GetFiles(storageFolder))
                {
                    File.Delete(file);
                }
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
        public async Task When_Adding_A_Project_Then_It_Appears_In_The_Project_List()
        {
            var projectInputModel = _fixture.Create<ProjectInputModel>();
            var postProjectResponse = await _httpClient.PostAsJsonAsync(_projectEndpoint, projectInputModel);
            var postProjectResponseContent = await postProjectResponse.Content.ReadAsStringAsync();
            var projectId = JsonConvert.DeserializeObject<Guid>(postProjectResponseContent);

            var getProjectsResponse = await _httpClient.GetAsync(_projectEndpoint);
            var getProjectsResponseContent = await getProjectsResponse.Content.ReadAsStringAsync();
            var projectViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getProjectsResponseContent);
            projectViewModels.Should().Contain(x => x.Id.Equals(projectId));
        }

        [Test]
        public async Task When_Adding_A_Project_With_A_Deadline_Then_It_Appears_In_The_Project_List()
        {
            var projectInputModel = _fixture.Create<ProjectInputModel>();
            var postProjectResponse = await _httpClient.PostAsJsonAsync(_projectEndpoint, projectInputModel);
            var postProjectResponseContent = await postProjectResponse.Content.ReadAsStringAsync();
            var projectId = JsonConvert.DeserializeObject<Guid>(postProjectResponseContent);

            var getProjectsResponse = await _httpClient.GetAsync(_projectEndpoint);
            var getProjectsResponseContent = await getProjectsResponse.Content.ReadAsStringAsync();
            var projectViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getProjectsResponseContent);
            projectViewModels.Should().Contain(x => x.Id.Equals(projectId) && x.Deadline.Equals(projectInputModel.Deadline));
        }

        [Test]
        public async Task Given_A_Project_Then_It_Can_Be_Fetched_Through_The_Api()
        {
            var projectInputModel = _fixture.Create<ProjectInputModel>();
            var postProjectResponse = await _httpClient.PostAsJsonAsync(_projectEndpoint, projectInputModel);
            var postProjectResponseContent = await postProjectResponse.Content.ReadAsStringAsync();
            var projectId = JsonConvert.DeserializeObject<Guid>(postProjectResponseContent);

            var response = await _httpClient.GetAsync(_projectEndpoint + projectId);
            var content = await response.Content.ReadAsStringAsync();
            var projectViewModel = JsonConvert.DeserializeObject<ProjectViewModel>(content);
            projectViewModel.Id.Should().Be(projectId);
        }
    }
}