namespace NetBook.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using NetBook.Data.Common.Models;
    using NetBook.Data.Models.Enums;

    public class Grade : BaseDeletableModel<string>
    {
        public Grade()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string SubjectId { get; set; }

        public SubjectClass Subject { get; set; }

        [Required]
        public string StudentId { get; set; }

        public Student Student { get; set; }

        [Required]
        public string GradeValue { get; set; }

        [Required]
        public Term Term { get; set; }

        [Required]
        public bool IsTermGrade { get; set; }
    }
}
