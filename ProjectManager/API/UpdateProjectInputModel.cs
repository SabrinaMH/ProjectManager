using System;
using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    public class UpdateProjectInputModel
    {
        [Required]
        public string Title { get; set; }

        public UpdateProjectInputModel(string title)
        {
            Title = title;
        }
    }
}