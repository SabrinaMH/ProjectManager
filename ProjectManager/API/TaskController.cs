using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ProjectManager.Domain;
using ProjectManager.Features.ViewTaskList;
using ProjectManager.Persistence;
using Task = System.Threading.Tasks.Task;

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
        [HttpGet]
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
        public async Task<HttpResponseMessage> Post([FromBody] AddTaskInputModel model)
        {
            var id = Guid.NewGuid();
            var task = new Domain.Task(id, model.ProjectId, model.Title, model.Priority, model.Deadline);
            await _taskRepository.SaveAsync(task);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        [Route("task/{id}")]
        public async Task<HttpResponseMessage> Post(Guid id, [FromBody] UpdateTaskInputModel model)
        {
            var task = _taskRepository.Get(id);
            task.Update(model.Title, model.Priority, model.Deadline);
            await _taskRepository.SaveAsync(task);
            return Request.CreateResponse(HttpStatusCode.OK);
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