namespace NetBook.Services.Models
{
    using System;

    using NetBook.Data.Models;
    using NetBook.Services.Mapping;

    public class RemarkServiceModel : IMapTo<Remark>, IMapFrom<Remark>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string SubjectId { get; set; }

        public ClassSubjectServiceModel Subject { get; set; }

        public string StudentId { get; set; }

        public StudentServiceModel Student { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
