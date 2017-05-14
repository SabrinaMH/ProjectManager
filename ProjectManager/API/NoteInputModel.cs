using Microsoft.Build.Framework;

namespace ProjectManager.API
{
    public class NoteInputModel
    {
        [Required]
        public string Text { get; set; }
        
    }
}