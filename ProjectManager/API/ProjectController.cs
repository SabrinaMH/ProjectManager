using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ProjectManager.Domain;
using ProjectManager.Features.ViewProjectList;
using ProjectManager.Persistence;

namespace ProjectManager.API
{
    [RoutePrefix("project")]
    public class ProjectController : ApiController
    {
        private readonly ProjectRepository _projectRepository;

        public ProjectController(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [Route("")]
        public HttpResponseMessage Get()
        {
            var projectQueryService = new ProjectQueryService();
            var getProjectsQuery = new GetProjectsQuery();
            var projects = projectQueryService.Execute(getProjectsQuery);
            return Request.CreateResponse(HttpStatusCode.OK, projects);
        }

        [Route("{id}")]
        public HttpResponseMessage Get(string id)
        {
            var projectQueryService = new ProjectQueryService();
            var getProjectByIdQuery = new GetProjectByIdQuery(id);
            var project = projectQueryService.Execute(getProjectByIdQuery);
            return Request.CreateResponse(HttpStatusCode.OK, project);
        }

        [Route("")]
        public async Task<HttpResponseMessage> Post([FromBody] AddProjectInputModel model)
        {
            var id = Guid.NewGuid();
            var project = new Project(id, model.Title);
            await _projectRepository.SaveAsync(project);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        [Route("{id}")]
        public async Task<HttpResponseMessage> Post(Guid id, [FromBody] UpdateProjectInputModel model)
        {
            var project = _projectRepository.Get(id);
            project.Update(model.Title);
            await _projectRepository.SaveAsync(project);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}