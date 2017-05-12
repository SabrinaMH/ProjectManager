using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ProjectManager.Features.ViewProjectList;
using ProjectManager.Features.ViewTaskList;
using ProjectManager.Persistence;

namespace ProjectManager.API
{
    [RoutePrefix("task")]
    public class TaskController : ApiController
    {
        private TaskRepository _taskRepository;

        [Route("")]
        public HttpResponseMessage Get()
        {
            var projectQueryService = new TaskQueryService();
            var getProjectsQuery = new GetTasksQuery();
            var projects = projectQueryService.Execute(getProjectsQuery);
            return Request.CreateResponse(HttpStatusCode.OK, projects);
        }

        [Route("")]
        public async Task<HttpResponseMessage> Post([FromBody] TaskInputModel model)
        {
            var id = Guid.NewGuid();
            var task = new Domain.Task(id, model.ProjectId, model.Title, model.Priority, model.Deadline);
            await _taskRepository.SaveAsync(task);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }
    }
}