namespace NetBook.Services.Models
{
    using System.Collections.Generic;

    public class GradesForSubjectServiceModel
    {
        public ClassSubjectServiceModel Subject { get; set; }

        public List<string> FirstTermGrades { get; set; }

        public string FirstTermGrade { get; set; }

        public List<string> SecondTermGrades { get; set; }

        public string SecondTermGrade { get; set; }
    }
}
