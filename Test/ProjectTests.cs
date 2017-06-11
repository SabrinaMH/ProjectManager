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
            var projectInputModel = _fixture.Create<AddProjectInputModel>();
            var projectId = await PostProjectAsync(projectInputModel);

            var getProjectsResponse = await _httpClient.GetAsync(_projectEndpoint);
            var getProjectsResponseContent = await getProjectsResponse.Content.ReadAsStringAsync();
            var projectViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getProjectsResponseContent);
            projectViewModels.Should().Contain(x => x.Id.Equals(projectId));
        }

        [Test]
        public async Task When_Adding_A_Project_With_A_Deadline_Then_It_Appears_In_The_Project_List()
        {
            var projectInputModel = _fixture.Create<AddProjectInputModel>();
            var projectId = await PostProjectAsync(projectInputModel);

            var getProjectsResponse = await _httpClient.GetAsync(_projectEndpoint);
            var getProjectsResponseContent = await getProjectsResponse.Content.ReadAsStringAsync();
            var projectViewModels = JsonConvert.DeserializeObject<List<ProjectViewModel>>(getProjectsResponseContent);
            projectViewModels.Should().Contain(x => x.Id.Equals(projectId) && x.Deadline.Equals(projectInputModel.Deadline));
        }

        [Test]
        public async Task Given_A_Project_Then_It_Can_Be_Fetched_Through_The_Api()
        {
            var projectInputModel = _fixture.Create<AddProjectInputModel>();
            var projectId = await PostProjectAsync(projectInputModel);

            var response = await _httpClient.GetAsync(_projectEndpoint + projectId);
            var content = await response.Content.ReadAsStringAsync();
            var projectViewModel = JsonConvert.DeserializeObject<ProjectViewModel>(content);
            projectViewModel.Id.Should().Be(projectId);
        }

        [Test]
        public async Task Given_A_Project_When_It_Is_Updated_Then_The_Changes_Are_Visible_When_Fetching_It()
        {
            var projectInputModel = _fixture.Create<AddProjectInputModel>();
            var projectId = await PostProjectAsync(projectInputModel);

            string specificProjectEndpoint = string.Format("{0}/{1}", _projectEndpoint, projectId);
            var updateProjectInputModel = _fixture.Create<UpdateProjectInputModel>();
            await _httpClient.PostAsJsonAsync(specificProjectEndpoint, updateProjectInputModel);

            var getProjectResponse = await _httpClient.GetAsync(specificProjectEndpoint);
            var projectViewModel = JsonConvert.DeserializeObject<ProjectViewModel>(await getProjectResponse.Content.ReadAsStringAsync());
            projectViewModel.Id.Should().Be(projectId);
            projectViewModel.Deadline.Should().Be(updateProjectInputModel.Deadline);
            projectViewModel.Title.Should().Be(updateProjectInputModel.Title);
        }

        private async Task<Guid> PostProjectAsync(AddProjectInputModel inputModel)
        {
            var postProjectResponse = await _httpClient.PostAsJsonAsync(_projectEndpoint, inputModel);
            var postProjectResponseContent = await postProjectResponse.Content.ReadAsStringAsync();
            var projectId = JsonConvert.DeserializeObject<Guid>(postProjectResponseContent);
            return projectId;
        }
    }
}