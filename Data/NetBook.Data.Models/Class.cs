namespace NetBook.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NetBook.Data.Common.Models;
    using NetBook.Data.Models.Enums;

    public class Class : BaseDeletableModel<string>
    {
        public Class()
        {
            this.CreatedOn = DateTime.UtcNow;

            this.Subjects = new HashSet<SubjectClass>();

            this.Students = new HashSet<Student>();
        }

        [Required]
        public int ClassNumber { get; set; }

        [Required]
        public ClassLetter ClassLetter { get; set; }

        [Required]
        public int SchoolYearStart { get; set; }

        [Required]
        public int SchoolYearEnd { get; set; }

        [Required]
        public string ClassTeacherId { get; set; }

        public NetBookUser ClassTeacher { get; set; }

        public ICollection<SubjectClass> Subjects { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
