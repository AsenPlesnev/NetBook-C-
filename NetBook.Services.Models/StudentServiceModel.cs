namespace NetBook.Services.Models
{
    using System;
    using System.Collections.Generic;

    using NetBook.Data.Models;
    using NetBook.Services.Mapping;

    public class StudentServiceModel : IMapTo<Student>, IMapFrom<Student>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string PIN { get; set; }

        public string Citizenship { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Town { get; set; }

        public string Municipality { get; set; }

        public string Region { get; set; }

        public string ClassId { get; set; }

        public ClassServiceModel Class { get; set; }

        public string Address { get; set; }

        public List<GradeServiceModel> Grades { get; set; }

        public List<RemarkServiceModel> Remarks { get; set; }

        public List<AbsenceServiceModel> Absences { get; set; }
    }
}
