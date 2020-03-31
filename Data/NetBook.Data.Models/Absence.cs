namespace NetBook.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using NetBook.Data.Common.Models;

    public class Absence : BaseDeletableModel<string>
    {
        public Absence()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string SubjectId { get; set; }

        public SubjectClass Subject { get; set; }

        [Required]
        public string StudentId { get; set; }

        public Student Student { get; set; }
    }
}
