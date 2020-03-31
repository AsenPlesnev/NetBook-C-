namespace NetBook.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NetBook.Data.Common.Models;

    public class SubjectClass : BaseDeletableModel<string>
    {
        public SubjectClass()
        {
            this.CreatedOn = DateTime.UtcNow;

            this.Grades = new HashSet<Grade>();
        }

        public string SubjectId { get; set; }

        public Subject Subject { get; set; }

        [Required]
        public int Workload { get; set; }

        public string ClassId { get; set; }

        public Class Class { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
