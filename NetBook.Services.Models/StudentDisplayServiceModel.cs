namespace NetBook.Services.Models
{
    using System.Collections.Generic;

    public class StudentDisplayServiceModel
    {
        public StudentServiceModel Student { get; set; }

        public List<GradesForSubjectServiceModel> GradesForSubject { get; set; }

        public List<AbsenceServiceModel> Absences { get; set; }

        public List<RemarkServiceModel> Remarks { get; set; }

        public SchoolServiceModel School { get; set; }
    }
}
