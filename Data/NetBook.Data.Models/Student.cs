namespace NetBook.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NetBook.Data.Common.Models;

    public class Student : BaseDeletableModel<string>
    {
        public Student()
        {
            this.CreatedOn = DateTime.UtcNow;

            this.Grades = new HashSet<Grade>();

            this.Remarks = new HashSet<Remark>();

            this.Absences = new HashSet<Absence>();
        }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PIN { get; set; }

        [Required]
        public string Citizenship { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ClassId { get; set; }

        [Required]
        public Class Class { get; set; }

        public ICollection<Grade> Grades { get; set; }

        public ICollection<Remark> Remarks { get; set; }

        public ICollection<Absence> Absences { get; set; }
    }
}
