using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ProjectManager.Domain;
using ProjectManager.Features.ViewNote;
using ProjectManager.Persistence;

namespace ProjectManager.API
{
    public class NoteController : ApiController
    {
        private readonly NoteRepository _noteRepository;
        private readonly NoteQueryService _noteQueryService;

        public NoteController(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
            _noteQueryService = new NoteQueryService();
        }

        [Route("task/{taskId}/note")]
        public HttpResponseMessage GetNoteForTask(Guid taskId)
        {
            var getNoteForTaskQuery = new GetNoteForTaskQuery(taskId);
            var note = _noteQueryService.Execute(getNoteForTaskQuery);
            return Request.CreateResponse(HttpStatusCode.OK, note);
        }

        [Route("note")]
        public async Task<HttpResponseMessage> Post(NoteInputModel model)
        {
            var id = Guid.NewGuid();
            var note = new Note(id, model.TaskId, model.Text);
            await _noteRepository.SaveAsync(note);
            return Request.CreateResponse(HttpStatusCode.Created, note.Id);
        }
    }
}