using System;
using FluentAssertions;
using NUnit.Framework;
using ProjectManager.Domain;

namespace Test
{
    [TestFixture]
    public class NoteTest
    {
        [Test]
        public void Cannot_Create_A_Note_That_Isnt_Associated_To_A_Task()
        {
            Action instantiateNote = () => new Note(Guid.NewGuid(), Guid.Empty, "test");
            instantiateNote.ShouldThrow<ArgumentException>();
        }
    }
}