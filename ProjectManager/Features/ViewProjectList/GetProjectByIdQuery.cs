namespace ProjectManager.Features.ViewProjectList
{
    public class GetProjectByIdQuery 
    {
        public string Id { get; }

        public GetProjectByIdQuery(string id)
        {
            Id = id;
        }
    }
}