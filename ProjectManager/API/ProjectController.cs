using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ProjectManager.Domain;
using ProjectManager.Features.ViewProjectList;
using ProjectManager.Infrastructure;
using ProjectManager.Persistence;

namespace ProjectManager.API
{
    [RoutePrefix("project")]
    public class ProjectController : ApiController
    {
        private ProjectRepository _projectRepository;

        public ProjectController(EventBus eventBus)
        {
            _projectRepository = new ProjectRepository(eventBus);
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
        public async Task<HttpResponseMessage> Post([FromBody] ProjectInputModel model)
        {
            var id = Guid.NewGuid();
            var project = new Project(id, model.Title, model.Deadline);
            await _projectRepository.SaveAsync(project);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        } 
    }
}