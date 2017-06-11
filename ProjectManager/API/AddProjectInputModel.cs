using System;
using System.Runtime.Serialization;
using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    [DataContract]
    public class AddProjectInputModel
    {
        [DataMember]
        [Required]
        public string Title { get; set; }

        [DataMember]
        public DateTime? Deadline { get; set; }

        //public ProjectInputModel(string title)
        //    : this(title, null)
        //{
        //}

        //public ProjectInputModel(string title, DateTime? deadline)
        //{
        //    Title = title;
        //    Deadline = deadline;
        //}
    }
}