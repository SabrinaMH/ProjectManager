using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ProjectManager.Features.ViewTaskList;
using ProjectManager.Persistence;

namespace ProjectManager.API
{
    public class TaskController : ApiController
    {
        private TaskRepository _taskRepository;

        [Route("project/{projectId}")]
        public HttpResponseMessage Get(Guid projectId)
        {
            var projectQueryService = new TaskQueryService();
            var query = new GetTasksForProjectQuery(projectId);
            var projects = projectQueryService.Execute(query);
            return Request.CreateResponse(HttpStatusCode.OK, projects);
        }

        [Route("task")]
        public async Task<HttpResponseMessage> Post([FromBody] TaskInputModel model)
        {
            var id = Guid.NewGuid();
            var task = new Domain.Task(id, model.ProjectId, model.Title, model.Priority, model.Deadline);
            await _taskRepository.SaveAsync(task);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }
    }
}