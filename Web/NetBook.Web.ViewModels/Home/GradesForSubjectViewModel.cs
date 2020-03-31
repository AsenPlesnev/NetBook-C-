namespace NetBook.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class GradesForSubjectViewModel : IMapFrom<GradesForSubjectServiceModel>
    {
        public ClassSubjectServiceModel Subject { get; set; }

        public List<string> FirstTermGrades { get; set; }

        public string FirstTermGrade { get; set; }

        public List<string> SecondTermGrades { get; set; }

        public string SecondTermGrade { get; set; }
    }
}
