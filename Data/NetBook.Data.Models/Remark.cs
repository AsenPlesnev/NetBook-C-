namespace NetBook.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using NetBook.Data.Common.Models;

    public class Remark : BaseDeletableModel<string>
    {
        public Remark()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string Text { get; set; }

        [Required]
        public string SubjectId { get; set; }

        public SubjectClass Subject { get; set; }

        [Required]
        public string StudentId { get; set; }

        public Student Student { get; set; }
    }
}
