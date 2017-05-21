using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ProjectManager.Domain;
using ProjectManager.Features.ViewProjectList;
using ProjectManager.Features.ViewTaskList;
using ProjectManager.Persistence;

namespace ProjectManager.API
{
    public class TaskController : ApiController
    {
        private readonly TaskRepository _taskRepository;
        private readonly TaskQueryService _taskQueryService;

        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _taskQueryService = new TaskQueryService();
        }

        [Route("task/{taskId}")]
        public HttpResponseMessage Get(Guid taskId)
        {
            var query = new GetTaskByIdQuery(taskId);
            var task = _taskQueryService.Execute(query);
            return Request.CreateResponse(HttpStatusCode.OK, task);
        }

        [Route("project/{projectId}/task")]
        [HttpGet]
        public HttpResponseMessage GetTasksForProject(Guid projectId)
        {
            var query = new GetTasksForProjectQuery(projectId);
            var tasks = _taskQueryService.Execute(query);
            return Request.CreateResponse(HttpStatusCode.OK, tasks);
        }

        [Route("task")]
        public async Task<HttpResponseMessage> Post([FromBody] TaskInputModel model)
        {
            var id = Guid.NewGuid();
            var task = new Domain.Task(id, model.ProjectId, model.Title, model.Priority, model.Deadline);
            await _taskRepository.SaveAsync(task);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }


        [Route("task/priority")]
        [HttpGet]
        public HttpResponseMessage GetPriorities()
        {
            var taskPriorities = TaskPriority.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, taskPriorities.Select(x => x.DisplayName));
        }
    }
}