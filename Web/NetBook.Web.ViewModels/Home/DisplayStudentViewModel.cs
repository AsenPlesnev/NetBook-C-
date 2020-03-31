namespace NetBook.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class DisplayStudentViewModel : IMapFrom<StudentDisplayServiceModel>
    {
        public StudentServiceModel Student { get; set; }

        public List<GradesForSubjectViewModel> GradesForSubject { get; set; }

        public List<AbsenceServiceModel> Absences { get; set; }

        public List<RemarkServiceModel> Remarks { get; set; }

        public SchoolServiceModel School { get; set; }

        public int SubjectIndex { get; set; } = 1;
    }
}
