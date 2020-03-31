namespace NetBook.Services.Models
{
    using NetBook.Data.Models;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Mapping;

    public class GradeServiceModel : IMapTo<Grade>, IMapFrom<Grade>
    {
        public string Id { get; set; }

        public string SubjectId { get; set; }

        public ClassSubjectServiceModel Subject { get; set; }

        public string StudentId { get; set; }

        public StudentServiceModel Student { get; set; }

        public string GradeValue { get; set; }

        public Term Term { get; set; }

        public bool IsTermGrade { get; set; }
    }
}
