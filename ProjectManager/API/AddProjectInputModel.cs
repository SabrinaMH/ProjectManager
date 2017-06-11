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
    }
}