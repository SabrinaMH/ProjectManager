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
        private TaskQueryService _taskQueryService;

        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _taskQueryService = new TaskQueryService();
        }

        [Route("project/{projectId}/task/{taskId}")]
        public HttpResponseMessage Get(Guid projectId, Guid taskId)
        {
            var query = new GetTaskByIdQuery(taskId, projectId);
            var task = _taskQueryService.Execute(query);
            return Request.CreateResponse(HttpStatusCode.OK, task);
        }

        [Route("project/{projectId}/task")]
        public HttpResponseMessage Get(Guid projectId)
        {
            var query = new GetTasksForProjectQuery(projectId);
            var tasks = _taskQueryService.Execute(query);
            return Request.CreateResponse(HttpStatusCode.OK, tasks);
        }

        [Route("project/{projectId}/task")]
        public async Task<HttpResponseMessage> Post(Guid projectId, [FromBody] TaskInputModel model)
        {
            var id = Guid.NewGuid();
            var task = new Domain.Task(id, projectId, model.Title, model.Priority, model.Deadline);
            await _taskRepository.SaveAsync(task);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        [Route("task/{taskId}/note")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetNote(Guid taskId)
        {
            var getNoteForTaskQuery = new GetNoteForTaskQuery(taskId);
            var note = _taskQueryService.Execute(getNoteForTaskQuery);
            return Request.CreateResponse(HttpStatusCode.Created, note);
        }

        [Route("task/{taskId}/note")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostNote(Guid taskId)
        {
            // TODO: Do such that one shouldnt have to go through project to do something with a task => simplifies tests in that we do not need to create a project to test task related stuff
            _taskRepository.GetById(taskId);
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