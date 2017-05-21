using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ProjectManager.Domain;
using ProjectManager.Infrastructure;
using ProjectManager.Persistence;

namespace Test
{
    [TestFixture]
    public class ProjectRepositoryTests
    {
        private ProjectRepository _projectRepository;

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
            var eventBus = new EventBus();
            _projectRepository = new ProjectRepository(eventBus);

        }

        [Test]
        public async System.Threading.Tasks.Task Can_Serialize_And_Then_Deserialize_ProjectStates()
        {
            var fixture = new Fixture();
            var project = fixture.Create<Project>();
            await _projectRepository.SaveAsync(project);

            var projects = _projectRepository.Get();
            projects.Should().ContainSingle(x => x.Equals(project));
        }
    }
}
