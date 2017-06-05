using System;

namespace ProjectManager.API
{
    public class UpdateNoteInputModel
    {
        public string Text { get; }

        public UpdateNoteInputModel(string text)
        {
            Text = text;
        }
    }
}