using System.Collections.Generic;

namespace ProjectManager.ViewProjectsFeature
{
    public class ProjectListViewModel
    {
        public List<ProjectListViewModel> ProjectViewModels { get; private set; }

        public ProjectListViewModel(List<ProjectListViewModel> projectViewModels)
        {
            ProjectViewModels = projectViewModels;
        }
    }
}